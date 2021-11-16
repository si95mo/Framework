using Diagnostic;
using Hardware;
using Nancy;
using Nancy.Hosting.Self;
using Nancy.Testing;
using System;
using System.Threading.Tasks;

namespace Rest
{
    /// <summary>
    /// Implement a REST server using <see cref="NancyHost"/>
    /// </summary>
    public class RestServer : Resource
    {
        private Uri uri;
        private NancyHost host;
        private HostConfiguration configuration;

        /// <summary>
        /// The <see cref="RestServer"/> <see cref="System.Uri"/>
        /// </summary>
        public Uri Uri => uri;

        public override bool IsOpen => Status.Value != ResourceStatus.Failure;

        /// <summary>
        /// Create a new instance of <see cref="RestServer"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="uri">The server <see cref="System.Uri"/></param>
        public RestServer(string code, Uri uri) : base(code)
        {
            configuration = new HostConfiguration();
            configuration.UrlReservations.CreateAutomatically = true;
            configuration.RewriteLocalhost = false;

            this.code = code;
            this.uri = uri;

            host = new NancyHost(uri, new DefaultNancyBootstrapper(), configuration);

            // Cannot await in the constructor!
            // Because this call is not awaited, execution of the current method continues before the call is completed
#pragma warning disable CS4014
            Start();
#pragma warning restore CS4014
        }

        /// <summary>
        /// Create a new instance of <see cref="RestServer"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="uri">The server <see cref="System.Uri"/></param>
        /// <param name="bootstrapper">The bootstrapper</param>
        public RestServer(string code, Uri uri, ConfigurableBootstrapper bootstrapper) : base(code)
        {
            configuration = new HostConfiguration();
            configuration.UrlReservations.CreateAutomatically = true;

            this.code = code;
            this.uri = uri;

            host = new NancyHost(uri, bootstrapper, configuration);
        }

        /// <summary>
        /// Start the <see cref="RestServer"/>
        /// </summary>
        public override async Task Start()
        {
            status.Value = ResourceStatus.Starting;

            await Task.Run(() => host.Start());
            Logger.Log($"{code} self-hosting on {uri}", Severity.Info);

            status.Value = ResourceStatus.Executing;
        }

        /// <summary>
        /// Stop the <see cref="RestServer"/>
        /// </summary>
        public override void Stop()
        {
            status.Value = ResourceStatus.Stopping;

            host.Stop();
            Logger.Log($"{code} stopped self-hosting on {uri}:{uri.Port}", Severity.Info);

            status.Value = ResourceStatus.Stopped;
        }

        public override async Task Restart()
        {
            Stop();
            await Start();
        }
    }
}