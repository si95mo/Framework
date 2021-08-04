using Devices.Template;

namespace Benches.Template.Devices
{
    public class Devices
    {
        private NewDevice device = new NewDevice("DummyDeviceInBench");

        public NewDevice Device => device;
    }
}