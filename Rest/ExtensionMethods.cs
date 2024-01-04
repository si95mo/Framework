using Nancy;
using Rest.Api;
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
    }
}
