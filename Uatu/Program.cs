using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrwgTronics.Uatu.Models;

namespace DrwgTronics.Uatu
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var x = new Log(toConsole: true, fileName: "out.txt"))
            {
                var create = new FileEvent(FileEventType.Create, "File1", 1000);
                var delete = new FileEvent(FileEventType.Delete, "File2");
                var update = new FileEvent(FileEventType.Update, "File1", -100);
                var updatez = new FileEvent(FileEventType.Update, "File2", 200);

                x.LogEvent(create);
                x.LogEvent(update);
                x.LogEvent(delete);
                x.LogEvent(updatez);
            }

            Console.ReadKey(true);
        }
    }
}
