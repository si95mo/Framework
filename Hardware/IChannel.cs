using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardware
{
    /// <summary>
    /// Describe a generic (hardware) channel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IChannel<T> : IProperty
    {
        /// <summary>
        /// The <see cref="IChannel"/> code
        /// </summary>
        T Value
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Connects an <see cref="IChannel"/> to another
        /// in order to propagate its value;
        /// </summary>
        /// <param name="channel">The destination <see cref="IChannel"/></param>
        void ConnectTo(IChannel<T> channel);
    }
}
