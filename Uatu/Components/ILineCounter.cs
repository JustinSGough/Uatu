using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrwgTronics.Uatu.Models;
using System.Threading;

namespace DrwgTronics.Uatu.Components
{
    public interface ILineCounter
    {
        string FolderPath { get; set; }
        LineCountProgress Count(FileEvent fileEvent);
        Task<LineCountProgress> CountAsync(FileEvent fileEvent);

        List<LineCountProgress> CountBatch(List<FileEvent> fileEvents);
        Task CountBatchAsync(List<FileEvent> fileEvents, IProgress<LineCountProgress> progress);
    }
}
