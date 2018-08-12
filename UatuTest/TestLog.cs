using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DrwgTronics.Uatu.Models;
using System.IO;
using System.Collections.Generic;

namespace DrwgTronics.UatuTest
{
    [TestClass]
    public class TestLog
    {
        [TestMethod]
        public void Basic()
        {
            var log = new Log(toConsole: true, fileName: "out.txt");

            var create = new FileEvent(FileEventType.Create, "File1", 1000);
            var delete = new FileEvent(FileEventType.Delete, "File2");
            var update = new FileEvent(FileEventType.Update, "File1", -100);

            log.LogEvent(create);
            log.LogEvent(update);
            log.LogEvent(delete);

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

            Assert.IsTrue(lines.Count == 3);
        }
    }
}
