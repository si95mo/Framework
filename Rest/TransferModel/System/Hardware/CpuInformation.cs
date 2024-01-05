using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Rest.TransferModel.System.Hardware
{
    public class CpuInformation
    {
        public string VendorId { get; private set; }
        public int CpuFamily { get; private set; }
        public int Model { get; private set; }
        public string ModelName { get; private set; }
        public int Stepping { get; private set; }
        public double Frequency { get; private set; }
        public string CacheSize { get; private set; }

        public CpuInformation()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                string[] info = File.ReadAllLines(@"/proc/cpuinfo");

                CpuInformationMatch[] cpuMatches =
                {
                    new CpuInformationMatch(@"^vendor_id\s+:\s+(.+)", (x) => VendorId = x),
                    new CpuInformationMatch(@"^cpu family\s+:\s+(.+)", (x) => { int.TryParse(x, out int y); CpuFamily = y; }),
                    new CpuInformationMatch(@"^model\s+:\s+(.+)", (x) => { int.TryParse(x, out int y); Model = y; }),
                    new CpuInformationMatch(@"^model name\s+:\s+(.+)", (x) => ModelName = x),
                    new CpuInformationMatch(@"^stepping\s+:\s+(.+)", (x) => { int.TryParse(x, out int y); Stepping = y; }),
                    new CpuInformationMatch(@"^cpu MHz\s+:\s+(.+)", (x) => { double.TryParse(x, out double y); Frequency = y; }),
                    new CpuInformationMatch(@"^cache size\s+:\s+(.+)", (x) => CacheSize = x)
                };

                foreach (string line in info)
                {
                    foreach (CpuInformationMatch cpuMatch in cpuMatches)
                    {
                        Match match = cpuMatch.Regex.Match(line);
                        if (match.Groups[0].Success)
                        {
                            string value = match.Groups[1].Value;
                            cpuMatch.Update(value);
                        }
                    }
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ManagementClass management = new ManagementClass("Win32_Processor");
                ManagementObjectCollection collection = management.GetInstances();
                foreach (ManagementObject obj in collection.OfType<ManagementObject>())
                {
                    VendorId = obj["Manufacturer"].ToString();
                    CpuFamily = int.Parse(obj["Family"].ToString());
                    Model = int.Parse(obj["Revision"]?.ToString() ?? "-1");
                    ModelName = obj["Name"].ToString();
                    Stepping = int.Parse(obj["Stepping"]?.ToString() ?? "-1");
                    Frequency = double.Parse(obj["MaxClockSpeed"].ToString());
                    CacheSize = (double.Parse(obj["L2CacheSize"].ToString()) + double.Parse(obj["L3CacheSize"].ToString())).ToString();
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                VendorId = "Apple";
                CpuFamily = -1;
                Model = -1;
                ModelName = "??";
                Stepping = -1;
                Frequency = -1;
                CacheSize = "??";
            }
        }
    }
}
