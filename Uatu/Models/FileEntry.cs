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

        public readonly string Name;
        public DateTime ModifiedDate;
        public int LineCount; // int OK. Max of 1G lines in 2GB file with CR LF endings.

        public FileEntry(string name, DateTime modifiedDate = default(DateTime), int lineCount = NotCounted)
        {
            Name = name;
            ModifiedDate = modifiedDate;
            LineCount = lineCount;
        }
    }
}
