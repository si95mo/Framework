using Core.DataStructures;
using Diagnostic;
using System;
using Tasks.Configurations;

namespace Tasks
{
    /// <summary>
    /// Define a <see cref="Service{T}"/> for <see cref="IAwaitable"/> tasks
    /// </summary>
    public class TasksService : Service<IAwaitable>
    {
        /// <summary>
        /// The <see cref="Tasks.Configurations.ConfigurationsService"/>
        /// </summary>
        public ConfigurationsService ConfigurationsService { get; private set; }

        /// <summary>
        /// Create a new instance of <see cref="TasksService"/>
        /// </summary>
        public TasksService() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Create a new instance of <see cref="TasksService"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="configurations">The <see cref="Tasks.Configurations.ConfigurationsService"/> to use. Leave <see langword="null"/> to use a new service</param>
        public TasksService(string code, ConfigurationsService configurations = null) : base(code)
        {
            ConfigurationsService = configurations ?? new ConfigurationsService($"{code}.Configurations");

            if(!ServiceBroker.CanProvide<ConfigurationsService>())
            {
                ServiceBroker.Provide(ConfigurationsService);
            }

            Subscribers.Added += Subscribers_Added;
        }

        private void Subscribers_Added(object sender, BagChangedEventArgs<IAwaitable> e)
        {
            if (e.Item is IConfigurationTask configurationTask)
            {
                ConfigurationsService.Add(configurationTask);
                Logger.Trace($"Configuration task with code \"{configurationTask.Code}\" added also to the {nameof(Tasks.Configurations.ConfigurationsService)}");
            }
        }

        public override void Dispose()
        {
            ConfigurationsService.Dispose();
            base.Dispose();
        }
    }
}