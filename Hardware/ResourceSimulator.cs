using System.Threading.Tasks;

namespace Hardware
{
    /// <summary>
    /// Define a <see cref="Resource"/> simulator (basically just as a channel collection)
    /// </summary>
    public class ResourceSimulator : Resource
    {
        public override bool IsOpen => Status.Value == ResourceStatus.Executing;

        /// <summary>
        /// Create a new instance of <see cref="ResourceSimulator"/>
        /// </summary>
        /// <param name="code"></param>
        public ResourceSimulator(string code) : base(code) 
        { }

        public override Task Restart()
        {
            Start();
            Stop();

            return Task.CompletedTask;
        }

        public override Task Start()
        {
            Status.Value = ResourceStatus.Executing;
            return Task.CompletedTask;
        }

        public override void Stop()
        {
            Status.Value = ResourceStatus.Stopped;
        }
    }
}
