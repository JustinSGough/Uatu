using System;
using System.IO;
using DrwgTronics.Uatu.Models;

namespace DrwgTronics.Uatu.Views
{
    /// <summary>
    /// Emits output. Use this rather than scattered Console writes. 
    /// Provides a place to add extra behavior such as logging to a file for tests
    /// and buffering if output is ever a bottleneck.
    /// </summary>
    public class Log : ILog, IDisposable
    {
        const int MaxLengthChars = 10; // big enough for int without sign
        static readonly string ConsoleFileErrorFormat = "ERROR: Could not open {0}. Exception:" + Environment.NewLine + "{1}";
        static readonly string UnknownEventTypeErrorFormat = "ERROR: Unknown Event Type {0}";
        static readonly string NullString = "Error: Null string passed to Log";

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
            if (text == null)
            {
                LogError(NullString);
                return;
            }

            if (_toConsole) { Console.WriteLine(text); }
            if (_writer != null) { _writer.WriteLine("S|0|" + text); }
        }

        public void LogError(string error)
        {
            if (_toConsole)
            {
                ConsoleColor fc = Console.ForegroundColor;
                ConsoleColor bc = Console.BackgroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(error);
                Console.ForegroundColor = fc;
                Console.BackgroundColor = bc;
            }
            if (_writer != null)
            {
                _writer.WriteLine("E|0|" + error);
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
