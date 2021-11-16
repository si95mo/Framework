using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardware.Libnodave
{
    /// <summary>
    /// Implement an analog output for the <see cref="LibnodaveResource"/>
    /// </summary>
    public class LibnodaveAnalogOutput : LibnodaveAnalogChannel
    {
        /// <summary>
        /// Create a new instance of <see cref="LibnodaveAnalogOutput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="memoryAddress">The memory address</param>
        /// <param name="resource">The resource</param>
        /// <param name="bytes">The <see cref="RepresentationBytes"/></param>
        /// <param name="representation">The <see cref="NumericRepresentation"/></param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        public LibnodaveAnalogOutput(string code, int memoryAddress, IResource resource, RepresentationBytes bytes, 
            NumericRepresentation representation, string measureUnit = "", string format = "0.000") 
            : base(code, memoryAddress, resource, bytes, representation, measureUnit, format)
        {
            ValueChanged += LibnodaveAnalogOutput_ValueChanged;
        }

        private async void LibnodaveAnalogOutput_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            await (resource as LibnodaveResource).Send(code);
        }
    }
}
