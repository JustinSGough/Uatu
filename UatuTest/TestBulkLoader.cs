using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DrwgTronics.Uatu.Models;
using DrwgTronics.Uatu.Components;
using System.IO;

namespace DrwgTronics.UatuTest
{
    /// <summary>
    /// Summary description for TestBulkLoader
    /// </summary>
    [TestClass]
    public class TestBulkLoader
    {
        public TestBulkLoader()
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
        public void BadDirectory()
        {
            var loader = new BulkLoader();
            var model  = new FolderModel();
            bool threwCorrectException = false;

            try
            {
                loader.Load(@"xxx\yyy\zzz\www\ttt", "*.txt", model);
            }
            catch (Exception ex)
            {
                if (ex is DirectoryNotFoundException) threwCorrectException = true;
            }

            Assert.IsTrue(threwCorrectException);
        }

        [TestMethod]
        public void BadFilter()
        {
            var loader = new BulkLoader();
            var model = new FolderModel();
            bool threwCorrectException = false;

            try
            {
                loader.Load(Directory.GetCurrentDirectory(), "\\x\\y", model);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException) threwCorrectException = true;
            }

            Assert.IsTrue(threwCorrectException);
        }
    }
}
