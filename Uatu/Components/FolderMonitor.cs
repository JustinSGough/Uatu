using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrwgTronics.Uatu.Models;
using System.Security.Permissions;

namespace DrwgTronics.Uatu.Components
{
    /// <summary>
    /// Encapsulates folder monitoring. 
    /// </summary>
    public class FolderMonitor : IFolderMonitor
    {
        FileSystemWatcher _watcher;

        public event EventHandler<FileEvent> FolderChanged;

        public FolderMonitor(IFolderModel model, string folder, string filter)
        {
            if (_watcher != null) _watcher.Dispose();
            
            _watcher = new FileSystemWatcher(folder, filter);
            _watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
            _watcher.InternalBufferSize = 65536;
            _watcher.Created += OnFolderChanged;
            
            _watcher.Deleted += OnFolderChanged;
            _watcher.Changed += OnFolderChanged;
            _watcher.Renamed += OnFolderChanged;
        }

        public void Start()
        {
            _watcher.EnableRaisingEvents = true;
        }

        public Task StartAsync()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            _watcher.EnableRaisingEvents = false;
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
    }
}
