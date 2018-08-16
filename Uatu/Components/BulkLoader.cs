using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrwgTronics.Uatu.Models;
using System.IO;

namespace DrwgTronics.Uatu.Components
{
    public class BulkLoader : IBulkLoader
    {
        const int BufferSize = 500;

        public int Load(string directory, string filter, IFolderModel model)
        {
            int count = 0;

            if (!Directory.Exists(directory)) throw new DirectoryNotFoundException(directory);

            var info = new DirectoryInfo(directory);

            var buffer = new List<FileEntry>(BufferSize);

            foreach (FileInfo fi in info.EnumerateFiles(filter)) // throws ArgumentException if filter is bad
            {
                var entry = new FileEntry(fi.Name, fi.LastWriteTimeUtc);
                buffer.Add(entry);
                count++;

                if (count % BufferSize == 0)
                {
                    model.BulkAddFiles(buffer);
                    buffer.Clear();
                }
            }
            if (buffer.Count > 0) model.BulkAddFiles(buffer);

            return count;
        }
    }
}
