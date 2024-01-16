using Core.Conditions;
using Core.DataStructures;
using Diagnostic;
using Diagnostic.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Tasks;
using UserInterface.Forms;

namespace UserInterface.Controls.Tests
{
    public partial class TasksForm : CustomForm
    {
        private Alarm alarm;
        private readonly UiService uiService;

        public TasksForm()
        {
            InitializeComponent();

            Logger.Initialize();

            ServiceBroker.Initialize();
            ServiceBroker.Provide(new TasksService());
            ServiceBroker.Provide(new SchedulersService(maxDegreesOfParallelism: 4));
            ServiceBroker.Provide(new DiagnosticMessagesService());

            uiService = new UiService(this);
            ServiceBroker.Provide(uiService);

            Scheduler cyclicScheduler = new Scheduler("CyclicScheduler", maxDegreesOfParallelism: 25);
            alarm = new Alarm("Alarm", "Alarm fired", string.Empty, null);

            AlarmMonitor.Initialize();

            lblSchedulerLoad.SetProperty(cyclicScheduler.Load);
            panelWithLed.InitializeLed(
                (cyclicScheduler.Load.IsInRange(0d, 25d), Colors.LightBlue),
                (cyclicScheduler.Load.IsInRange(25d, 50d, isMinimumExcluded: true), Colors.LightYellow),
                (cyclicScheduler.Load.IsInRange(50d, 75d, isMinimumExcluded: true), Colors.Yellow),
                (cyclicScheduler.Load.IsInRange(75d, 100d, isMinimumExcluded: true), Colors.Red)
            );

            for (int i = 0; i < 10; i++)
            {
                new FunctionTask(Name + i);
            }

            for (int i = 0; i < 2; i++)
            {
                new CyclicFunctionTaskThatWontStop($"{Name}.CyclicThatWontStop.{i}", TimeSpan.FromMilliseconds(1000d), cyclicScheduler);
            }

            new CyclicFunctionTask(Name + ".Cyclic", TimeSpan.FromMilliseconds(1000d));
            for (int i = 0; i < 10; i++)
            {
                new CyclicFunctionTask($"{Name}.Cyclic.{i}", TimeSpan.FromMilliseconds(1000d), cyclicScheduler);
            }

            foreach (IAwaitable t in ServiceBroker.GetService<TasksService>().GetAll().Cast<IAwaitable>())
            {
                TaskControl taskControl = new TaskControl(t);
                taskFlowLayout.Controls.Add(taskControl);
            }

            foreach (IScheduler scheduler in ServiceBroker.GetService<SchedulersService>().GetAll().Cast<IScheduler>())
            {
                SchedulerControl schedulerControl = new SchedulerControl(scheduler);
                schedulerFlowLayout.Controls.Add(schedulerControl);
            }

            CenterToScreen();
        }

        private void TasksForm_Load(object sender, EventArgs e)
        {
            foreach (IAwaitable task in ServiceBroker.GetService<TasksService>().GetAll().Cast<IAwaitable>())
            {
                task.Start();
            }

            uiService.ShowToaster($"{DateTime.Now:HH:mm:ss.fff} >> Toaster message test 1", ToasterType.Message, TimeSpan.FromSeconds(2d));
            uiService.ShowToaster($"{DateTime.Now:HH:mm:ss.fff} >> Toaster warning test 2", ToasterType.Warning, TimeSpan.FromSeconds(4d));
            uiService.ShowToaster($"{DateTime.Now:HH:mm:ss.fff} >> Toaster error test 3", ToasterType.Error, TimeSpan.FromSeconds(6d));
            uiService.ShowToaster(
                $"{DateTime.Now:HH:mm:ss.fff} >> Toaster long text {new string(Enumerable.Repeat('x', 128).ToArray())} test",
                ToasterType.Message,
                TimeSpan.FromSeconds(8d)
            );

            uiService.ShowToaster("Message 1", ToasterType.Message, TimeSpan.FromSeconds(5d));
            uiService.ShowToaster("Message 2", ToasterType.Error, TimeSpan.FromSeconds(30d));
        }

        private void BtnFireAlarm_Click(object sender, EventArgs e)
        {
            if (!alarm.Active)
            {
                alarm.Fire($"Alarm fired @{DateTime.Now}");
            }
        }

        private void BtnReserAlarm_Click(object sender, EventArgs e)
        {
            if (alarm.Active)
            {
                alarm.Reset();

                foreach (IAwaitable task in ServiceBroker.GetService<TasksService>().GetAll().Cast<IAwaitable>())
                {
                    task.Start();
                }
            }
        }
    }

    #region Custom tasks classes

    public class FunctionTask : Awaitable
    {
        public FunctionTask(string code, Scheduler scheduler = null) : base(code, scheduler)
        { }

        public override IEnumerable<string> Execution()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return WaitFor(TimeSpan.FromSeconds(1d))
                    .WithMessage($"Step {i + 1}, waiting 1 second");
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

    [DontStopInAlarm]
    public class CyclicFunctionTaskThatWontStop : CyclicAwaitable
    {
        private int counter;

        public CyclicFunctionTaskThatWontStop(string code, TimeSpan cycleTime, Scheduler scheduler = null) : base(code, cycleTime, scheduler)
        {
            counter = 0;
        }

        public override IEnumerable<string> Execution()
        {
            yield return "Incrementing counter";
            counter++;

            yield return "Cycle done";
        }
    }

    #endregion Custom tasks classes
}