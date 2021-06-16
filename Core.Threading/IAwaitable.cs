using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Threading
{
    public enum TaskResult
    {
        None = 0,
        Success = 1,
        Aborted = 2,
        Failure = 3
    }

    public interface IAwaitable
    {
        string Code { get; }
        TaskStatus Status { get; }
        TaskResult Result { get; }
        bool Running { get; }
        bool Succeded { get; }
        bool NotSucceded { get; }
        string ReasonOfFailure { get; }

        TaskAwaiter<TaskResult> GetAwaiter();
    }
}
