using IO;
using System;
using System.Collections.Generic;

namespace Configuration
{
    internal class ConfigurationHandler : FileHandler
    {
        private static string configPath;
        private static Dictionary<string, ConfigurationItem> items;

        /// <summary>
        /// Initialize the <see cref="ConfigurationHandler"/>
        /// </summary>
        /// <param name="path">The configuration file path</param>
        public static void Initialize(string path= "config/config.ini")
        {
            items = new Dictionary<string, ConfigurationItem>();
            configPath = path;
        }

        /// <summary>
        /// Parse the configuration file
        /// </summary>
        /// <returns>The <see cref="Dictionary{TKey, TValue}"/> of <see cref="ConfigurationItem"/></returns>
        public static Dictionary<string, ConfigurationItem> Parse()
        {
            string config = Read(configPath);
            string[] lines = config.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None
            );

            ConfigurationItem item;
            string name, value;
            foreach(string line in lines)
            {
                if (line.CompareTo("") != 0)
                {
                    (name, value) = (line.Split(':')[0], line.Split(':')[1]);
                    item = new ConfigurationItem(name, value);

                    items.Add(item.Name, item);
                }
            }

            return items;
        }
    }
}
