using Core.DataStructures;
using Core.Scheduling.Wrapper;
using Diagnostic;
using NUnit.Framework;
using System.Linq;

namespace Core.Scheduling.Tests
{
    internal class AsyncSchedulerTestClass
    {
        private DummyClass dummy;
        private AsyncScheduler scheduler;

        [OneTimeSetUp]
        public void Setup()
        {
            Logger.Initialize();

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

            var shortMethod = scheduler.Execute();
            var longMEthod = scheduler.Execute();
        }
    }
}