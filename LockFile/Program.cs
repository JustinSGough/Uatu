using System;
using System.IO;
using System.Threading;

namespace LockFile
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Locking " + args[0]);
            Console.WriteLine("Press ctrl-C or close the console window to exit");
            FileStream s = File.Open(args[0], FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);

            if (args.Length > 1)
            {
                byte b = 2;
                s.WriteByte(b);
                s.Flush();
            }

            while (true) Thread.Sleep(1000000);
        }
    }
}
