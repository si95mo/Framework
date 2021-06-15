using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardware
{
    public interface IChannel
    {
        /// <summary>
        /// The value of the <see cref="IChannel"/>
        /// </summary>
        double Value
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Connects an <see cref="IChannel"/> to another
        /// in order to propagate its value;
        /// </summary>
        /// <param name="channel">The destination <see cref="IChannel"/></param>
        void ConnectTo(IChannel channel);
    }
}
