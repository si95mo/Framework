using Core;
using Core.Conditions;
using Core.DataStructures;
using Core.Parameters;
using Hardware;
using System.Threading;

namespace Devices
{
    /// <summary>
    /// Describe a generic device
    /// </summary>
    public interface IDevice : IProperty
    {
        /// <summary>
        /// The <see cref="IDevice"/> collection
        /// of channels. See <see cref="IChannel"/>
        /// and <see cref="Bag{IProperty}"/>
        /// </summary>
        Bag<IChannel> Channels
        { get; }

        /// <summary>
        /// The <see cref="IDevice"/> collection
        /// of parameters. See <see cref="IParameter"/>
        /// and <see cref="Bag{T}"/>
        /// </summary>
        Bag<IParameter> Parameters
        { get; }

        /// <summary>
        /// The <see cref="IDevice"/> collection of
        /// conditions. See <see cref="ICondition"/>
        /// and <see cref="Bag{T}"/>
        /// </summary>
        Bag<ICondition> Conditions
        { get; }

        /// <summary>
        /// The <see cref="IDevice"/> tasks <see cref="CancellationTokenSource"/>
        /// </summary>
        CancellationTokenSource TokenSource
        { get; }

        /// <summary>
        /// Stop the <see cref="IDevice"/>.
        /// This method should handle the safety (i.e. should put the <see cref="IDevice"/>
        /// in a safe condition when something abnormal happens)
        /// </summary>
        void Stop();
    }
}