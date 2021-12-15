using Sharp7;
using System;
using System.Threading.Tasks;

namespace Hardware.Snap7
{
    /// <summary>
    /// Implement a <see cref="Resource"/> that communicate with
    /// Siemens PLC using the Snap7 protocol
    /// </summary>
    public class Snap7Resource : Resource
    {
        private string ipAddress;
        private int rack;
        private int slot;

        private S7Client client;

        public override bool IsOpen => client.Connected;

        /// <summary>
        /// Create a new instance of <see cref="Snap7Resource"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="ipAddress">The ip address</param>
        /// <param name="rack">The rack number</param>
        /// <param name="slot">The slot number</param>
        public Snap7Resource(string code, string ipAddress, int rack, int slot) : base(code)
        {
            this.ipAddress = ipAddress;
            this.rack = rack;
            this.slot = slot;

            client = new S7Client();
        }

        public override async Task Restart()
        {
            Stop();
            await Start();
        }

        public override Task Start()
        {
            Status.Value = ResourceStatus.Starting;
            int result = client.ConnectTo(ipAddress, rack, slot);

            if (result == 0)
                Status.Value = ResourceStatus.Executing;
            else
            {
                string message = $"Unable to connect to the Siemens PLC! Error code: {result}.{Environment.NewLine}" +
                    $"Description: {client.ErrorText(result)}";
                HandleException(message);
            }

            return Task.CompletedTask;
        }

        public override void Stop()
        {
            Status.Value = ResourceStatus.Stopping;
            int result = client.Disconnect();

            if (result == 0)
                Status.Value = ResourceStatus.Stopped;
            else
            {
                string message = $"Unable to disconnect to the Siemens PLC at {ipAddress}! (Error code: {result}){Environment.NewLine}" +
                    $"Description: {client.ErrorText(result)}";
                HandleException(message);
            }
        }

        /// <summary>
        /// Receive a value from the PLC and store it in the relative channel
        /// </summary>
        /// <param name="code">The channel code</param>
        /// <returns></returns>
        internal async Task Receive(string code)
        {
        }

        /// <summary>
        /// Send a value stored in the relative channel to the PLC
        /// </summary>
        /// <param name="code">The channel code</param>
        internal void Send(string code)
        {
        }
    }
}