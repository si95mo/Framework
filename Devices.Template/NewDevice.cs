namespace Devices.Template
{
    public class NewDevice : Device
    {
        /// <summary>
        /// Create a new instance of <see cref="NewDevice"/>
        /// </summary>
        /// <param name="code">The code</param>
        public NewDevice(string code) : base(code)
        {
            // Add stuff here
        }

        public override void Stop()
        {
            throw new System.NotImplementedException();
        }

        protected override void ConnectChannelsToParameters()
        {
            throw new System.NotImplementedException();
        }
    }
}