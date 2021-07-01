using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devices.Template
{
    public class NewDevice : Device<Channels.Channels, Parameters.Parameters>
    {
        /// <summary>
        /// Create a new instance of <see cref="NewDevice"/>
        /// </summary>
        /// <param name="code">The code</param>
        public NewDevice(string code) : base(code)
        { 
            // Add stuff here
        }
    }
}
