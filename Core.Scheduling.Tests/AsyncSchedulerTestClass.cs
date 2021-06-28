using Core.DataStructures;
using Core.Scheduling.Wrapper;
using Diagnostic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Scheduling.Tests
{
    class AsyncSchedulerTestClass
    {
        private DummyClass dummy;
        AsyncScheduler scheduler;

        [OneTimeSetUp]
        public void Setup()
        {
            Logger.Init(IO.IOUtility.GetDesktopFolder() + "\\logs\\");

            dummy = new DummyClass();
            scheduler = new AsyncScheduler();
        }

        [Test]
        public void ExecuteAsyncScheduling()
        {
            Method waitShort = MethodWrapper.Wrap(dummy).ElementAt(0);
            waitShort.Parameters.ElementAt(0).Value = 1000;

            Method waitLong = MethodWrapper.Wrap(dummy).ElementAt(1);
            waitLong.Parameters.ElementAt(0).Value = 2500;

            scheduler.AddElement(waitShort);
            scheduler.AddElement(waitLong);

            var shortMethod = scheduler.ExecuteAction();
            var longMEthod = scheduler.ExecuteAction();
        }
    }
}
