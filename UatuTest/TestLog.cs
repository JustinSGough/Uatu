using DrwgTronics.Uatu.Models;
using DrwgTronics.Uatu.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace DrwgTronics.UatuTest
{
    [TestClass]
    public class TestLog
    {
        [TestMethod]
        public void LogBasic()
        {
            var log = new Log(toConsole: true, fileName: "out.txt");

            var create = new FileEvent(FileEventType.Create, "File1", 1000);
            var delete = new FileEvent(FileEventType.Delete, "File2");
            var update = new FileEvent(FileEventType.Update, "File1", -100);

            log.LogEvent(create);
            log.LogEvent(update);
            log.LogEvent(delete);
            log.LogString("Test String");
            log.LogError("Test Error. Ouch!");
            log.Dispose();

            Assert.IsTrue(File.Exists("out.txt"));

            var resultFile = new StreamReader("out.txt");
            var lines = new List<string>();

            string line;

            do
            {
                line = resultFile.ReadLine();
                if (line != null) lines.Add(line);
            }
            while (line != null);

            Assert.IsTrue(lines.Count == 5);
        }
    }
}
