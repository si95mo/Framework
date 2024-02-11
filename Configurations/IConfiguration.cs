using Core;
using System;
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

        /// <summary>
        /// Try to convert an <paramref name="fieldName"/> as <see cref="string"/> into the relative <see langword="enum"/> value
        /// </summary>
        /// <typeparam name="T">The <see cre="Enum"/> type</typeparam>
        /// <param name="fieldName">The <see cref="Enum"/> field name to convert</param>
        /// <param name="result">The conversion result</param>
        /// <returns><see langword="true"/> if the conversion succeeded, <see langword="false"/> otherwise</returns>
        bool TryConvertToEnum<T>(string fieldName, out T result) where T : struct;
    }
}