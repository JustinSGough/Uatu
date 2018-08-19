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
        public string FolderPath { get; set; }

        public LineCountProgress Count(FileEvent fileEvent)
        {
            const double TimeoutSeconds = 5.0;
            const int RetryInterval = 900; // milliseconds
            var report = new LineCountProgress(fileEvent);

            DateTime startTime = DateTime.Now;
            TimeSpan elapsed = new TimeSpan(ticks: 0);
            bool keepTrying = true;

            while (keepTrying)
            {
                try
                {
                    int count = FileEntry.NotCounted;
                    string path = Path.Combine(FolderPath ?? "", fileEvent.FileEntry.Name);

                    using (var f = new StreamReader(path, detectEncodingFromByteOrderMarks: true))
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

        public Task<LineCountProgress> CountAsync(FileEvent fileEvent)
        {
            return Task.Run(() => Count(fileEvent));
        }

        public List<LineCountProgress> CountBatch(List<FileEvent> fileEvents)
        {
            var results = new List<LineCountProgress>(fileEvents.Count);
            fileEvents.ForEach((fileEvent) => CountAndSave(fileEvent, results));
            return results;
        }

        public Task CountBatchAsync(List<FileEvent> fileEvents, IProgress<LineCountProgress> progress)
        {
            return Task.Run(() => CountAndReportBatch(fileEvents, progress));
        }

        void CountAndReportBatch(List<FileEvent> fileEvents, IProgress<LineCountProgress> progress)
        {
            Parallel.ForEach(fileEvents, (fileEvent) => CountAndReport(fileEvent, progress));
        }

        void CountAndReport(FileEvent fileEvent, IProgress<LineCountProgress> progress)
        {
            LineCountProgress report = Count(fileEvent);
            if (progress != null) progress.Report(report);
        }

        void CountAndSave(FileEvent fileEvent, List<LineCountProgress> results)
        {
            LineCountProgress report = Count(fileEvent);
            results.Add(report);
        }

    }
}
