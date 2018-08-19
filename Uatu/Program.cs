using DrwgTronics.Uatu.Components;
using DrwgTronics.Uatu.Models;
using DrwgTronics.Uatu.Views;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DrwgTronics.Uatu
{
    class Program
    {
        static bool DebugMode = System.Diagnostics.Debugger.IsAttached;

        static readonly string Usage = 
            @"Usage: utau.exe [path] [filter]" + Environment.NewLine + 
            @"Example: utau.exe C:\Users\joe *.txt";
        static readonly string InvalidPath = "Path not found {0}";

        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 2)
                {
                    Console.WriteLine(Usage);
                    return;
                }

                string path = args[0];
                string filter = args[1];

                if (!Directory.Exists(path))
                {
                    Console.WriteLine(InvalidPath, path);
                    return;
                }

                Console.WriteLine("UTAU Line Counter");
                Console.WriteLine("=================");
                Console.WriteLine("Folder: " + path);
                Console.WriteLine("Filter: " + filter);

                var model = new FolderModel();
                var view = new Log(toConsole: true, fileName: null);
                var counter = new LineCounter();
                var loader = new BulkLoader();
                var controller = new Watcher(view, model, counter, loader, path, filter);
                controller.FolderChanged += Controller_FolderChanged;
                controller.Status += Controller_Status;
                Console.WriteLine("Starting watcher.");
                Task t = controller.StartAsync();

                Console.WriteLine("Running. Ctrl-C or close the console window to exit.");
                Console.WriteLine();
                t.Wait();
                Console.WriteLine("DONE");
            }
            finally
            {
                WaitForExit();
            }
        }

        private static void Controller_Status(object sender, string e)
        {
            Console.WriteLine(e);
        }

        private static void Controller_FolderChanged(object sender, FileEvent e)
        {
            const string CreateFormat = "{0}Create | {1} Lines | {2}";
            const string DeleteFormat = "{0}Delete | {1}";
            const string UpdateFormat = "{0}Update | {1} Line Change | {2}";
            const string GenerationFormat = "[{0}] ";

            string generationTag = (DebugMode) ? string.Format(GenerationFormat, e.FileEntry.Generation) : "";
            string logEntry = null;

            if (e.EventType == FileEventType.Create)
            {
                logEntry = string.Format(CreateFormat, generationTag, e.FileEntry.LineCount, e.FileEntry.Name);
            }
            else if (e.EventType == FileEventType.Update)
            {
                int deltaCount;
                string deltaText;

                if (e.OldCount == FileEntry.NotCounted)
                {
                    // The original count must have timed out due to locking, etc.
                    // Use actual line count rather than delta.
                    deltaCount = e.FileEntry.LineCount;
                }
                else
                {
                    deltaCount = e.FileEntry.LineCount - e.OldCount;
                }
                deltaText = (deltaCount >= 0) ? "+" + deltaCount : "" + deltaCount;

                logEntry = string.Format(UpdateFormat, generationTag, deltaText, e.FileEntry.Name);
            }
            else if (e.EventType == FileEventType.Delete)
            {
                logEntry = string.Format(DeleteFormat, generationTag, e.FileEntry.Name);
            }

            if (logEntry != null) Console.WriteLine(logEntry);
        }


        static void WaitForExit()
        {
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey(true);
        }
    }
}
