using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrwgTronics.Uatu.Models
{
    public class FileEvent 
    {
        public FileEventType EventType { get; set; }
        public string NewName { get; set; }
        public int OldCount { get; set; }
        public FileEntry FileEntry { get; set; }

        public FileEvent(FileEventType eventType, FileEntry entry, string newName = null, int oldCount = FileEntry.NotCounted)
        {
            EventType = eventType;
            FileEntry = entry;
            NewName = newName;
            OldCount = oldCount;
        }
    }
}
