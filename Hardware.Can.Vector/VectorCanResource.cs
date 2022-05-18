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
        XL_HWTYPE_NONE = 0,
        XL_HWTYPE_VIRTUAL = 1,
        XL_HWTYPE_CANCARDX = 2,
        XL_HWTYPE_CANAC2PCI = 6,
        XL_HWTYPE_CANCARDY = 12,
        XL_HWTYPE_CANCARDXL = 15,
        XL_HWTYPE_CANCASEXL = 21,
        XL_HWTYPE_CANCASEXL_LOG_OBSOLETE = 23,
        XL_HWTYPE_CANBOARDXL = 25,
        XL_HWTYPE_CANBOARDXL_PXI = 27,
        XL_HWTYPE_VN2600 = 29,
        XL_HWTYPE_VN2610 = 29,
        XL_HWTYPE_VN3300 = 37,
        XL_HWTYPE_VN3600 = 39,
        XL_HWTYPE_VN7600 = 41,
        XL_HWTYPE_CANCARDXLE = 43,
        XL_HWTYPE_VN8900 = 45,
        XL_HWTYPE_VN8950 = 47,
        XL_HWTYPE_VN2640 = 53,
        XL_HWTYPE_VN1610 = 55,
        XL_HWTYPE_VN1630 = 57,
        XL_HWTYPE_VN1640 = 59,
        XL_HWTYPE_VN8970 = 61,
        XL_HWTYPE_VN1611 = 63,
        XL_HWTYPE_VN5610 = 65,
        XL_HWTYPE_VN5620 = 66,
        XL_HWTYPE_VN7570 = 67,
        XL_HWTYPE_IPCLIENT = 69,
        XL_HWTYPE_IPSERVER = 71,
        XL_HWTYPE_VX1121 = 73,
        XL_HWTYPE_VX1131 = 75,
        XL_HWTYPE_VT6204 = 77,
        XL_HWTYPE_VN1630_LOG = 79,
        XL_HWTYPE_VN7610 = 81,
        XL_HWTYPE_VN7572 = 83,
        XL_HWTYPE_VN8972 = 85,
        XL_HWTYPE_VN0601 = 87,
        XL_HWTYPE_VN5640 = 89,
        XL_HWTYPE_VX0312 = 91,
        XL_HWTYPE_VH6501 = 94,
        XL_HWTYPE_VN8800 = 95,
        XL_HWTYPE_IPCL8800 = 96,
        XL_HWTYPE_IPSRV8800 = 97,
        XL_HWTYPE_CSMCAN = 98,
        XL_HWTYPE_VN5610A = 101,
        XL_HWTYPE_VN7640 = 102,
        XL_HWTYPE_VX1135 = 104,
        XL_HWTYPE_VN4610 = 105,
        XL_HWTYPE_VT6306 = 107,
        XL_HWTYPE_VT6104A = 108,
        XL_HWTYPE_VN5430 = 109,
        XL_HWTYPE_VN1530 = 112,
        XL_HWTYPE_VN1531 = 113,
        XL_MAX_HWTYPE = 113
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
            Status.Value = ResourceStatus.Starting;

            driver.XL_CloseDriver(); // Close already started sessions

            return Task.CompletedTask;
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }
    }
}