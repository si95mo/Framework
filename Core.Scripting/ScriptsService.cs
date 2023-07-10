using Configurations;
using Core.DataStructures;
using System.IO;
using System.Reflection;

namespace Core.Scripting
{
    /// <summary>
    /// Define a <see cref="Service{T}"/> for <see cref="IScript"/>
    /// </summary>
    public class ScriptsService : Service<IScript>
    {
        private const string ClassKeyword = "class ";
        private const string CsxFileName = "startup.json";

        /// <summary>
        /// Create a new instance of <see cref="ScriptsService"/>
        /// </summary>
        /// <param name="path">The startup catalog path</param>
        public ScriptsService(string path) : base()
        {
            ReadStartup(path);
        }

        /// <summary>
        /// Create a new instance of <see cref="ScriptsService"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="path">The startup catalog path</param>
        public ScriptsService(string code, string path) : base(code)
        {
            ReadStartup(path);
        }

        /// <summary>
        /// Read the startup configuration file and parse all the found <see cref="Script"/>
        /// </summary>
        /// <param name="configPath">The configuration file path</param>
        private void ReadStartup(string configPath)
        {
            configPath = Path.Combine(configPath, CsxFileName);
            Configuration configuration = new Configuration(path: configPath);

            string csxPath;
            foreach (ConfigurationItem item in configuration.Items.Values)
            {
                csxPath = Path.Combine(Path.GetDirectoryName(configPath), item.Name);
                Assembly assembly = ScriptManager.Compile(csxPath);

                string typeName = Path.GetFileNameWithoutExtension(item.Name);
                IScript script = Script.NewInstance(assembly, item.Name, typeName);
                Add(script);
            }
        }
    }
}