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
        void Initialize(IFolderModel model);
        bool IsInintialized { get; }

        void CountAll();
        Task CountAllAsync();
        Task CountAllAsync(CancellationToken cancellationToken);
        Task CountAllAsync(IProgress<LineCountProgress> progress);
        Task CountAllAsync(CancellationToken cancellationToken, IProgress<LineCountProgress> progress);
    }
}
