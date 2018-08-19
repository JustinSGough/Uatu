using DrwgTronics.Uatu.Components;
using DrwgTronics.Uatu.Models;
using DrwgTronics.Uatu.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace DrwgTronics.UatuTest
{
    /// <summary>
    /// Summary description for TestWatcher
    /// </summary>
    [TestClass]
    public class TestWatcher
    {
        public TestWatcher()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Initialize()
        {
            const string Tag = "TestWatcherInitialize_";

            var x = new FileFiller();
            x.FillFile(Tag + "1" + ".txt"    , numberOfLines: 1, lineLength: FileFiller.RandomLength);
            x.FillFile(Tag + "2" + ".txt"    , numberOfLines: 2, lineLength: FileFiller.RandomLength);
            x.FillFile(Tag + "3" + ".txt"    , numberOfLines: 3, lineLength: FileFiller.RandomLength);
            x.FillFile(Tag + "4" + ".txt"    , numberOfLines: 4, lineLength: FileFiller.RandomLength);
            x.FillFile(Tag + "5" + ".txt"    , numberOfLines: 5, lineLength: FileFiller.RandomLength);
            x.FillFile(Tag + "6" + ".txt"    , numberOfLines: 6, lineLength: FileFiller.RandomLength);
            x.FillFile(Tag + "7" + ".txt"    , numberOfLines: 7, lineLength: FileFiller.RandomLength);
            x.FillFile(Tag + "8" + ".txt"    , numberOfLines: 8, lineLength: FileFiller.RandomLength);
            x.FillFile(Tag + "9" + ".txt"    , numberOfLines: 9, lineLength: FileFiller.RandomLength);
            x.FillFile(Tag + "10" + ".txt"   , numberOfLines: 10, lineLength: FileFiller.RandomLength);
            x.FillFile(Tag + "100" + ".txt"  , numberOfLines: 100, lineLength: FileFiller.RandomLength);
            x.FillFile(Tag + "1000" + ".txt" , numberOfLines: 1000, lineLength: FileFiller.RandomLength);
            x.FillFile(Tag + "10000" + ".txt", numberOfLines: 10000, lineLength: FileFiller.RandomLength);

            var model = new FolderModel();
            var view = new Log(toConsole: false, fileName: "out.txt");
            var counter = new LineCounter();
            var loader = new BulkLoader();
            string dir = Directory.GetCurrentDirectory();

            var controller = new Watcher(view, model, counter, loader, dir, Tag + "*");

            System.Threading.Tasks.Task w = controller.StartAsync();
            w.Wait(10000);
            var q = model[Tag + "1" + ".txt"];

            Assert.AreEqual(model.Count(), 13);
            Assert.AreEqual(model[Tag + "1" + ".txt"    ].LineCount,1);
            Assert.AreEqual(model[Tag + "2" + ".txt"    ].LineCount,2);
            Assert.AreEqual(model[Tag + "3" + ".txt"    ].LineCount,3);
            Assert.AreEqual(model[Tag + "4" + ".txt"    ].LineCount,4);
            Assert.AreEqual(model[Tag + "5" + ".txt"    ].LineCount,5);
            Assert.AreEqual(model[Tag + "6" + ".txt"    ].LineCount,6);
            Assert.AreEqual(model[Tag + "7" + ".txt"    ].LineCount,7);
            Assert.AreEqual(model[Tag + "8" + ".txt"    ].LineCount,8);
            Assert.AreEqual(model[Tag + "9" + ".txt"    ].LineCount,9);
            Assert.AreEqual(model[Tag + "10" + ".txt"   ].LineCount,10);
            Assert.AreEqual(model[Tag + "100" + ".txt"  ].LineCount,100);
            Assert.AreEqual(model[Tag + "1000" + ".txt" ].LineCount,1000);
            Assert.AreEqual(model[Tag + "10000" + ".txt"].LineCount,10000);

            Console.WriteLine("DONE");
        }

        int _RandomCounted = 0;
        string _lastStatus = null;

        [TestMethod]
        public void RandomChanges()
        {
            const string Tag = "TestWatcherRandomChanges_";
            string folderName = Directory.GetCurrentDirectory();
            string pattern = Tag + "*";

            foreach (string file in Directory.EnumerateFiles(folderName, pattern)) File.Delete(file);

            var ff = new FileFiller();

            var initialPopulation = new List<FileEntry>();
            for (int i = 1; i <= 99; i++)
            {
                string fName = Tag + i + ".txt";
                ff.FillFile(fName, numberOfLines: i, lineLength: FileFiller.RandomLength);
                initialPopulation.Add(new FileEntry(fName,DateTime.UtcNow, i,0));
            }
            for (int i = 100; i <= 102400; i *= 2)
            {
                string fName = Tag + i + ".txt";
                ff.FillFile(fName, numberOfLines: i, lineLength: FileFiller.RandomLength);
                initialPopulation.Add(new FileEntry(fName, DateTime.UtcNow, i, 0));
            }

            _RandomCounted = 0;

            var model = new FolderModel();
            var view = new Log(toConsole: false, fileName: "out.txt");
            var counter = new LineCounter();
            var loader = new BulkLoader();

            var controller = new Watcher(view, model, counter, loader, folderName, Tag + "*");
            controller.FolderChanged += Controller_FolderChanged_Random;
            controller.Status += Controller_Status;
            System.Threading.Tasks.Task w = controller.StartAsync();

            int loops = 0;
            while (_lastStatus != Watcher.Monitoring)
            {
                loops++;
                Thread.Sleep(1000);
                if (loops > 100) { Assert.Fail("Some Ting Wong"); break; }
            }
            

        }

        private void Controller_Status(object sender, string e)
        {
            _lastStatus = e;
        }

        private void Controller_FolderChanged_Random(object sender, FileEvent e)
        {
            _RandomCounted++;
        }

        

    }
}
