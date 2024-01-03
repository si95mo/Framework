using Configurations;
using Core.DataStructures;
using Diagnostic;
using System.IO;
using System.Reflection;

namespace Core.Scripting
{
    /// <summary>
    /// Define a <see cref="Service{T}"/> for <see cref="IScript"/>
    /// </summary>
    public class ScriptsService : Service<IScript>
    {
        private const string CsxFileName = "startup.json";

        /// <summary>
        /// Create a new instance of <see cref="ScriptsService"/>
        /// </summary>
        /// <param name="path">The startup catalog path, leave <see langword="null"/> for the default one</param>
        public ScriptsService(string path = null) : base()
        {
            path = path ?? Path.Combine(IO.Paths.Scripts, CsxFileName);
            ReadStartup(path);
        }

        /// <summary>
        /// Create a new instance of <see cref="ScriptsService"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="path">The startup catalog path, leave <see langword="null"/> for the default one</param>
        public ScriptsService(string code, string path = null) : base(code)
        {
            path = path ?? Path.Combine(IO.Paths.Scripts, CsxFileName);
            ReadStartup(path);
        }

        /// <summary>
        /// Read the startup configuration file and parse all the found <see cref="Script"/>
        /// </summary>
        /// <param name="path">The startup catalog pat</param>
        private void ReadStartup(string path)
        {
            if (File.Exists(path))
            {
                if (path != null)
                {
                    path = Path.Combine(path, CsxFileName);
                    Configuration configuration = new Configuration(fileName: path);

                    string csxPath;
                    foreach (ConfigurationItem item in configuration.Items.Values)
                    {
                        csxPath = Path.Combine(Path.GetDirectoryName(path), item.Name);
                        Assembly assembly = ScriptManager.Compile(csxPath);

                        string typeName = Path.GetFileNameWithoutExtension(item.Name);
                        IScript script = Script.NewInstance(assembly, item.Name, typeName);
                        script.Description = item.Value.Description;

                        Add(script);
                    }
                }
                else
                {
                    Logger.Warn("No path provided for the scripts location");
                }
            }
            else
            {
                Logger.Error($"Scripts catalog file @ '{path}' not found");
            }
        }
    }
}