using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Add10Lines
{
    class Program
    {
        static void Main(string[] args)
        {
            string Line = "123456789.123456789.123456789.123456789.123456789.123456789." + Environment.NewLine;
            byte[] lineBytes = Encoding.ASCII.GetBytes(Line);
            FileStream fs = File.Open(args[0], FileMode.Append);
            for (int i = 0; i < 10; ++i) fs.Write(lineBytes, 0, lineBytes.Length);
            fs.Close();
        }
    }
}
