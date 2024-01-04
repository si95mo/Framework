using Nancy;
using Nancy.Extensions;
using Nancy.Routing;
using System.Linq;
using System.Text;

namespace Rest.Api
{
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
                        foreach (INancyModule module in Server.Bootstrapper.GetAllModules(new NancyContext()))
                        {
                            routes.Clear();

                            foreach(Route route in module.Routes)
                            {
                                routes.AppendLine(
                                    $"| {new string(Enumerable.Repeat(' ', ModuleNameLength).ToArray()) } | " +
                                    $"{route.Description.Method,MethodLength}{route.Description.Path,RouteLength} |"
                                );
                            }

                            table.AppendLine($"| {module.GetModuleName(), ModuleNameLength} | {new string(Enumerable.Repeat(' ', MethodLength + RouteLength).ToArray())} |");
                            table.AppendLine(routes.ToString());
                            table.AppendLine(new string(Enumerable.Repeat(' ', ModuleNameLength + MethodLength + RouteLength + 4).ToArray())); // 4 additional white spaces
                        }
                    }
                    else
                    {
                        table.AppendLine("| No REST server provided |");
                    }

                    return table.ToString();
                }
            );
        }
    }
}
