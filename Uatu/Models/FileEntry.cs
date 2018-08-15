using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrwgTronics.Uatu.Models
{
    public class FileEntry
    {
        public readonly static DateTime NotModified = default(DateTime);
        public const int NotCounted = -1;

        public string Name { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int LineCount { get; set; } // int OK. Max of 1G lines in 2GB file with CR LF endings.
        public int Generation { get; set; }

        public FileEntry(
            string name, 
            DateTime modifiedDate = default(DateTime), 
            int lineCount = NotCounted,
            int generation = 0)
        {
            Name = name;
            ModifiedDate = modifiedDate;
            LineCount = lineCount;
        }

        public FileEntry CopyTo(FileEntry to)
        {
            to.Name = Name;
            to.ModifiedDate = ModifiedDate;
            to.LineCount = LineCount;
            to.Generation = Generation;
            return to;
        }
    }
}
