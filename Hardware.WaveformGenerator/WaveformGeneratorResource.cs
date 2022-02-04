using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Hardware.WaveformGenerator
{
    /// <summary>
    /// Define the waveform type
    /// </summary>
    public enum WaveformType
    {
        /// <summary>
        /// A non type
        /// </summary>
        None = 0,

        /// <summary>
        /// A square waveform
        /// </summary>
        Square = 1,

        /// <summary>
        /// A sine waveform
        /// </summary>
        Sine = 2,

        /// <summary>
        /// A triangular waveform
        /// </summary>
        Triangular = 3,

        /// <summary>
        /// A sawtooth waveform
        /// </summary>
        Sawtooth = 4
    }

    /// <summary>
    /// Implement a waveform generator resource
    /// </summary>
    public class WaveformGeneratorResource : Resource
    {
        private WaveformType waveformType;
        private double amplitude;
        private double period;
        private double frequency;
        private double offset;
        private double phase;

        private AnalogOutput output;

        private Task generationTask;

        public override bool IsOpen => Status.Value == ResourceStatus.Executing;

        /// <summary>
        /// Create a new instance of <see cref="WaveformGeneratorResource"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="waveformType">The <see cref="WaveformType"/></param>
        /// <param name="frequency">The waveform frequency (in Hertz)</param>
        /// <param name="amplitude">The amplitude (in Volt)</param>
        /// <param name="output">The <see cref="AnalogOutput"/> that will store the waveform value</param>
        /// <param name="offset">The offset (in Volt)</param>
        /// <param name="phase">The phase (in radiants)</param>
        public WaveformGeneratorResource(string code, WaveformType waveformType, double amplitude, double frequency,
            AnalogOutput output, double offset = 0, double phase = 0) : base(code)
        {
            this.waveformType = waveformType;
            this.amplitude = amplitude;
            this.frequency = frequency;
            this.offset = offset;
            this.phase = phase;

            period = 1000 / frequency; // In milliseconds
            generationTask = default;

            this.output = new AnalogOutput($"{output.Code}.{Code}", output.MeasureUnit, output.Format);
            this.output.ConnectTo(output);
        }

        public override async Task Restart()
        {
            Stop();
            await Start();
        }

        public override Task Start()
        {
            Status.Value = ResourceStatus.Starting;

            generationTask = CreateGenerationTask();

            Status.Value = ResourceStatus.Executing;
            generationTask.Start();

            return Task.CompletedTask;
        }

        public override void Stop()
        {
            if (Status.Value == ResourceStatus.Executing)
            {
                Status.Value = ResourceStatus.Stopping;

                if (generationTask != default && !generationTask.IsCompleted)
                {
                    int timeout = (int)TimeSpan.FromMilliseconds(2 * period).TotalMilliseconds;
                    generationTask.Wait(timeout);
                }

                generationTask = default;

                Status.Value = ResourceStatus.Stopped;
            }
        }

        /// <summary>
        /// Create the generation task
        /// </summary>
        /// <returns>The generation <see cref="Task"/></returns>
        private Task CreateGenerationTask()
        {
            Task task = new Task(async () =>
                {
                    double time = 0d;
                    double t = 0d;
                    double startTime = Stopwatch.GetTimestamp() / Stopwatch.Frequency;

                    while (Status.Value == ResourceStatus.Executing)
                    {
                        time = (Stopwatch.GetTimestamp() - startTime) / Stopwatch.Frequency - startTime;
                        t = 2 * Math.PI * frequency * time + phase;

                        switch (waveformType)
                        {
                            case WaveformType.Sine: // sin(2 * pi * t)
                                output.Value = amplitude * Math.Sin(t) + offset;
                                break;

                            case WaveformType.Square: // sign(sin(2 * pi * t))
                                output.Value = amplitude * Math.Sign(Math.Sin(t)) + offset;
                                break;

                            case WaveformType.Triangular: // 2 * abs(t - 2 * floor(t / 2) - 1) - 1
                                output.Value = amplitude * (1 - 4 * Math.Abs(Math.Round(t - 0.25) - (t - 0.25))) + offset;
                                break;

                            case WaveformType.Sawtooth: // 2 * (t/a - floor(t/a + 1/2))
                                output.Value = amplitude * (2 * (t - Math.Floor(t + 0.5))) + offset;
                                break;
                        }

                        await Task.Delay(0);
                    }
                }
            );

            return task;
        }
    }
}