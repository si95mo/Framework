using Nancy;
using Nancy.Extensions;
using Nancy.Routing;
using Newtonsoft.Json;
using Rest.TransferModel.Info;
using Rest.TransferModel.System.Server;
using Rest.TransferModel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rest.Api
{
    /// <summary>
    /// Provides REST call to retrieve common informations about the <see cref="RestServer"/>
    /// </summary>
    public class InfoApi : NancyModule
    {
        private const int ModuleNameLength = 32;
        private const int MethodLength = 16;
        private const int RouteLength = 64 - MethodLength;

        public static RestServer Server { get; set; }

        /// <summary>
        /// Define a basic info API
        /// </summary>
        /// <remarks>
        /// This should be the last module added
        /// </remarks>
        public InfoApi()
        {
            Get("info/modules", args =>
                {
                    if (Server != null)
                    {
                        StringBuilder htmlBuilder = Helpers.CreateHtmlDocument();
                        htmlBuilder = Helpers.AddTitle(htmlBuilder, "Available HTML modules");

                        IEnumerable<INancyModule> modules = Server.Bootstrapper.GetAllModules(new NancyContext());
                        IEnumerable<ModuleInformation> modulesInformation = modules.Select((x) => new ModuleInformation(x.GetModuleName(), default));
                        IEnumerable<IEnumerable<RouteInformation>> routesInformation = modules
                            .Select((x) => x.Routes)
                            .Select((x) => x.Select((y) => new RouteInformation(y)));

                        htmlBuilder = Helpers.AddTable(htmlBuilder, modulesInformation, routesInformation);
                        string html = Helpers.CloseHtmlDocument(htmlBuilder);

                        return html;
                    }
                    else
                    {
                        string message = "No REST server provided";
                        ErrorInformation errorInformation = new ErrorInformation(message, new ArgumentException(message, "info/modules"));

                        string json = errorInformation.Serialize();
                        return json;
                    }
                }
            );

            Get("info/version/assembly", args =>
                {
                    Version version = typeof(RestServer).Assembly.GetName().Version;
                    string json = JsonConvert.SerializeObject(version, Formatting.Indented);

                    return json;
                }
            );

            Get("info/version/application", args =>
                {
                    Version version = Assembly.GetExecutingAssembly().GetName().Version;
                    string json = JsonConvert.SerializeObject(version, Formatting.Indented);

                    return json;
                }
            );

            Get("info/server", args =>
                {
                    ServerInformation info = new ServerInformation(Server);
                    string json = JsonConvert.SerializeObject(info, Formatting.Indented);

                    return json;
                }
            );
        }
    }
}
