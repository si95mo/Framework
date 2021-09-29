namespace Configuration
{
    /// <summary>
    /// Define a configuration file item
    /// </summary>
    public class ConfigurationItem
    {
        private string name;
        private string value;

        /// <summary>
        /// The <see cref="ConfigurationItem"/> name
        /// </summary>
        public string Name => name;

        /// <summary>
        /// The <see cref="ConfigurationItem"/> value
        /// </summary>
        public string Value => value;

        /// <summary>
        /// Create a new instance of <see cref="ConfigurationItem"/>
        /// </summary>
        /// <param name="name">The item name</param>
        /// <param name="value">The item value</param>
        public ConfigurationItem(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public override string ToString()
        {
            string description = $"{name}: {value}";
            return description;
        }
    }
}
