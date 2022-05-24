using IO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Configuration
{
    /// <summary>
    /// Handle the configuration file read operations
    /// </summary>
    internal class ConfigurationHandler : FileHandler
    {
        private static string configPath;
        private static Dictionary<string, ConfigurationItem> items;

        /// <summary>
        /// Initialize the <see cref="ConfigurationHandler"/>
        /// </summary>
        /// <param name="path">The configuration file path</param>
        public static void Initialize(string path = "config/config.json")
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

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new[] { new DynamicJsonConverter() });

            dynamic values = serializer.Deserialize(config, typeof(object));

            foreach(var item in values.Items)
            {
                string name = Convert.ToString(item.Name);
                ConfigurationItem configurationItem = new ConfigurationItem(name, item);
                items.Add(configurationItem.Name, configurationItem);
            }

            return items;
        }
    }
}