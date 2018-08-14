using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrwgTronics.Uatu.Models;

namespace DrwgTronics.Uatu.Views
{
    public interface ILog
    {
        void LogString(string text);
        void LogEvent(FileEvent fileEvent);
    }
}
