using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrwgTronics.Uatu.Models
{
    public interface IFolderModel
    {
        void AddFile(string name, DateTime modifiedDate, int lineCount);
        void AddFile(FileEntry fileEntry);
        void UpdateFile(string name, DateTime modifiedDate, int lineCount);
        void UpdateFile(FileEntry fileEntry);
        void DeleteFile(string name);
        void DeleteFile(FileEntry fileEntry);

        // Events for add, delete, update?
    }
}
