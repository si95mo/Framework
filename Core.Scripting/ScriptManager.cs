using Core.DataStructures;
using Diagnostic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Core.Scripting
{
    /// <summary>
    /// The script manager. Handles all the scripting logic
    /// </summary>
    public class ScriptManager
    {
        /// <summary>
        /// The name of the method to call at startup
        /// </summary>
        internal const string StartupMethodName = "Run";

        /// <summary>
        /// The name of the method to call at shutdown
        /// </summary>
        internal const string ShutdownMethodName = "Clear";

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
        /// Execute (i.e. call the Run method) all the script contained in the <see cref="ScriptManager"/>
        /// </summary>
        public static void Run()
        {
            if (!initialized)
                Logger.Error("Script manager not initialized, unable to execute the code inside the csx(s)");
            else
            {
                foreach (IScript script in scripts)
                    script.Run();
            }
        }

        /// <summary>
        /// Execute (i.e. call the Clear method) all the script contained in the <see cref="ScriptManager"/>
        /// </summary>
        public static void Clear()
        {
            if (!initialized)
                Logger.Error("Script manager not initialized, unable to execute the code inside the csx(s)");
            else
            {
            }
        }

        /// <summary>
        /// Compile the csx file specified by <paramref name="scriptPath"/>
        /// </summary>
        /// <param name="scriptPath">The csx file with the script to execute</param>
        /// <returns>The relative <see cref="Assembly"/></returns>
        private Assembly Compile(string scriptPath)
        {
            ScriptOptions options = ScriptOptions.Default;
            byte[] assemblyBinaryContent;

            string script = File.ReadAllText(scriptPath);

            Script<object> roslynScript = CSharpScript.Create(script, options);
            Compilation compilation = roslynScript.GetCompilation();

            compilation = compilation.WithOptions(compilation.Options.WithOptimizationLevel(OptimizationLevel.Release).WithOutputKind(OutputKind.DynamicallyLinkedLibrary));

            using (MemoryStream assemblyStream = new MemoryStream())
            {
                EmitResult result = compilation.Emit(assemblyStream);
                if (!result.Success)
                {
                    string errors = string.Join(Environment.NewLine, result.Diagnostics.Select((x) => x));
                    throw new Exception("Compilation errors: " + Environment.NewLine + errors);
                }

                assemblyBinaryContent = assemblyStream.ToArray();
            }

            GC.Collect(); // Allows to force clear compilation stuff.

            Assembly assembly = Assembly.Load(assemblyBinaryContent);
            return assembly;
        }

        private void ExecuteScript(string methodName)
        { }
    }
}