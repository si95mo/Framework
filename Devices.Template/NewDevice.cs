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

        protected override void ConnectChannelsToParameters()
        {
            throw new System.NotImplementedException();
        }
    }
}