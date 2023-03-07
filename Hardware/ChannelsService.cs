using Core.DataStructures;

namespace Hardware
{
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
    }
}
