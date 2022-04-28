using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hardware.Can.Vector
{
    /// <summary>
    /// Implement a <see cref="Resource"/> for the CAN protocol that communicates using Vector hardware
    /// </summary>
    public class VectorCanResource : Resource, ICanResource
    {
        public Dictionary<int, bool> FilteredCanId { get; private set; }

        public override bool IsOpen => Status.Value == ResourceStatus.Executing || Status.Value == ResourceStatus.Stopped;

        /// <summary>
        /// Create a new instance of <see cref="VectorCanResource"/>
        /// </summary>
        /// <param name="code">The code</param>
        public VectorCanResource(string code) : base(code)
        {
            FilteredCanId = new Dictionary<int, bool>();
        }

        public void DisableLog()
        {
            throw new NotImplementedException();
        }

        public void EnableLog(int maxLogSize = 65535)
        {
            throw new NotImplementedException();
        }

        public string ReadLog()
        {
            throw new NotImplementedException();
        }

        public override Task Restart()
        {
            throw new NotImplementedException();
        }

        public bool Send(CanFrame canFrame)
        {
            throw new NotImplementedException();
        }

        public override Task Start()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
