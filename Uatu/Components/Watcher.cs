using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrwgTronics.Uatu.Models;
using DrwgTronics.Uatu.Views;
using System.Threading;
using System.Threading.Tasks;

namespace DrwgTronics.Uatu.Components
{
    public class Watcher : IProgress<LineCountProgress>
    {
        ILog _view;
        IFolderModel _model;
        ILineCounter _counter;
        IBulkLoader _loader;
        string _directory;
        string _filter;

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
            _model.Clear();
            _loader.Load(_directory, _filter, _model);
            List<FileEvent> initialpopulation = _model.AsFileEvents(FileEventType.Initialize);
            var x = _counter.CountBatchAsync(initialpopulation, this);
            x.Wait();
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
            if (value.Status == LineCountStatus.Success)
            {
                if (value.FileEvent.EventType == FileEventType.Initialize)
                {
                    value.FileEvent.FileEntry.LineCount = value.Count;
                }
                //else if (value.FileEvent.EventType == FileEventType.Create)
                //{
                //    _view.LogEvent(value.FileEvent);
                //    _model.UpdateFile(value.FileEvent.FileEntry);
                //}
            }
        }
    }
}
