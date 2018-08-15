using DrwgTronics.Uatu.Models;
using DrwgTronics.Uatu.Views;
using DrwgTronics.Uatu.Components;

using System;
using System.IO;

namespace DrwgTronics.Uatu
{
    class Program
    {
        static void Main(string[] args)
        {
            var model = new FolderModel();
            var view = new Log(toConsole: false, fileName: "out.txt");
            var counter = new LineCounter();
            var loader = new BulkLoader();
            var controller = new Watcher(view, model, counter, loader, @"C:\Users\mgough\Desktop\PDQ\Uatu\UatuTest\bin\Debug", "*.txt");

            controller.Start();

       
            Console.WriteLine("DONE");
            Console.ReadKey(true);

        }
    }
}
