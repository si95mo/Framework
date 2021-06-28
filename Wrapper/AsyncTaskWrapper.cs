using Core.DataStructures;
using System;
using System.Threading.Tasks;

namespace Core.Scheduling.Wrapper
{
    public class AsyncTaskWrapper
    {
        /// <summary>
        /// Wrap a <see cref="Method"/> to an awaitable <see cref="Task"/>
        /// and start it asynchronously
        /// </summary>
        /// <param name="method">The <see cref="Method"/> to wrap</param>
        /// <returns>The wrapped <see cref="Task"/></returns>
        public static Task<string> WrapAndStart(Method method)
        {
            return Task.Factory.StartNew(() =>
                {
                    Action action = method.UnwrapAction();
                    action?.Invoke();

                    return $"Executing {method.ExtendedToString()}";
                }
            );
        }
    }
}
