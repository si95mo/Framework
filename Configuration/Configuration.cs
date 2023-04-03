using Core;
using Diagnostic;
using System;
using System.Collections.Generic;
using System.IO;

namespace Configuration
{
    /// <summary>
    /// Handle the configuration file in memory
    /// </summary>
    public class Configuration : IProperty<Dictionary<string, ConfigurationItem>>
    {
        private string code;
        private Dictionary<string, ConfigurationItem> items;

        #region IProperty implementation

        public string Code => code;

        public object ValueAsObject { get => Items; set => _ = value; }

        public Type Type => typeof(Configuration);

        #endregion IProperty implementation

        #region IProperty<T> implementation

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        /// <summary>
        /// The <see cref="IProperty"/> value. The same as <see cref="Items"/>
        /// </summary>
        public Dictionary<string, ConfigurationItem> Value { get => Items; set => _ = value; }

        #endregion IProperty<T> implementation

        /// <summary>
        /// The collection of <see cref="ConfigurationItem"/> stored in <see cref="Configuration"/>.
        /// The same as <see cref="Value"/>
        /// </summary>
        public Dictionary<string, ConfigurationItem> Items => items;

        /// <summary>
        /// Create a new instance of <see cref="Configuration"/>
        /// </summary>
        /// <remarks>By default, <paramref name="code"/> is equal to <paramref name="path"/> (changeable by specifying a value to <paramref name="code"/>)</remarks>
        /// <param name="code">The code</param>
        /// <param name="path">The configuration file path</param>
        public Configuration(string code = "config/config.json", string path = "config/config.json")
        {
            this.code = code;

            if (File.Exists(path))
            {
                ConfigurationHandler.Initialize(path);
                items = ConfigurationHandler.Parse();
            }
            else
                Logger.Warn($"Configuration file not found at {path}");
        }

        public void ConnectTo(IProperty property)
        {
            throw new NotImplementedException();
        }
    }
}