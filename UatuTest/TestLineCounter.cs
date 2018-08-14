using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DrwgTronics.Uatu.Components;
using DrwgTronics.Uatu.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DrwgTronics.UatuTest
{
    [TestClass]
    public class TestLineCounter
    {
        [TestMethod]
        public void CountMissingFile()
        {
            var fc = new LineCounter();
            var model = new FileEvent(FileEventType.Create, "r:\\no way that this file is really there.txt", 0);
            LineCountProgress result = fc.Count(model);
            Assert.AreEqual(result.Status, LineCountStatus.FileNotFound);
        }

        [TestMethod]
        public void CountLockedFile()
        {
            const string Test_Locked = "Test_Locked.txt";

            if (!File.Exists(Test_Locked))
            {
                var ff = new FileFiller();
                ff.FillFile(Test_Locked, 10, -2);
            }

            var x = Task.Run(() => LockFile(Test_Locked, seconds:30));
            Thread.Sleep(1000); // give it time to open and lock the file
            var fc = new LineCounter();
            var model = new FileEvent(FileEventType.Create, Test_Locked, 0);
            LineCountProgress result = fc.Count(model);
            
            Assert.AreEqual(result.Status, LineCountStatus.TimedOut);

            x.Wait();
        }

        [TestMethod]
        public void CountTemporarilyLockedFile()
        {
            const string Test_Locked = "Test_Locked.txt";

            if (!File.Exists(Test_Locked))
            {
                var ff = new FileFiller();
                ff.FillFile(Test_Locked, 10, -2);
            }

            var x = Task.Run(() => LockFile(Test_Locked, seconds:15));
            Thread.Sleep(1000); // give it time to open and lock the file
            var fc = new LineCounter();
            var model = new FileEvent(FileEventType.Create, Test_Locked, 0);
            LineCountProgress result = fc.Count(model);

            Assert.AreEqual(result.Status, LineCountStatus.Success);
            Assert.AreEqual(result.Count, 10);

            x.Wait();
        }

        public void LockFile(string fileName, int seconds)
        {
            var fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite);
            Console.WriteLine("Locking " + fileName);
            for (int i = 0; i < seconds; ++i)
            {
                Console.WriteLine("Locked for " + i + " seconds.");
                Thread.Sleep(1000);
            }
            Console.WriteLine("Unlocking " + fileName);
            fs.Close();
            fs.Dispose();
        }

        [TestMethod]
        public void CountZeroLengthFile()
        {
            const string Test_0 = "Test_0.txt";

            if (!File.Exists(Test_0))
            {
                var ff = new FileFiller();
                ff.FillFile(Test_0, 0, -2);
            }
            var fc = new LineCounter();
            var model = new FileEvent(FileEventType.Create, Test_0, 0);
            LineCountProgress result = fc.Count(model);

            Assert.AreEqual(result.Status, LineCountStatus.Success);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void CountFile10000()
        {
            const string Test_10000 = "Test_10000.txt";

            if (!File.Exists(Test_10000))
            {
                var ff = new FileFiller();
                ff.FillFile(Test_10000, 10000, 0);
            }
            var fc = new LineCounter();
            var model = new FileEvent(FileEventType.Create, Test_10000, 0);
            LineCountProgress result = fc.Count(model);

            Assert.AreEqual(result.Status, LineCountStatus.Success);
            Assert.AreEqual(10000, result.Count);
        }
    }
}
