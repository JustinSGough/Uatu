using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrwgTronics.Uatu.Models
{
    public interface ILog
    {
        void LogString(string text);
        void LogEvent(FileEvent fileEvent);
    }
}
