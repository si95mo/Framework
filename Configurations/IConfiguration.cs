using Core;
using System.Collections.Generic;

namespace Configurations
{
    /// <summary>
    /// Define a basic configuration
    /// </summary>
    public interface IConfiguration : IProperty<Dictionary<string, ConfigurationItem>>
    {
        /// <summary>
        /// Try to get the specific configuration section
        /// </summary>
        /// <remarks>If <paramref name="sectionName"/> is not found, then <paramref name="value"/> will be <see langword="null"/></remarks>
        /// <param name="sectionName">The section name of interest</param>
        /// <param name="value">The retrieved section, if found</param>
        /// <returns><see langword="true"/> if the operation succeeded, <see langword="false"/> otherwise</returns>
        bool TryGetSection(string sectionName, out dynamic value);
    }
}