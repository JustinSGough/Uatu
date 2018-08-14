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
        void CountBatch(List<FileEvent> fileEvents, IProgress<LineCountProgress> progress);
        Task CountBatchAsync(List<FileEvent> fileEvents, IProgress<LineCountProgress> progress);

        void Count(FileEvent fileEvent, IProgress<LineCountProgress> progress);
        Task CountAsync(FileEvent fileEvent, IProgress<LineCountProgress> progress);
    }
}
