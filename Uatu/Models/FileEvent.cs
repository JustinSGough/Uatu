using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrwgTronics.Uatu.Models
{
    public enum FileEventType
    {
        Create,
        Update,
        Delete
    }

    public class FileEvent
    {
        public readonly FileEventType EventType;
        public readonly string Name;
        public readonly int LineCount;

        public FileEvent(FileEventType eventType, string name, int lineCount = 0)
        {
            EventType = eventType;
            Name = name;
            LineCount = lineCount;
        }
    }
}
