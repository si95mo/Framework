using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using SystemEnvironment = System.Environment;

namespace Rest.TransferModel.System.Environment
{
    /// <summary>
    /// Provide a transfer model for the environment information call
    /// </summary>
    public class EnvironmentInformation : Information
    {
        /// <summary>
        /// The OS description
        /// </summary>
        public string OperatingSystemDescription { get; set; } = RuntimeInformation.OSDescription;
        /// <summary>
        /// The <see cref="PlatformID"/>
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public PlatformID Platform { get; set; } = SystemEnvironment.OSVersion.Platform;
        /// <summary>
        /// The OS <see cref="Version"/>
        /// </summary>
        public Version OperatingSystemVersion { get; set; } = SystemEnvironment.OSVersion.Version;
        /// <summary>
        /// The OS processes bit (64 or 32)
        /// </summary>
        public int OsBits { get; set; } = SystemEnvironment.Is64BitProcess ? 64 : 32;
        /// <summary>
        /// The system directory
        /// </summary>
        public string SystemDirectory { get; set; } = SystemEnvironment.SystemDirectory;
        /// <summary>
        /// The processor count
        /// </summary>
        public int ProcessorCount { get; set; } = SystemEnvironment.ProcessorCount;
        /// <summary>
        /// The user domain name
        /// </summary>
        public string UserDomainName { get; set; } = SystemEnvironment.UserDomainName;
        /// <summary>
        /// The user name
        /// </summary>
        public string UserName { get; set; } = SystemEnvironment.UserName;
        /// <summary>
        /// The collection of <see cref="DriveInformation"/>
        /// </summary>
        public List<DriveInformation> DrivesInfo { get; set; } = DriveInfo.GetDrives().Select((x) => new DriveInformation(x)).ToList();
        /// <summary>
        /// The system page size, in B
        /// </summary>
        public int SystemPageSize { get; set; } = SystemEnvironment.SystemPageSize;
        /// <summary>
        /// The CLR <see cref="Version"/>
        /// </summary>
        public Version CommonLanguageRuntimeVersion { get; set; } = SystemEnvironment.Version;

        public EnvironmentInformation()
        { }
    }
}
