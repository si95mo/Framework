using Hardware.Resources;
using System;
using System.Threading.Tasks;

namespace Hardware.Tcp.IntegrationTest
{
    internal class Program
    {
        private const string IpAddress = "127.0.0.1";
        private const int Port = 4065;

        private static TcpResource resource;
        private static TcpChannel firstChannel, secondChannel;

        private static async Task Main(string[] args)
        {
            Console.WriteLine("Initializing...");
            await Initialize(IpAddress, Port);

            if (resource.Status.Value == ResourceStatus.Executing)
            {
                Console.WriteLine("Initialization done");

                while (true)
                {
                    Console.WriteLine(firstChannel.Code + ": " + firstChannel.Value);
                    Console.WriteLine(secondChannel.Code + ": " + secondChannel.Value);

                    await Task.Delay(1000);
                }
            }
            else
                Console.WriteLine("Error during initialization");
        }

        /// <summary>
        /// Initialize the hardware resources
        /// </summary>
        /// <param name="ipAddress">The ip address</param>
        /// <param name="port">The port</param>
        /// <returns>The (async) <see cref="Task"/></returns>
        private static async Task Initialize(string ipAddress, int port)
        {
            resource = new TcpResource("TcpResource", ipAddress, port);

            firstChannel = new TcpChannel("FirstChannel", "1", resource, 1000, true, true);
            secondChannel = new TcpChannel("SecondChannel", "2", resource, 1000, true, true);

            await resource.Start();
        }
    }
}