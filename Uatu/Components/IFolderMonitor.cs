using DrwgTronics.Uatu.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DrwgTronics.Uatu.Components
{
    /// <summary>
    /// Looks for changes in the directory and reports them.
    /// </summary>
    public interface IFolderMonitor
    {
        void Start();
        void Stop();
        Task StartAsync();
        event EventHandler<FileEvent> FolderChanged;
    }
}
