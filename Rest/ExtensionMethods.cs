using Nancy;
using Rest.Api;
using System.Linq;
using System.Text;
using static Nancy.Testing.ConfigurableBootstrapper;

namespace Rest
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Add a new module to <paramref name="source"/>
        /// </summary>
        /// <typeparam name="T">The type of the module to add</typeparam>
        /// <param name="source">The <see cref="ConfigurableBootstrapperConfigurator"/> of the <see cref="RestServer"/></param>
        public static void AddModule<T>(this ConfigurableBootstrapperConfigurator source) where T : NancyModule
        {
            source.Module<T>();
        }

        /// <summary>
        /// Add the <see cref="InfoApi"/> module to <paramref name="source"/>
        /// </summary>
        /// <remarks>
        /// This should be the last module added
        /// </remarks>
        /// <param name="source">The <see cref="ConfigurableBootstrapperConfigurator"/> of the <see cref="RestServer"/></param>
        public static void AddInfo(this ConfigurableBootstrapperConfigurator source)
            => source.AddModule<InfoApi>();

        /// <summary>
        /// Add an horizontal line to a <see cref="StringBuilder"/>
        /// </summary>
        /// <param name="source">The <see cref="StringBuilder"/></param>
        /// <param name="length">The length of the line</param>
        internal static void AddHorizontalLine(this StringBuilder source, int length)
        {
            source.AppendLine(new string(Enumerable.Repeat('-', length).ToArray())); // Plus white spaces and pipes
        }
    }
}
