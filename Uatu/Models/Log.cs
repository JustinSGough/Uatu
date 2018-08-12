using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DrwgTronics.Uatu.Models 
{
    /// <summary>
    /// 
    /// </summary>
    public class Log : ILog, IDisposable
    {
        const int MaxLengthChars = 10; // big enough for int without sign
        static readonly string ConsoleFileErrorFormat = "ERROR: Could not open {0}. Exception:" + Environment.NewLine + "{1}";
        static readonly string UnknownEventTypeErrorFormat = "ERROR: Unknown Event Type {0}";

        bool _disposedValue = false;
        StreamWriter _writer;
        bool _toConsole = true;

        public Log(bool toConsole = true, string fileName = null)
        {
            try
            {
                _toConsole = toConsole;
                if (fileName != null) { _writer = new StreamWriter(fileName); }
            }
            catch (Exception ex)
            {
                _writer = null;
                string m = string.Format(ConsoleFileErrorFormat, fileName, ex.Message);
                LogError(m);
            }
        }

        public void LogEvent(FileEvent fileEvent)
        {
            if (_toConsole)
            {
                string message;

                switch (fileEvent.EventType)
                {
                    case FileEventType.Create:
                        message = string.Format(
                            "Create: {0} {1}",
                            fileEvent.LineCount.ToString().PadRight(MaxLengthChars+1), fileEvent.Name);
                        break;
                    case FileEventType.Delete:
                        message = string.Format("Delete:  {0} {1}", "".PadRight(MaxLengthChars), fileEvent.Name);
                        break;
                    case FileEventType.Update:
                        string sign = (fileEvent.LineCount < 0) ? "-" : "+";
                        message = string.Format(
                            "Update: {0}{1} {2}",
                            sign, Math.Abs(fileEvent.LineCount).ToString().PadRight(MaxLengthChars), fileEvent.Name);
                        break;
                    default:
                        message = string.Format(UnknownEventTypeErrorFormat, fileEvent);
                        break;
                }
                Console.WriteLine(message);
            }

            if (_writer != null)
            {
                _writer.WriteLine("{0}|{1}|{2}", fileEvent.EventType, fileEvent.LineCount, fileEvent.Name);
            }
        }

        public void LogString(string text)
        {
            throw new NotImplementedException();
        }

        public void LogError(string error)
        {
            if (_toConsole)
            {
                ConsoleColor fc = Console.ForegroundColor;
                ConsoleColor bc = Console.BackgroundColor;
                Console.WriteLine(error);
                Console.ForegroundColor = fc;
                Console.BackgroundColor = bc;
            }
            if (_writer != null)
            {

            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (_writer != null) _writer.Dispose();
                }
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
