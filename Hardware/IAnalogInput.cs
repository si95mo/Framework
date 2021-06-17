using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardware
{
    /// <summary>
    /// Define a generic analog input channel
    /// </summary>
    public interface IAnalogInput : IChannel<double>
    {
    }
}
