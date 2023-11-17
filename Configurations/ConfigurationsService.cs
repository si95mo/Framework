using Core.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configurations
{
    /// <summary>
    /// Define an <see cref="IService{T}"/> for <see cref="IConfiguration"/>
    /// </summary>
    public class ConfigurationsService : Service<IConfiguration>
    {
        /// <summary>
        /// Create a new instance of <see cref="ConfigurationsService"/>
        /// </summary>
        public ConfigurationsService() : base()
        { }

        /// <summary>
        /// Create a new instance of <see cref="ConfigurationsService"/>
        /// </summary>
        /// <param name="code">The code</param>
        public ConfigurationsService(string code) : base(code)
        { }
    }
}
