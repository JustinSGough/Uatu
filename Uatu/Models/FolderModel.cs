using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrwgTronics.Uatu.Models
{
    /// <summary>
    /// Central repository of folder information. Modified dates, names, etc. ConcurrentDictionary would work but this
    /// way buffering or other behavior could be added later if needed. We can also relax locks for bulk loading.
    /// </summary>
    public class FolderModel : IFolderModel
    {
        protected Dictionary<string, FileEntry> Files = new Dictionary<string, FileEntry>(1000);
        object _lockObject = new Object();  // use c# lock, upgrade to Monitor if timeout is needed.

        public FileEntry this[string key]
        {
            get { return Files[key]; }
            set { Files[key] = value; }
        }

        /// <summary>
        /// Used for initial load only. Bypasses checks.
        /// </summary>
        /// <param name="fileEntries"></param>
        public void BulkAddFiles(IEnumerable<FileEntry> fileEntries)
        {
            foreach(FileEntry e in fileEntries)
            {
                Files.Add(e.Name, e); 
            }
        }

        public void AddFile(FileEntry fileEntry)
        {
            lock(_lockObject)
            {
                Files.Add(fileEntry.Name, fileEntry);
            }
        }

        public void DeleteFile(FileEntry fileEntry)
        {
            Files.Remove(fileEntry.Name);
        }

        public void DeleteFile(string name)
        {
            lock (_lockObject)
            {
                Files.Remove(name);
            }
        }

        public void UpdateFile(FileEntry fileEntry)
        {
            lock(_lockObject)
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
            lock(_lockObject)
            {
                Files.Clear();
            }
        }

        public List<FileEvent> AsFileEvents(FileEventType fileEventType)
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
            return Files.Count;
        }
    }
}
