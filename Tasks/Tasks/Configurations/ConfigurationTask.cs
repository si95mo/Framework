using Core.DataStructures;
using Core.Parameters;
using Diagnostic;
using System;

namespace Tasks.Configurations
{
    public abstract class ConfigurationTask : Awaitable, IConfigurationTask
    {
        public Bag<IParameter> Settings { get; private set; }
        public event EventHandler<ConfigurationTaskCreateEventArgs> Created;

        protected ConfigurationTask(string code, Scheduler scheduler = null) : base(code, scheduler)
        {
            Settings = new Bag<IParameter>();

            InputParameters.Added += InputParameters_Added;
            InputParameters.Removed += InputParameters_Removed;
            InputParameters.Cleared += InputParameters_Cleared;

            Created?.Invoke(this, new ConfigurationTaskCreateEventArgs(this));
            Status.ValueChanged += Status_ValueChanged;
        }

        private void InputParameters_Added(object sender, BagChangedEventArgs<IParameter> e)
        {
            bool added = Settings.Add(e.Item);

            if (!added)
            {
                Logger.Error($"Unable to add {e.Item.Code} to {Code} {nameof(Settings)}");
            }
        }

        private void InputParameters_Removed(object sender, BagChangedEventArgs<IParameter> e)
        {
            bool removed = Settings.Remove(e.Item.Code);

            if (!removed)
            {
                Logger.Error($"Unable to remove {e.Item.Code} to {Code} {nameof(Settings)}");
            }
        }

        private void InputParameters_Cleared(object sender, BagClearedEventArgs<IParameter> e)
        {
            Settings.Clear();
        }

        private void Status_ValueChanged(object sender, Core.ValueChangedEventArgs e)
        {
            if(Status.Value == System.Threading.Tasks.TaskStatus.Running)
            {
                if (ServiceBroker.CanProvide<ConfigurationsService>())
                {
                    ConfigurationsService service = ServiceBroker.GetService<ConfigurationsService>();
                    service.ConnectFromSystemParameters(this);
                }
                else
                {
                    Logger.Error($"{nameof(ConfigurationsService)} not provided");
                }
            }
        }
    }
}
