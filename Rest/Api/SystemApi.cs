using Nancy;
using Newtonsoft.Json;
using Rest.TransferModel.System;
using Rest.TransferModel.System.Environment;
using Rest.TransferModel.System.Hardware;
using System;

namespace Rest.Api
{
    /// <summary>
    /// Define basic REST call to retrieve the system information
    /// </summary>
    public class SystemApi : NancyModule
    {
        public SystemApi()
        {
            Get("system/environment", args =>
            {
                EnvironmentInformation environmentInfo = new EnvironmentInformation();
                string json = JsonConvert.SerializeObject(environmentInfo, Formatting.Indented);

                return json;
            }
            );

            Get("system/hardware/cpu", args =>
            {
                CpuInformation cpuInfo = new CpuInformation();
                string json = JsonConvert.SerializeObject(cpuInfo, Formatting.Indented);

                return json;
            }
            );

            Get("system/timestamp", args =>
            {
                TimestampInformation timestamp = new TimestampInformation(DateTime.Now);
                string json = JsonConvert.SerializeObject(timestamp, Formatting.Indented);

                return json;
            }
            );
        }
    }
}
