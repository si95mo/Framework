using Core.DataStructures;
using Core.Scheduling.Wrapper;
using Diagnostic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Scheduling
{
    /// <summary>
    /// Implement an async <see cref="Method"/> scheduler
    /// </summary>
    [Serializable]
    public class AsyncScheduler : MethodScheduler
    {
        /// <summary>
        /// Creates a new instance of the <see cref="AsyncScheduler"/>
        /// </summary>
        public AsyncScheduler() : base()
        { }

        /// <summary>
        /// Executes the <see cref="Action"/> associated with the <see cref="Method"/>
        /// stored in the <see cref="SubscribedMethods"/>,
        /// and remove it from the <see cref="ActionQueue{T}"/>
        /// </summary>
        /// <returns>The <see cref="Method"/> executed</returns>
        public override Method Execute()
        {
            Method method = subscribedMethods.Dequeue();

            Logger.Log(
                $"{method.ExtendedToString()} :: " +
                $"Execution async started at : {DateTime.Now:HH:mm:ss:ffff}",
                Severity.Debug
            );

            var results = ExecuteAsync(method);
            foreach (var result in results)
                Logger.Log(result.ToString(), Severity.Trace);

            Logger.Log(
                $"{method.ExtendedToString()} :: " +
                $"Execution async ended at : {DateTime.Now:HH:mm:ss:ffff}",
                Severity.Debug
            );

            return method;
        }

        private IEnumerable<string> ExecuteAsync(Method method)
        {
            Task task = AsyncTaskWrapper.WrapAndStart(method);

            task.Wait();
            yield return "Waiting...";

            yield return "Done";
        }
    }
}