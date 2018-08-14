using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DrwgTronics.Uatu.Models;
using System.IO;

namespace DrwgTronics.Uatu.Components
{
    public class LineCounter : ILineCounter
    {
        public void Count(FileEvent fileEvent, IProgress<LineCountProgress> progress)
        {
            throw new NotImplementedException();
        }

        public Task CountAsync(FileEvent fileEvent, IProgress<LineCountProgress> progress)
        {
            throw new NotImplementedException();
        }

        public void CountBatch(List<FileEvent> fileEvents, IProgress<LineCountProgress> progress)
        {
            throw new NotImplementedException();
        }

        public Task CountBatchAsync(List<FileEvent> fileEvents, IProgress<LineCountProgress> progress)
        {
            throw new NotImplementedException();
        }

        public LineCountProgress Count(FileEvent fileEvent)
        {
            const double TimeoutSeconds = 20.0;
            const int RetryInterval = 5000; // 5 seconds
            var report = new LineCountProgress(fileEvent);

            DateTime startTime = DateTime.Now;
            TimeSpan elapsed = new TimeSpan(ticks: 0);
            bool keepTrying = true;

            while (keepTrying)
            {
                try
                {
                    int count = -1;

                    using (var f = new StreamReader(fileEvent.Name, detectEncodingFromByteOrderMarks: true))
                    {
                        string line;
                        do
                        {
                            count++;
                            line = f.ReadLine();
                        }
                        while (line != null);
                    }
                    report.Count = count;
                    report.Status = LineCountStatus.Success;
                    keepTrying = false;
                }
                catch (IOException ex)
                {
                    if (ex is DirectoryNotFoundException ||
                        ex is DriveNotFoundException ||
                        ex is FileNotFoundException ||
                        ex is PathTooLongException)
                    {
                        report.Status = LineCountStatus.FileNotFound;
                        report.Note = ex.Message;
                        keepTrying = false;
                    }
                    else
                    {
                        // locked file raises IOException base class
                        elapsed = DateTime.Now - startTime;
                        if (elapsed.TotalSeconds >= TimeoutSeconds)
                        {
                            report.Status = LineCountStatus.TimedOut;
                            report.Note = "Timed out after " + elapsed;
                            keepTrying = false;
                        }
                        else
                        {
                            Thread.Sleep(RetryInterval);
                        }
                    }
                }
                catch (Exception ex)
                {
                    report.Status = LineCountStatus.Exception;
                    report.Note = ex.Message;
                    keepTrying = false;
                }
            }

            return report;
        }
    }
}
