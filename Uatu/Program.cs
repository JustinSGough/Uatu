using DrwgTronics.Uatu.Models;
using DrwgTronics.Uatu.Views;
using System;
using System.IO;

namespace DrwgTronics.Uatu
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
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
                x.LogString("test string");
                x.LogError("zowie");
                x.LogString("done");
            }

            string[] dir = Directory.GetFiles(@"C:\Users\mgough\Desktop\PDQ\Uatu\UatuTest\Data\Encodings");

            foreach (string fileName in dir)
            {
                using (var x = new StreamReader(fileName, detectEncodingFromByteOrderMarks: true))
                {
                   
                    Console.WriteLine("{0} {1}", fileName, x.CurrentEncoding);
                    for (int i = 1; i <= 10; i++)
                    {
                        string line = x.ReadLine();
                        Console.WriteLine("    " + line);
                    }
                }
            }
            */
            /*
            using (StreamWriter w = new StreamWriter(@"C:\Users\mgough\Desktop\PDQ\Uatu\UatuTest\Data\Large\x.txt"))
            {
                int max = Int32.MaxValue / Environment.NewLine.Length;

                for (Int32 i = 1; i <= max; i++)
                {
                    w.WriteLine();
                    if (i % 1000000 == 0) Console.WriteLine(i);
                }
            }
            */
            
            Console.ReadKey(true);
        }
    }
}
