using Core;
using Core.DataStructures;
using Diagnostic;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Unosquare.RaspberryIO;

namespace Hardware.Raspberry
{
    /// <summary>
    /// Implement a resource that grants access to Raspberry GPIO
    /// </summary>
    public class PiGpioResource : Resource
    {
        private int pollingIntervalInMilliseconds;

        private Task pollingTask;

        public override bool IsOpen => Status.Value == ResourceStatus.Executing || Status.Value == ResourceStatus.Stopped;

        /// <summary>
        /// Create a new instance of <see cref="PiGpioResource"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="pollingIntervalInMilliseconds">The polling interval (in milliseconds)</param>
        public PiGpioResource(string code, int pollingIntervalInMilliseconds) : base(code)
        {
            this.pollingIntervalInMilliseconds = pollingIntervalInMilliseconds;

            pollingTask = null;

            Channels.Added += Channels_ItemAdded;
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
            Task.Delay(2 * pollingIntervalInMilliseconds);
            Status.Value = pollingTask.Status == TaskStatus.RanToCompletion ? ResourceStatus.Stopped : ResourceStatus.Failure;
        }

        /// <summary>
        /// Send a value via the <see cref="PiGpioResource"/>
        /// </summary>
        /// <param name="pinNumber">The pin number</param>
        /// <param name="value">The value to write</param>
        internal void Send(int pinNumber, bool value)
            => Pi.Gpio[pinNumber].Write(value);

        /// <summary>
        /// Create the polling <see cref="Task"/>
        /// </summary>
        /// <returns>The polling <see cref="Task"/></returns>
        private Task CreatePollingTask()
        {
            Task t = new Task(async () =>
                {
                    Stopwatch timer = Stopwatch.StartNew();
                    while (Status.Value == ResourceStatus.Executing)
                    {
                        timer.Restart();
                        foreach (IPiChannel channel in Channels.OfType<IPiChannel>()) // The polling task is only for the inputs. The write operation is async
                        {
                            if (channel is PiDigitalInput inputChannel)
                            {
                                inputChannel.Value = Pi.Gpio[channel.PinNumber].Read();
                            }
                            else if (channel is PiOneWireInput oneWireChannel)
                            {
                                oneWireChannel.Read();
                            }
                        }

                        timer.Stop();
                        int elapsedMilliseconds = (int)timer.ElapsedMilliseconds;

                        int timeToWait = pollingIntervalInMilliseconds - elapsedMilliseconds;
                        if (timeToWait < 0)
                        {
                            await Logger.WarnAsync($"{Code} polling cycle exceeded the wanted time by {Math.Abs(timeToWait)}[ms]");
                            timeToWait = 0;
                        }

                        await Task.Delay(timeToWait);
                    }
                }
            );
            return t;
        }
    }
}