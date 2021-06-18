using Core;
using System;

namespace Hardware
{
    /// <summary>
    /// Describe a generic (hardware) channel
    /// </summary>
    public interface IChannel : IProperty
    {
    }

    /// <summary>
    /// Describe a generic (hardware) channel
    /// with a defined type of value
    /// </summary>
    /// <typeparam name="T">The type of value</typeparam>
    public interface IChannel<T> : IProperty<T>, IChannel
    {
        /// <summary>
        /// The <see cref="IChannel"/> code
        /// </summary>
        new T Value
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Connects an <see cref="IChannel"/> to another
        /// in order to propagate its value;
        /// </summary>
        /// <param name="channel">The destination <see cref="IChannel"/></param>
        new void ConnectTo(IProperty<T> channel);
    }
}
