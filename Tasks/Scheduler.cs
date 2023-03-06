using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tasks
{
    public class Scheduler : IScheduler
    {
        #region Datastructures

        /// <summary>
        /// Define a <see cref="Scheduler"/> slot
        /// </summary>
        private struct Slot
        {
            /// <summary>
            /// The <see cref="IAwaitable"/> task that occupy the <see cref="Slot"/>
            /// </summary>
            public IAwaitable Task;

            /// <summary>
            /// The free state
            /// </summary>
            public bool IsFree => Task == null;

            /// <summary>
            /// The busy state
            /// </summary>
            public bool IsBusy => Task != null;
        }

        #endregion Datastructures

        /// <summary>
        /// The maximum number of tasks the the <see cref="Scheduler"/> can run
        /// </summary>
        public const int MaximumNumberOfTasks = 1024;

        private int maximumNumberOfTasks;
        private Stack<int> freeSlots;
        private Slot[] slots;
        private int numberOfSlotsUsed;

        public ActionQueue<IAwaitable> Tasks { get; protected set; }

        /// <summary>
        /// The current active <see cref="IAwaitable"/> task
        /// </summary>
        public IAwaitable ActiveTask { get; protected set; }

        public Scheduler(int maximumNumberOfTasks = MaximumNumberOfTasks)
        {
            this.maximumNumberOfTasks = maximumNumberOfTasks;
            freeSlots = new Stack<int>();

            for (int i = 0; i < maximumNumberOfTasks; i++)
                freeSlots.Push(i);

            slots = new Slot[maximumNumberOfTasks];
            numberOfSlotsUsed = 0;
        }

        public void Add(IAwaitable task)
        {
            if (freeSlots.Count < maximumNumberOfTasks)
            {
                int i = freeSlots.Pop();
                if (i + 1 > numberOfSlotsUsed)
                    numberOfSlotsUsed = i + 1;

                slots[i].Task = task;
            }
        }

        public Task<List<IAwaitable>> Execute()
        {
            throw new NotImplementedException();
        }

        public Task Execute(string code)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll()
        {
            throw new NotImplementedException();
        }
    }
}
