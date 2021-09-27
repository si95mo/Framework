using Core.Parameters;
using Diagnostic;
using Hardware;
using Nancy;
using Nancy.Hosting.Self;
using System;

namespace Rest
{
    /// <summary>
    /// Implement a REST server using <see cref="Nancy"/>
    /// </summary>
    public class RestServer
    {
        private string code;
        private Uri uri;
        private NancyHost host;
        private HostConfiguration configuration;

        private EnumParameter<ResourceStatus> status;

        /// <summary>
        /// The <see cref="RestServer"/> code
        /// </summary>
        public string Code => code;

        /// <summary>
        /// The <see cref="RestServer"/> <see cref="System.Uri"/>
        /// </summary>
        public Uri Uri => uri;

        /// <summary>
        /// The <see cref="RestServer"/> status
        /// </summary>
        public EnumParameter<ResourceStatus> Status => status;

        /// <summary>
        /// Create a new instance of <see cref="RestServer"/>
        /// </summary>
        /// <param name="uri">The server <see cref="System.Uri"/></param>
        public RestServer(string code, Uri uri)
        {
            configuration = new HostConfiguration();
            configuration.UrlReservations.CreateAutomatically = true;

            this.code = code;
            this.uri = uri;

            host = new NancyHost(uri, new DefaultNancyBootstrapper(), configuration);
            host.Start();
            Logger.Log($"{code} self-hosting on {uri}", Severity.Info);

            status.Value = ResourceStatus.Executing;
        }
    }
}
