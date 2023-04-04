using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hardware
{
    /// <summary>
    /// Implement a <see cref="Resource"/> that will have channels with system infos
    /// </summary>
    public class SystemInfoResource : Resource
    {
        private Timer timer;

        public override bool IsOpen => Status.Value == ResourceStatus.Executing;

        /// <summary>
        /// The polling time
        /// </summary>
        public TimeSpan PollingTime { get; private set; }

        private Dictionary<IChannel, Func<object>> updaters;

        /// <summary>
        /// Create a new instance of <see cref="SystemInfoResource"/>
        /// </summary>
        public SystemInfoResource() : this(Guid.NewGuid().ToString(), TimeSpan.FromMilliseconds(1000d))
        { }

        /// <summary>
        /// Create a new instance of <see cref="SystemInfoResource"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="pollingTime">The polling time <see cref="TimeSpan"/></param>
        public SystemInfoResource(string code, TimeSpan pollingTime) : base(code)
        {
            PollingTime = pollingTime;
            updaters = new Dictionary<IChannel, Func<object>>();

            AddChannel(new AnalogInput("Year", format: "0"), () => double.Parse(DateTime.Now.ToString("yyyy")));
            AddChannel(new AnalogInput("Month", format: "0"), () => double.Parse(DateTime.Now.ToString("MM")));
            AddChannel(new AnalogInput("Day", format: "0"), () => double.Parse(DateTime.Now.ToString("dd")));
            AddChannel(new AnalogInput("Hours", format: "0"), () => double.Parse(DateTime.Now.ToString("HH")));
            AddChannel(new AnalogInput("Minutes", format: "0"), () => double.Parse(DateTime.Now.ToString("mm")));
            AddChannel(new AnalogInput("Seconds", format: "0"), () => double.Parse(DateTime.Now.ToString("ss")));
            AddChannel(new AnalogInput("Milliseconds", format: "0"), () =>  double.Parse(DateTime.Now.ToString("fff")));
            AddChannel(
                new AnalogInput("CalendarWeek", format: "0"), 
                () =>
                {
                    DateTime time = DateTime.Now;
                    DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
                    if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
                        time = time.AddDays(3d);

                    double calendarWeek = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                    return calendarWeek;
                }
            );

            AddChannel(new AnalogInput("DayOfTheYear", format: "0"), () => (double)CultureInfo.InvariantCulture.Calendar.GetDayOfYear(DateTime.Now));

            Process process = Process.GetCurrentProcess();
            PerformanceCounter memCounter = new PerformanceCounter("Process", "Working Set - Private", process.ProcessName, true);
            AddChannel(new AnalogInput("Memory", measureUnit: "MB", format: "0.0"), () => memCounter.NextValue() / 1024d / 1024d);

            PerformanceCounter cpuCounter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName, true);
            AddChannel(new AnalogInput("CPU", measureUnit: "%", format: "0.0"), () => (double)cpuCounter.NextValue());

            AddChannel(new DigitalInput("Responding"), () => process.Responding);

            AddChannel(new StreamInput("MachineName", Encoding.ASCII), () => Encoding.ASCII.GetBytes(Environment.MachineName));
            AddChannel(new StreamInput("UserDomain", Encoding.ASCII), () => Encoding.ASCII.GetBytes(Environment.UserDomainName));
            AddChannel(new StreamInput("User", Encoding.ASCII), () => Encoding.ASCII.GetBytes(Environment.UserName));
            AddChannel(new StreamInput("UtcNow", Encoding.ASCII), () => Encoding.ASCII.GetBytes(DateTime.UtcNow.ToString()));
            AddChannel(new StreamInput("Now", Encoding.ASCII), () => Encoding.ASCII.GetBytes(DateTime.Now.ToString()));

            timer = new Timer((x) => UpdateChannels(), this, 0, (int)PollingTime.TotalMilliseconds);
        }

        /// <summary>
        /// Create a new instance of <see cref="SystemInfoResource"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="pollingTimeInMilliseconds">The polling time in milliseconds</param>
        public SystemInfoResource(string code, int pollingTimeInMilliseconds) : this(code, TimeSpan.FromMilliseconds(pollingTimeInMilliseconds))
        { }

        #region Resource implementation

        public override Task Restart()
        {
            Stop();
            Start();

            return Task.CompletedTask;
        }

        public override Task Start()
        {
            Status.Value = ResourceStatus.Starting;
            timer.Change(0, (int)PollingTime.TotalMilliseconds);

            Status.Value = ResourceStatus.Executing;
            return Task.CompletedTask;
        }

        public override void Stop()
        {
            Status.Value = ResourceStatus.Stopped;
        }

        #endregion Resource implementation

        /// <summary>
        /// Add a new <see cref="IChannel"/> with the relative updater
        /// </summary>
        /// <param name="channel">The <see cref="IChannel"/> to add</param>
        /// <param name="updater">The update <see cref="Func{T, TResult}"/></param>
        private void AddChannel(IChannel channel, Func<object> updater)
        {
            Channels.Add(channel);
            updaters.Add(channel, updater);
        }

        /// <summary>
        /// Update the channels
        /// </summary>
        private void UpdateChannels()
        {
            foreach (KeyValuePair<IChannel, Func<object>> update in updaters)
                update.Key.ValueAsObject = update.Value.Invoke();
        }
    }
}
