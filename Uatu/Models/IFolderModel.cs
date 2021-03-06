﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrwgTronics.Uatu.Models
{
    public interface IFolderModel 
    {
        FileEntry this[string key] { get; set; }
        void AddFile(FileEntry fileEntry);
        void UpdateFile(FileEntry fileEntry);
        void DeleteFile(FileEntry fileEntry);
        void BulkAddFiles(IEnumerable<FileEntry> fileEntries);
        List<FileEvent> AsFileEvents(FileEventType fileEventType);
        bool EnforceLocks { get; set; }
        bool TryGetValue(string key, out FileEntry fileEntry);
        void Clear();
        int Count();
        IEnumerable<FileEntry> FromGenerationPriorTo(int generation);
    }
}
