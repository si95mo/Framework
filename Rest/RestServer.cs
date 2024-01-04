using Diagnostic;
using Hardware;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Hosting.Self;
using Nancy.Testing;
using Nancy.TinyIoc;
using Rest.Api;
using System;
using System.Net;
using System.Net.Sockets;
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
        internal NancyBootstrapperWithRequestContainerBase<TinyIoCContainer> Bootstrapper;

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
            Bootstrapper = new DefaultNancyBootstrapper();

            this.uri = uri;

            host = new NancyHost(uri, Bootstrapper, configuration);

            InfoApi.Server = this;
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
            Bootstrapper = bootstrapper;

            this.uri = uri;

            host = new NancyHost(uri, bootstrapper, configuration);

            InfoApi.Server = this;
        }

        /// <summary>
        /// Create a new instance of <see cref="RestServer"/> with the machine local ip address
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="port">The port number</param>
        public RestServer(string code, int port) : this(code, new Uri($"{GetLocalIpAddress()}:{port}"))
        { }

        /// <summary>
        /// Create a new instance of <see cref="RestServer"/> with the machine local ip address
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="port">The port number</param>
        /// <param name="bootstrapper">The bootstrapper</param>
        public RestServer(string code, int port, ConfigurableBootstrapper bootstrapper) : this(code, new Uri($"http://{GetLocalIpAddress()}:{port}"), bootstrapper)
        { }

        /// <summary>
        /// Start the <see cref="RestServer"/>
        /// </summary>
        public override Task Start()
        {
            Status.Value = ResourceStatus.Starting;

            try
            {
                host.Start();
                Logger.Log($"{Code} self-hosting on {uri}", Severity.Info);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                Logger.Warn($"Attempting to restart {Code} on localhost");

                uri = new Uri($"http://localhost:{uri.Port}");
                host = new NancyHost(uri, Bootstrapper, configuration);

                host.Start();
            }

            Status.Value = ResourceStatus.Executing;

            return Task.CompletedTask;
        }

        /// <summary>
        /// Stop the <see cref="RestServer"/>
        /// </summary>
        public override void Stop()
        {
            Status.Value = ResourceStatus.Stopping;

            host.Stop();
            Logger.Log($"{Code} stopped self-hosting on {uri}:{uri.Port}", Severity.Info);

            Status.Value = ResourceStatus.Stopped;
        }

        public override async Task Restart()
        {
            Stop();
            await Start();
        }

        /// <summary>
        /// Get the machine local ip address
        /// </summary>
        /// <returns>The local ip address</returns>
        private static string GetLocalIpAddress()
        {
            string ipAddress;

            try
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;

                    ipAddress = endPoint.Address.ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                Logger.Warn("Unable to find local machine IP address. Using localhost instead (127.0.0.1)");

                ipAddress = "127.0.0.1";
            }

            return ipAddress;
        }
    }
}