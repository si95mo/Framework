using Core.DataStructures;

namespace Hardware
{
    /// <summary>
    /// Define a <see cref="Service{T}"/> for <see cref="IChannel"/>
    /// </summary>
    public class ChannelsService : Service<IChannel>
    {
        /// <summary>
        /// Create a new instance of <see cref="ChannelsService"/>
        /// </summary>
        public ChannelsService() : base()
        { }

        /// <summary>
        /// Create a new instance of <see cref="ChannelsService"/>
        /// </summary>
        /// <param name="code">The code</param>
        public ChannelsService(string code) : base(code)
        { }

        #region Custom get

        /// <summary>
        /// Get a channel as an <see cref="IAnalogInput"/>
        /// </summary>
        /// <param name="code">The channel code</param>
        /// <returns>The <see cref="IAnalogInput"/>, or <see langword="null"/> if no cast could be applied</returns>
        public IAnalogInput GetAnalogInput(string code)
        {
            IChannel channel = Get(code);
            return channel as IAnalogInput;
        }

        /// <summary>
        /// Get a channel as an <see cref="IAnalogOutput"/>
        /// </summary>
        /// <param name="code">The channel code</param>
        /// <returns>The <see cref="IAnalogOutput"/>, or <see langword="null"/> if no cast could be applied</returns>
        public IAnalogOutput GetAnalogOutput(string code)
        {
            IChannel channel = Get(code);
            return channel as IAnalogOutput;
        }

        /// <summary>
        /// Get a channel as an <see cref="IDigitalInput"/>
        /// </summary>
        /// <param name="code">The channel code</param>
        /// <returns>The <see cref="IDigitalInput"/>, or <see langword="null"/> if no cast could be applied</returns>
        public IDigitalInput GetDigitalInput(string code)
        {
            IChannel channel = Get(code);
            return channel as IDigitalInput;
        }

        /// <summary>
        /// Get a channel as an <see cref="IDigitalOutput"/>
        /// </summary>
        /// <param name="code">The channel code</param>
        /// <returns>The <see cref="IDigitalOutput"/>, or <see langword="null"/> if no cast could be applied</returns>
        public IDigitalOutput GetDigitalOutput(string code)
        {
            IChannel channel = Get(code);
            return channel as IDigitalOutput;
        }

        #endregion Custom get
    }
}