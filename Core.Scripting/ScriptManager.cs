using Core.DataStructures;
using Diagnostic;
using System;
using System.Threading.Tasks;

namespace Core.Scripting
{
    /// <summary>
    /// The script manager. Handles all the scripting logic
    /// </summary>
    public class ScriptManager
    {
        private static Bag<IScript> scripts;
        private static bool initialized = false;

        /// <summary>
        /// Initialize the script manager
        /// </summary>
        public static void Initialize()
        {
            try
            {
                scripts = ServiceBroker.Get<IScript>();
                initialized = true;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        /// <summary>
        /// Execute all the script contained in the <see cref="ScriptManager"/>
        /// </summary>
        /// <returns>The (async) <see cref="Task"/> that will execute all the scripts</returns>
        public static async Task ExecuteScripts()
        {
            if (!initialized)
                Logger.Log(new Exception("Script manager not initialized!"));
            else
            {
                foreach (IScript script in scripts)
                    await script.Execute();
            }
        }
    }
}