using Core.DataStructures;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Tasks;

namespace UserInterface.Controls.Tests
{
    public partial class TasksForm : Form
    {
        public TasksForm()
        {
            InitializeComponent();

            ServiceBroker.Initialize();
            ServiceBroker.Provide(new TasksService());
            ServiceBroker.Provide(new SchedulersService());

            Scheduler defaultScheduler = new Scheduler("DefaultScheduler", maxDegreesOfParallelism: 4);
            ServiceBroker.GetService<SchedulersService>().Add(defaultScheduler);

            Scheduler otherScheduler = new Scheduler(maxDegreesOfParallelism: 11);
            ServiceBroker.GetService<SchedulersService>().Add(otherScheduler);

            FunctionTask task;
            for (int i = 0; i < 10; i++)
            {
                task = new FunctionTask(Name + i);
                ServiceBroker.GetService<TasksService>().Add(task);
            }

            CyclicFunctionTask cyclicTask = new CyclicFunctionTask(Name + ".Cyclic", TimeSpan.FromMilliseconds(1000d));
            ServiceBroker.GetService<TasksService>().Add(cyclicTask);

            for (int i = 0; i < 10; i++)
            {
                cyclicTask = new CyclicFunctionTask($"{Name}.Cyclic.{i}", TimeSpan.FromMilliseconds(1000d), otherScheduler);
                ServiceBroker.GetService<TasksService>().Add(cyclicTask);
            }

            foreach (IAwaitable t in ServiceBroker.GetService<TasksService>().GetAll())
            {
                TaskControl taskControl = new TaskControl(t);
                taskFlowLayout.Controls.Add(taskControl);
            }

            foreach (IScheduler scheduler in ServiceBroker.GetService<SchedulersService>().GetAll())
            {
                SchedulerControl schedulerControl = new SchedulerControl(scheduler);
                schedulerFlowLayout.Controls.Add(schedulerControl);
            }

            CenterToScreen();
        }

        public class FunctionTask : Awaitable
        {
            public FunctionTask(string code, Scheduler scheduler = null) : base(code, scheduler)
            { }

            public override IEnumerable<string> Execution()
            {
                for (int i = 0; i < 10; i++)
                {
                    yield return WaitFor(TimeSpan.FromSeconds(1d))
                        .WithDescription($"Step {i + 1}, waiting 1 second");
                }

                yield return "Done";
            }
        }

        public class CyclicFunctionTask : CyclicAwaitable
        {
            private int counter;

            public CyclicFunctionTask(string code, TimeSpan cycleTime, Scheduler scheduler = null) : base(code, cycleTime, scheduler)
            {
                counter = 0;
            }

            public override IEnumerable<string> Execution()
            {
                yield return "Incrementing counter";
                counter++;

                yield return "Cycle done";
            }

            public override IEnumerable<string> Termination()
            {
                return base.Termination();
            }
        }

        private void TasksForm_Load(object sender, EventArgs e)
        {
            foreach (IAwaitable task in ServiceBroker.GetService<TasksService>().GetAll())
                task.Start();
        }
    }
}