using System.Collections.Generic;

namespace Configuration
{
    /// <summary>
    /// Handle the configuration file
    /// </summary>
    public class Configuration
    {
        private Dictionary<string, ConfigurationItem> items;

        /// <summary>
        /// The collection of <see cref="ConfigurationItem"/>
        /// stored in <see cref="Configuration"/>
        /// </summary>
        public Dictionary<string, ConfigurationItem> Items => items;

        /// <summary>
        /// Create a new instance of <see cref="Configuration"/>
        /// </summary>
        /// <param name="path">The configuration file path</param>
        public Configuration(string path = "config/config.ini")
        {
            ConfigurationHandler.Initialize(path);
            items = ConfigurationHandler.Parse();
        }
    }
}