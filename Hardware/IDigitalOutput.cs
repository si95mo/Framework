using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardware
{
    /// <summary>
    /// Define a generic digital output
    /// </summary>
    public interface  IDigitalOutput : IChannel<bool>
    {
    }
}
