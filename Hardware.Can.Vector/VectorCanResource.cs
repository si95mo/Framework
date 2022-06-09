using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vxlapi_NET;

namespace Hardware.Can.Vector
{
    /// <summary>
    /// Define all the available hardware type for a <see cref="VectorCanResource"/>
    /// </summary>
    public enum HardwareType : byte
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        None = 0,
        Virtual = 1,
        CanCardX = 2,
        CanAC2Pci = 6,
        CanCardY = 12,
        CanCardXl = 15,
        CanCaseXl = 21,
        CanCaseXlLogObsolete = 23,
        CanBoardXl = 25,
        CanBoardXlPxi = 27,
        Vn2600 = 29,
        Vn2610 = 29,
        Vn3300 = 37,
        Vn3600 = 39,
        Vn7600 = 41,
        CanCardXle = 43,
        Vn8900 = 45,
        Vn8950 = 47,
        Vn2640 = 53,
        Vn1610 = 55,
        Vn1630 = 57,
        Vn1640 = 59,
        Vn8970 = 61,
        Vn1611 = 63,
        Vn5610 = 65,
        Vn5620 = 66,
        Vn7570 = 67,
        IpClient = 69,
        IpServer = 71,
        Vx1121 = 73,
        Vx1131 = 75,
        Vt6204 = 77,
        Vn1630Log = 79,
        Vn7610 = 81,
        Vn7572 = 83,
        Vn8972 = 85,
        Vn0601 = 87,
        Vn5640 = 89,
        Vz0312 = 91,
        Vh6501 = 94,
        Vn8800 = 95,
        IpCl8800 = 96,
        IpSrv8800 = 97,
        CsmCan = 98,
        Vn5610A = 101,
        Vn7640 = 102,
        Vx1135 = 104,
        Vn4610 = 105,
        Vt6306 = 107,
        Vt6104A = 108,
        Vn5430 = 109,
        Vn1530 = 112,
        Vn1531 = 113,
        MaxHwType = 113
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    /// <summary>
    /// Implement a <see cref="Resource"/> for the CAN protocol that communicates using Vector hardware
    /// </summary>
    public class VectorCanResource : Resource, ICanResource
    {
        public Dictionary<int, bool> FilteredCanId { get; private set; }

        public override bool IsOpen => Status.Value == ResourceStatus.Executing || Status.Value == ResourceStatus.Stopped;

        private XLDriver driver;
        private string applicationName;
        private uint applicationChannel, hardwareIndex, hardwareChannel;
        private HardwareType hardwareType;

        private bool logEnabled;
        private int maxCapacity;
        private Queue<CanFrame> logQueue;
        private object logLock = new object();

        /// <summary>
        /// Create a new instance of <see cref="VectorCanResource"/>
        /// </summary>
        /// <param name="code">The code</param>
        public VectorCanResource(string code) : base(code)
        {
            FilteredCanId = new Dictionary<int, bool>();
            driver = new XLDriver();
        }

        /// <summary>
        /// Create a new instance of <see cref="VectorCanResource"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="applicationName">The application name</param>
        /// <param name="applicationChannel">The application channel</param>
        /// <param name="hardwareType">The <see cref="HardwareType"/></param>
        /// <param name="hardwareIndex">The hardware index</param>
        /// <param name="hardwareChannel">The hardware channel</param>
        public VectorCanResource(string code, string applicationName, uint applicationChannel, HardwareType hardwareType,
            uint hardwareIndex, uint hardwareChannel) : this(code)
        {
            this.applicationChannel = applicationChannel;
            this.hardwareType = hardwareType;
            this.hardwareIndex = hardwareIndex;
            this.hardwareChannel = hardwareChannel;

            driver = new XLDriver();
        }

        public void DisableLog() => logEnabled = false;

        public void EnableLog(int maxLogSize = 65535)
        {
            logEnabled = true;
            maxCapacity = maxLogSize;
            logQueue = new Queue<CanFrame>(maxLogSize);
        }

        public string ReadLog()
        {
            string log = "";

            // EnableLog(int) should have been called at least once
            if (logQueue != null)
            {
                lock (logLock)
                {
                    log = string.Join(Environment.NewLine, logQueue);
                    logQueue.Clear();
                }
            }

            return log;
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
            Status.Value = ResourceStatus.Starting;

            driver.XL_CloseDriver(); // Close already started sessions
            driver.XL_OpenDriver();

            return Task.CompletedTask;
        }

        public override void Stop()
        {
            Status.Value = ResourceStatus.Stopping;
            driver.XL_CloseDriver();
            Status.Value = ResourceStatus.Stopped;
        }
    }
}