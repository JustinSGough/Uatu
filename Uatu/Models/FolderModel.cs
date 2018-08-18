using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DrwgTronics.Uatu.Models
{
    /// <summary>
    /// Central repository of folder information. Modified dates, names, etc. ConcurrentDictionary would work but this
    /// way buffering or other behavior could be added later if needed. We can also relax locks for bulk loading.
    /// </summary>
    public class FolderModel : IFolderModel
    {
        protected Dictionary<string, FileEntry> Files = 
            new Dictionary<string, FileEntry>(1000, StringComparer.OrdinalIgnoreCase);

        object _lockObject = new object();

        public bool EnforceLocks
        {
            get { return _enforceLocks;  } // no need for locks, set of bool is thread safe.
            set { _enforceLocks = value; }
        }
        bool _enforceLocks = true;

        public FileEntry this[string key]
        {
            get
            {
                if (_enforceLocks)
                {
                    try
                    {
                        Monitor.Enter(_lockObject); return Files[key];
                    }
                    finally
                    {
                        Monitor.Exit(_lockObject);
                    }
                }
                else
                {
                    return Files[key];
                }
            }
            set
            {
                if (_enforceLocks)
                {
                    try
                    {
                        Monitor.Enter(_lockObject);
                        Files[key] = value;
                    }
                    finally
                    {
                        Monitor.Exit(_lockObject);
                    }
                }
                else
                { 
                    Files[key] = value;  
                }
            }
        }

        public bool TryGetValue(string key, out FileEntry fileEntry)
        {
            if (_enforceLocks)
            {
                try
                {
                    Monitor.Enter(_lockObject);
                    return Files.TryGetValue(key, out fileEntry);
                }
                finally
                {
                    Monitor.Exit(_lockObject);
                }
            }
            else
            { 
                return Files.TryGetValue(key, out fileEntry);
            }
        }

        /// <summary>
        /// Used for initial load only. Bypasses checks.
        /// </summary>
        /// <param name="fileEntries"></param>
        public void BulkAddFiles(IEnumerable<FileEntry> fileEntries)
        {
            if (_enforceLocks)
            {
                try
                {
                    Monitor.Enter(_lockObject);
                    foreach (FileEntry e in fileEntries) Files.Add(e.Name, e);
                }
                finally
                {
                    Monitor.Exit(_lockObject);
                }
            }
            else
            {
                foreach (FileEntry e in fileEntries) Files.Add(e.Name, e);
            }
        }

        public void AddFile(FileEntry fileEntry)
        {
            if (_enforceLocks)
            {
                try
                {
                    Monitor.Enter(_lockObject);
                    Files.Add(fileEntry.Name, fileEntry);
                }
                finally
                {
                    Monitor.Exit(_lockObject);
                }
            }
            else
            {
                Files.Add(fileEntry.Name, fileEntry);
            }


        }

        public void DeleteFile(FileEntry fileEntry)
        {
            if (_enforceLocks)
            {
                try
                {
                    Monitor.Enter(_lockObject);
                    Files.Remove(fileEntry.Name);
                }
                finally
                {
                    Monitor.Exit(_lockObject);
                }
            }
            else
            {
                Files.Remove(fileEntry.Name);
            }
        }

        public void DeleteFile(string name)
        {
            if (_enforceLocks)
            {
                try
                {
                    Monitor.Enter(_lockObject);
                    Files.Remove(name);
                }
                finally
                {
                    Monitor.Exit(_lockObject);
                }
            }
            else
            {
                Files.Remove(name);
            }
        }

        public void UpdateFile(FileEntry fileEntry)
        {
            if (_enforceLocks)
            {
                try
                {
                    Monitor.Enter(_lockObject);
                    FileEntry entry;
                    if (Files.TryGetValue(fileEntry.Name, out entry))
                    {
                        fileEntry.CopyTo(entry);
                    }
                }
                finally
                {
                    Monitor.Exit(_lockObject);
                }
            }
            else
            {
                FileEntry entry;
                if (Files.TryGetValue(fileEntry.Name, out entry))
                {
                    fileEntry.CopyTo(entry);
                }
            }
        }

        public void Clear()
        {
            if (_enforceLocks)
            {
                try
                {
                    Monitor.Enter(_lockObject);
                    Files.Clear();
                }
                finally
                {
                    Monitor.Exit(_lockObject);
                }
            }
            else
            {
                Files.Clear();
            }
        }

        public List<FileEvent> AsFileEvents(FileEventType fileEventType)
        {
            if (_enforceLocks)
            {
                try
                {
                    Monitor.Enter(_lockObject);
                    return AsFileEventsInternal(fileEventType);
                }
                finally
                {
                    Monitor.Exit(_lockObject);
                }
            }
            else
            {
                return AsFileEventsInternal(fileEventType);
            }
        }

        List<FileEvent> AsFileEventsInternal(FileEventType fileEventType)
        {
            var list = new List<FileEvent>(Files.Count);

            foreach (FileEntry f in Files.Values)
            {
                var fileEvent = new FileEvent(fileEventType, f);
                list.Add(fileEvent);
            }
            return list;
        }

        public int Count()
        {
            if (_enforceLocks)
            {
                try
                {
                    Monitor.Enter(_lockObject);
                    return Files.Count;
                }
                finally
                {
                    Monitor.Exit(_lockObject);
                }
            }
            else
            {
                return Files.Count;
            }
        }

        public IEnumerable<FileEntry> FromGeneration(int generation)
        {
            if (_enforceLocks)
            {
                try
                {
                    Monitor.Enter(_lockObject);
                    return Files.Values.Where(f => f.Generation == generation);
                }
                finally
                {
                    Monitor.Exit(_lockObject);
                }
            }
            else
            {
                return Files.Values.Where(f => f.Generation == generation);
            }        
        } 
    }
}
