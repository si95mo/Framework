using Core;
using Diagnostic;
using IO;
using System;
using System.Collections.Generic;
using System.IO;

namespace Configurations
{
    /// <summary>
    /// Handle the <see cref="IConfiguration"/> file in memory
    /// </summary>
    public class Configuration : IConfiguration
    {
        private string code;

        #region IProperty implementation

        public string Code => code;

        public object ValueAsObject { get => Items; set => _ = value; }

        public Type Type => typeof(Configuration);

        #endregion IProperty implementation

        #region IProperty<T> implementation

        public event EventHandler<ValueChangedEventArgs> ValueChanged;
        public event EventHandler<ValueSetEventArgs> ValueSet;

        /// <summary>
        /// The <see cref="IProperty"/> value. The same as <see cref="Items"/>
        /// </summary>
        public Dictionary<string, ConfigurationItem> Value { get => Items; set => _ = value; }

        #endregion IProperty<T> implementation

        /// <summary>
        /// The collection of <see cref="ConfigurationItem"/> stored in <see cref="Configuration"/>.
        /// The same as <see cref="Value"/>
        /// </summary>
        public Dictionary<string, ConfigurationItem> Items { get; private set; }

        /// <summary>
        /// Create a new instance of <see cref="Configuration"/>
        /// </summary>
        /// <remarks>By default, <paramref name="code"/> is equal to <paramref name="fileName"/> (changeable by specifying a value to <paramref name="code"/>)</remarks>
        /// <param name="code">The code</param>
        /// <param name="fileName">The configuration file path</param>
        public Configuration(string code = "Configuration", string fileName = "config.json")
        {
            this.code = code;

            string path = Path.Combine(Paths.Configuration, fileName);
            if (File.Exists(path))
            {
                ConfigurationHandler.Initialize(path);
                Items = ConfigurationHandler.Parse();
            }
            else
            {
                Logger.Warn($"Configuration file {fileName} not found @ {path}");
            }
        }

        public bool TryGetSection(string sectionName, out dynamic value)
        {
            bool found = Value.TryGetValue(sectionName, out ConfigurationItem item);
            if (found)
            {
                value = item.Value;
            }
            else
            {
                value = null;
            }

            return found;
        }

        public bool TryConvertToEnum<T>(string enumField, out T result) where T : struct
        {
            bool succeeded = Enum.TryParse(enumField, out result);
            return succeeded;
        }

        public void ConnectTo(IProperty property)
        {
            Logger.Error($"Trying to connect a {nameof(Configuration)}, operation not allowed");
        }
    }
}