using DrwgTronics.Uatu.Models;
using DrwgTronics.Uatu.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

namespace DrwgTronics.Uatu.Components
{
    public class Watcher : IProgress<LineCountProgress>, IFolderMonitor
    {
        const double PollTimeMilliseconds = 10000.0;

        ILog _view;
        IFolderModel _model;
        ILineCounter _counter;
        IBulkLoader _loader;
        string _directory;
        string _filter;

        public event EventHandler<FileEvent> FolderChanged;
        public event EventHandler<string> Status;

        public Watcher(ILog view, IFolderModel model, ILineCounter counter, IBulkLoader loader, string folder, string filter)
        {
            _view = view;
            _model = model;
            _counter = counter;
            _counter.FolderPath = folder;
            _loader = loader;
            _directory = folder;
            _filter = filter;
        }

        public void Start()
        {
            if (!Directory.Exists(_directory)) throw new DirectoryNotFoundException(_directory);
            var info = new DirectoryInfo(_directory);

            _model.Clear();
            OnStatus(this, "Getting initial directory list.");
            _loader.Load(_directory, _filter, _model);

            OnStatus(this, "Getting baseline counts for list.");
            List<FileEvent> initialpopulation = _model.AsFileEvents(FileEventType.Initialize);
            var batchCountTask = _counter.CountBatchAsync(initialpopulation, this);
            batchCountTask.Wait();

            OnStatus(this, "Monitoring for changes.");
            List<FileEvent> addUpdateBatch;
            List<FileEvent> deleteBatch;
            DateTime lastScanStart;

            for (int generation = 1; true ; generation++)
            {
                lastScanStart = DateTime.Now;

                try
                {
                    //_model.EnforceLocks = false;
                    Scan(_model, info, generation, out addUpdateBatch, out deleteBatch);
                }
                finally
                {
                    //_model.EnforceLocks = true;
                }

                Task addUpdateCounterTask = _counter.CountBatchAsync(addUpdateBatch, this);

                foreach (FileEvent e in deleteBatch)
                {
                    OnFolderChanged(this, e);
                }
                //addUpdateCounterTask.Wait();

                double timeLeftInCycle = PollTimeMilliseconds - (DateTime.Now - lastScanStart).TotalMilliseconds;
                
                if (timeLeftInCycle > 1.0) Thread.Sleep((int)(timeLeftInCycle)); 
            }
        }

        private void Scan(IFolderModel model, DirectoryInfo info, int generation, out List<FileEvent> addUpdateBatch, out List<FileEvent> deleteBatch)
        {
            addUpdateBatch = new List<FileEvent>(100);

            foreach (FileInfo fi in info.EnumerateFiles(_filter)) // throws ArgumentException if filter is bad
            {
                FileEntry foundEntry;

                if (model.TryGetValue(fi.Name, out foundEntry))
                {
                    foundEntry.Generation = generation;

                    if (fi.LastWriteTimeUtc > foundEntry.ModifiedDate)
                    {
                        foundEntry.ModifiedDate = fi.LastWriteTimeUtc;
                        var fileEvent = new FileEvent(FileEventType.Update, foundEntry);
                        addUpdateBatch.Add(fileEvent);
                    }
                }
                else
                {
                    var newEntry = new FileEntry(fi.Name, fi.LastWriteTimeUtc, FileEntry.NotCounted, generation);
                    model.AddFile(newEntry);
                    var fileEvent = new FileEvent(FileEventType.Create, newEntry);
                    addUpdateBatch.Add(fileEvent);
                }
            }

            var filesMissingInThisGeneration = model.FromGeneration(generation - 1);
            deleteBatch = new List<FileEvent>(100);

            foreach (FileEntry fileEntry in filesMissingInThisGeneration.ToArray())
            {
                var fileEvent = new FileEvent(FileEventType.Delete, fileEntry);
                model.DeleteFile(fileEntry);
                deleteBatch.Add(fileEvent);
            }
        }

        public Task StartAsync()
        {
            return Task.Run(() => Start());
        }

        public void Stop()
        {

        }

        public void Report(LineCountProgress value)
        {
            FileEvent fileEvent = value.FileEvent;

            if (value.Status == LineCountStatus.Success)
            {
                FileEventType t = fileEvent.EventType;

                if (t == FileEventType.Initialize)
                {
                    fileEvent.FileEntry.LineCount = value.Count;
                }

                if (t == FileEventType.Update)
                {
                    fileEvent.OldCount = fileEvent.FileEntry.LineCount;
                }

                if (t == FileEventType.Create || t == FileEventType.Update)
                {
                    fileEvent.FileEntry.LineCount = value.Count;
                    OnFolderChanged(this, fileEvent);
                }

                // deletes don't come back through progress and are logged all at once elsewhere
            }
            else if (value.Status == LineCountStatus.TimedOut)
            {
                // When and if the file is released, it will be logged as an update.
            }
        }

        protected virtual void OnFolderChanged(object source, FileEvent e)
        {
            EventHandler<FileEvent> handler = FolderChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnStatus(object source, string e)
        {
            EventHandler<string> handler = Status;

            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

}
