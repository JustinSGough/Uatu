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
        public FileEntry FileEntry { get; set; }

        public FileEvent(FileEventType eventType, FileEntry entry)
        {
            EventType = eventType;
            FileEntry = entry;
        }
    }
}
