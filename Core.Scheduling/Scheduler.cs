using Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks;

namespace Core.Scheduling
{
    public class Scheduler : IScheduler
    {
        public ActionQueue<IAwaitable> Tasks { get; }

        public void Add(IAwaitable task)
        {
            Tasks.Enqueue(task);
        }

        public async Task<List<IAwaitable>> Execute()
        {
            IAwaitable task = Tasks.Dequeue();
            task.Execute().Select((x) => x.);
        }

        public void RemoveAll()
        {
            throw new NotImplementedException();
        }
    }
}
