using Core.DataStructures;
using Diagnostic;
using System;

namespace Core.Scripting
{
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

        public static void ExecuteScripts()
        {
            if (!initialized)
                Logger.Log(new Exception("Script manager not initialized!"));
            else
            {
                foreach (string code in scripts)
                    (scripts.Get(code) as IScript).Execute();
            }
        }
    }
}