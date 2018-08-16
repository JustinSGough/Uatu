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

                var model = new FolderModel();
                var view = new Log(toConsole: true, fileName: "log.txt");
                var counter = new LineCounter();
                var loader = new BulkLoader();
                var controller = new Watcher(view, model, counter, loader, path, filter);

                Task t = controller.StartAsync();

                Console.Write("Waitng...");
                t.Wait();
                Console.WriteLine("DONE");
            }
            finally
            {
                WaitForExit();
            }
        }

        static void WaitForExit()
        {
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey(true);
        }
    }
}
