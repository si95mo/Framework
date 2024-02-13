using Core.DataStructures;
using Diagnostic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tasks;
using Tasks.Configurations;

namespace UserInterface.Controls.Panels.Default
{
    /// <summary>
    /// Define a <see cref="ContainerControl"/> for <see cref="IAwaitable"/>
    /// </summary>
    public partial class TasksPanel : ContainerControl
    {
        /// <summary>
        /// The <see cref="TasksService"/>
        /// </summary>
        TasksService Service { get; set; }
        
        /// <summary>
        /// Create a new instance of <see cref="TasksPanel"/>
        /// </summary>
        public TasksPanel() : base()
        {
            InitializeComponent();

            if (ServiceBroker.CanProvide<TasksService>())
            {
                Service = ServiceBroker.GetService<TasksService>();

                Bag<IAwaitable> allTasks = Service.GetAll();
                IEnumerable<IConfigurationTask> configurationTasks = allTasks.OfType<IConfigurationTask>();
                IEnumerable<IAwaitable> tasks = allTasks.Except(configurationTasks);

                Parallel.ForEach(configurationTasks, (x) => AddTask(x));
                Parallel.ForEach(tasks, (x) => AddTask(x));
            }
            else
            {
                Logger.Info($"Unable to add the {nameof(TasksPanel)} because {nameof(ServiceBroker)} cannot provide the {nameof(TasksService)}");
            }
        }

        /// <summary>
        /// Add a new <see cref="IAwaitable"/> to the controls
        /// </summary>
        /// <param name="task">The <see cref="IAwaitable"/> to add</param>
        private void AddTask(IAwaitable task)
        {
            if (!InvokeRequired)
            {
                TaskControl control = new TaskControl(task);
                Container.Add(control);
            }
            else
            {
                BeginInvoke(new Action(() => AddTask(task)));
            }
        }
    }
}
