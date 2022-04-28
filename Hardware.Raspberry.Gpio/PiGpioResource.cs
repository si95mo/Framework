using Core;
using Core.DataStructures;
using System.Threading.Tasks;
using Unosquare.RaspberryIO;

namespace Hardware.Raspberry
{
    /// <summary>
    /// Implement a resource that grants access to Raspberry GPIO
    /// </summary>
    public class PiGpioResource : Resource
    {
        private int pollingInterval;

        private Task pollingTask;

        public override bool IsOpen => Status.Value == ResourceStatus.Executing || Status.Value == ResourceStatus.Stopped;

        /// <summary>
        /// Create a new instance of <see cref="PiGpioResource"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="pollingInterval">The polling interval (in milliseconds)</param>
        public PiGpioResource(string code, int pollingInterval) : base(code)
        {
            this.pollingInterval = pollingInterval;

            pollingTask = null;

            Channels.ItemAdded += Channels_ItemAdded;
        }

        private void Channels_ItemAdded(object sender, BagChangedEventArgs<IProperty> e)
        {
            if (e.Item is IPiChannel)
            {
                IPiChannel piChannel = e.Item as IPiChannel;
                Pi.Gpio[piChannel.PinNumber].PinMode = piChannel.PinMode;
            }
        }

        public override async Task Restart()
        {
            Stop();
            await Start();
        }

        public override Task Start()
        {
            Status.Value = ResourceStatus.Starting;

            pollingTask = CreatePollingTask();
            Status.Value = ResourceStatus.Executing;
            pollingTask.Start();

            return Task.CompletedTask;
        }

        public override void Stop()
        {
            Status.Value = ResourceStatus.Stopping;
            Task.Delay(2 * pollingInterval);
            Status.Value = pollingTask.Status == TaskStatus.RanToCompletion ? ResourceStatus.Stopped : ResourceStatus.Failure;
        }

        /// <summary>
        /// Create the polling <see cref="Task"/>
        /// </summary>
        /// <returns>The polling <see cref="Task"/></returns>
        private Task CreatePollingTask()
        {
            Task t = new Task(async () =>
                {
                    while (Status.Value == ResourceStatus.Executing)
                    {
                        foreach (IChannel channel in Channels)
                        {
                            if (channel is IPiChannel piChannel)
                            {
                                if (piChannel is PiInputChannel)
                                    (piChannel as PiInputChannel).Value = Pi.Gpio[piChannel.PinNumber].Read();
                                else
                                {
                                    if (piChannel is PiOutputChannel)
                                        Pi.Gpio[piChannel.PinNumber].Write((piChannel as PiOutputChannel).Value);
                                }
                            }
                        }

                        await Task.Delay(pollingInterval);
                    }
                }
            );
            return t;
        }
    }
}