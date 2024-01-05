using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;

namespace Rest.TransferModel.System.Environment
{
    /// <summary>
    /// Provide a transfer model for the drive information call
    /// </summary>
    public class DriveInformation
    {
        /// <summary>
        /// The drive name
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// The volume label
        /// </summary>
        public string VolumeLabel { get; set; } = string.Empty;
        /// <summary>
        /// The <see cref="System.IO.DriveType"/>
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public DriveType DriveType { get; set; }
        /// <summary>
        /// The drive format
        /// </summary>
        public string DriveFormat { get; set; } = string.Empty;
        /// <summary>
        /// The total size of the drive, in GB
        /// </summary>
        public double TotalSize { get; set; } = double.MinValue;
        /// <summary>
        /// The available free space of the drive, in GB
        /// </summary>
        public double AvailableFreeSpace { get; set; } = double.MinValue;

        public DriveInformation()
        { }

        public DriveInformation(DriveInfo driveInfo)
        {
            Name = driveInfo.Name;
            VolumeLabel = driveInfo.VolumeLabel;
            DriveType = driveInfo.DriveType;
            DriveFormat = driveInfo.DriveFormat;
            TotalSize = driveInfo.TotalSize / Math.Pow(1024d, 3d);
            AvailableFreeSpace = driveInfo.AvailableFreeSpace / Math.Pow(1024d, 3d);
        }
    }
}
