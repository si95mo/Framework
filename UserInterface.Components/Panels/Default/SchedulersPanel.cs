using Core.DataStructures;
using Diagnostic;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tasks;

namespace UserInterface.Controls.Panels.Default
{
    /// <summary>
    /// Define a <see cref="ContainerControl"/> for <see cref="IScheduler"/>
    /// </summary>
    public partial class SchedulersPanel : ContainerPanel
    {
        /// <summary>
        /// The <see cref="SchedulersService"/>
        /// </summary>
        public SchedulersService Service { get; set; }

        public SchedulersPanel() : base()
        {
            InitializeComponent();

            if (ServiceBroker.CanProvide<SchedulersService>())
            {
                Service = ServiceBroker.GetService<SchedulersService>();
                foreach (IScheduler scheduler in Service.GetAll())
                {
                    AddScheduler(scheduler);
                }
            }
            else
            {
                Logger.Info($"Unable to add the {nameof(SchedulersPanel)} because {nameof(ServiceBroker)} cannot provide the {nameof(SchedulersService)}");
            }
        }

        /// <summary>
        /// Add a new <see cref="IScheduler"/> to the controls
        /// </summary>
        /// <param name="scheduler">The <see cref="IScheduler"/> to add</param>
        private void AddScheduler(IScheduler scheduler)
        {
            if (!InvokeRequired)
            {
                SchedulerControl control = new SchedulerControl(scheduler);
                Container.AddControl(control);
            }
            else
            {
                BeginInvoke(new Action(() => AddScheduler(scheduler)));
            }
        }
    }
}
