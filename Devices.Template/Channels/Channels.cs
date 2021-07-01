using Hardware;

namespace Devices.Template.Channels
{
    /// <summary>
    /// Contains all of the <see cref="Device{TChannel, TParameter}"/> channels.
    /// So, in <see cref="Device{TChannel, TParameter}"/> <see cref="Channels"/> is
    /// the actual type of the generics <see cref="TChannel"/>.
    /// </summary>
    public class Channels
    {
        private AnalogInput dummy = new AnalogInput("DummyAnalogInput");

        // Fore each channel, there must be a public property with the getter
        public AnalogInput Dummy => dummy;
    }
}