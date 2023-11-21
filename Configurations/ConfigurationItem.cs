using System;

namespace Configurations
{
    /// <summary>
    /// Define a configuration file item
    /// </summary>
    public class ConfigurationItem
    {
        /// <summary>
        /// The <see cref="ConfigurationItem"/> name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The <see cref="ConfigurationItem"/> value
        /// </summary>
        public dynamic Value { get; private set; }

        /// <summary>
        /// Create a new instance of <see cref="ConfigurationItem"/>
        /// </summary>
        /// <param name="name">The item name</param>
        /// <param name="value">The item value</param>
        public ConfigurationItem(string name, dynamic value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Convert a <see langword="dynamic"/> value into the respective <see cref="Enum"/>
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Enum"/> to convert</typeparam>
        /// <param name="value">The <see langword="dynamic"/> value to convert (must be mapped into a <see cref="string"/>)</param>
        /// <param name="result">The result of the conversion</param>
        /// <returns><see langword="true"/> if the conversion succeeded, <see langword="false"/> otherwise</returns>
        public bool ToEnum<T>(dynamic value, out T result) where T : Enum
        {
            bool parsed = Enum.TryParse(value, true, out result);
            return parsed;
        }

        public override string ToString()
        {
            string description = $"{Name}: {Value}";
            return description;
        }
    }
}