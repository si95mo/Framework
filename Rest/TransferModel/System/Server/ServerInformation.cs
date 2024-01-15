using Hardware;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;

namespace Rest.TransferModel.System.Server
{
    public class ServerInformation : Information
    {
        /// <summary>
        /// The server code
        /// </summary>
        public string Code { get; set; } = string.Empty;
        /// <summary>
        /// The server <see cref="System.Uri"/>
        /// </summary>
        public Uri Uri { get; set; } = default;
        /// <summary>
        /// The server <see cref="ResourceStatus"/>
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public ResourceStatus Status { get; set; } = ResourceStatus.Failure;

        public ServerInformation()
        { }

        public ServerInformation(string code, Uri uri)
        {
            Code = code;
            Uri = uri;
        }

        public ServerInformation(RestServer server)
        {
            Code = server.Code;
            Uri = server.Uri;
            Status = server.Status.Value;
        }
    }
}
