using Nancy;
using Nancy.Extensions;
using Nancy.Routing;
using Newtonsoft.Json;
using Rest.TransferModel.System.Server;
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
                    StringBuilder table = new StringBuilder();
                    StringBuilder routes = new StringBuilder();

                    if (Server != null)
                    {
                        //StringBuilder html = Helpers.CreateHtmlDocument();
                        //html = Helpers.AddTitle(html, "Available HTML modules");

                        IEnumerable<INancyModule> modules = Server.Bootstrapper.GetAllModules(new NancyContext());
                        if(modules.Any())
                        {
                            table.AddHorizontalLine(ModuleNameLength + MethodLength + RouteLength + 7);
                        }

                        foreach (INancyModule module in modules)
                        {
                            routes.Clear();

                            foreach (Route route in module.Routes)
                            {
                                routes.AppendLine(
                                    $"| {new string(Enumerable.Repeat(' ', ModuleNameLength).ToArray())} | " +
                                    $"{route.Description.Method, MethodLength}{route.Description.Path, RouteLength} |"
                                );
                            }

                            table.AppendLine($"| {module.GetModuleName(), ModuleNameLength} | {new string(Enumerable.Repeat(' ', MethodLength + RouteLength).ToArray())} |");
                            table.AppendLine(routes.ToString().TrimEnd(Environment.NewLine.ToCharArray()));
                            table.AddHorizontalLine(ModuleNameLength + MethodLength + RouteLength + 7);
                        }
                    }
                    else
                    {
                        table.AppendLine("| No REST server provided |");
                    }

                    return table.ToString();
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
