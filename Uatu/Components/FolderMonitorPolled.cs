using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrwgTronics.Uatu.Models;
using System.IO;

namespace DrwgTronics.Uatu.Components
{
    public class FolderMonitorPolled : IFolderMonitor
    {
        public event EventHandler<FileEvent> FolderChanged;

        public FolderMonitorPolled(IFolderModel model, string folder, string filter)
        {
        }

        public void Start()
        {
            
        }

        public Task StartAsync()
        {
            return null;
        }

        public void Stop()
        {
        }

        protected virtual void OnFolderChanged(object source, FileSystemEventArgs e)
        {
            EventHandler<FileEvent> handler = FolderChanged;

            if (handler != null)
            {
                var fileEntry = new FileEntry(e.Name);
                FileEvent fileEvent = null;

                if (e.ChangeType == WatcherChangeTypes.Created)
                    fileEvent = new FileEvent(FileEventType.Create, fileEntry);
                else if (e.ChangeType == WatcherChangeTypes.Changed)
                    fileEvent = new FileEvent(FileEventType.Update, fileEntry);
                else if (e.ChangeType == WatcherChangeTypes.Deleted)
                    fileEvent = new FileEvent(FileEventType.Create, fileEntry);
                else if (e.ChangeType == WatcherChangeTypes.Renamed)
                {
                    fileEntry.Name = (e as RenamedEventArgs).OldName;
                    fileEvent = new FileEvent(FileEventType.Rename, fileEntry, newName: e.Name);
                }

                if (fileEvent != null)
                {
                    handler(this, fileEvent);
                }
            }
        }

        void Scan(IFolderModel model, string folder, string filter, int generation)
        {
            if (!Directory.Exists(folder)) throw new DirectoryNotFoundException(folder);

            var info = new DirectoryInfo(folder);

            foreach (FileInfo fi in info.EnumerateFiles(filter)) // throws ArgumentException if filter is bad
            {
                
            }
        }
    }
}
