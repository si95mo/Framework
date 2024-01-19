using Core;
using Core.DataStructures;
using Core.Parameters;
using System;

namespace Tasks.Configurations
{
    /// <summary>
    /// The <see cref="IConfigurationTask"/> created <see cref="EventArgs"/>
    /// </summary>
    public class ConfigurationTaskCreateEventArgs : EventArgs
    {
        /// <summary>
        /// The <see cref="IConfigurationTask"/> involved
        /// </summary>
        public IConfigurationTask Task { get; private set; }

        /// <summary>
        /// Create a new instance of <see cref="ConfigurationTaskCreateEventArgs"/>
        /// </summary>
        /// <param name="task">The <see cref="IConfigurationTask"/> created</param>
        public ConfigurationTaskCreateEventArgs(IConfigurationTask task)
        {
            Task = task;
        }
    }

    /// <summary>
    /// Define a basic configuration <see cref="IAwaitable"/> task
    /// </summary>
    public interface IConfigurationTask : IAwaitable
    {
        /// <summary>
        /// The <see cref="Bag{T}"/> with all the <see cref="IParameter"/> that will be stored in the file system and that will be stored
        /// </summary>
        Bag<IParameter> Settings { get; }

        /// <summary>
        /// The created <see cref="EventHandler{TEventArgs}"/>
        /// </summary>
        event EventHandler<ConfigurationTaskCreateEventArgs> Created;
    }
}
