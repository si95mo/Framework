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

            Scheduler scheduler = new Scheduler(maxDegreesOfParallelism: 4);
            ServiceBroker.GetService<SchedulersService>().Add(scheduler);

            FunctionTask task;
            for (int i = 0; i < 10; i++)
            {
                task = new FunctionTask(Name + i);
                ServiceBroker.GetService<TasksService>().Add(task);
            }

            foreach(IAwaitable t in ServiceBroker.GetService<TasksService>().GetAll())
            {
                TaskControl taskControl = new TaskControl(t);
                flowLayout.Controls.Add(taskControl);
            }

            SchedulerControl schedulerControl = new SchedulerControl(ServiceBroker.GetService<SchedulersService>().GetDefaultScheduler());
            flowLayout.Controls.Add(schedulerControl);

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

    }
}
