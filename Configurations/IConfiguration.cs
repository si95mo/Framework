using Core;
using System.Collections.Generic;

namespace Configurations
{
    /// <summary>
    /// Define a basic configuration
    /// </summary>
    public interface IConfiguration : IProperty<Dictionary<string, ConfigurationItem>>
    {
    }
}
