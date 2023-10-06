using Core.Conditions;
using Core.DataStructures;
using Diagnostic;
using Diagnostic.Messages;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
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
                // If the ServiceBroker can provide the service
                if (ServiceBroker.CanProvide<ScriptsService>())
                    scripts = ServiceBroker.GetService<ScriptsService>().Subscribers; // Get the scripts from there
                else
                    scripts = ServiceBroker.Get<IScript>(); // Otherwise get them in the misc collection

                initialized = true;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                initialized = false;
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
                {
                    script.Run();
                }
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
                foreach (IScript script in scripts)
                {
                    script.Clear();
                }
            }
        }

        /// <summary>
        /// Compile the csx file specified by <paramref name="scriptPath"/>
        /// </summary>
        /// <param name="scriptPath">The csx file with the script to execute</param>
        /// <returns>The relative <see cref="Assembly"/></returns>
        internal static Assembly Compile(string scriptPath)
        {
            ScriptOptions options = ScriptOptions.Default;
            byte[] assemblyBinaryContent;

            string script = File.ReadAllText(scriptPath);

            Script<object> roslynScript = CSharpScript.Create(script, options);
            Compilation compilation = roslynScript.GetCompilation();

            compilation = compilation.WithOptions(
                compilation.Options.WithOptimizationLevel(OptimizationLevel.Release).WithOutputKind(OutputKind.DynamicallyLinkedLibrary)
            );

            // Gathering all using directives in the compilation
            UsingDirectiveSyntax[] usings = compilation.SyntaxTrees.Select(
                tree => tree.GetRoot().ChildNodes().OfType<UsingDirectiveSyntax>()
            ).SelectMany(s => s).ToArray();

            // For each using directive add a MetadataReference to it
            List<MetadataReference> references = new List<MetadataReference>();
            string directory = Directory.GetCurrentDirectory();
            foreach (var u in usings)
                references.Add(MetadataReference.CreateFromFile(Path.Combine(directory, u.Name.ToString() + ".dll")));

            //add the reference list to the compilation
            compilation = compilation.AddReferences(references);

            using (MemoryStream assemblyStream = new MemoryStream())
            {
                EmitResult result = compilation.Emit(assemblyStream);
                if (!result.Success)
                {
                    DummyCondition condition = new DummyCondition(Guid.NewGuid().ToString());
                    Warn warn = Warn.New($"{nameof(ScriptManager)}.{nameof(Warn)}", "Error when compiling a script", condition.IsTrue(), sourceCode: null);

                    string errors = string.Join(Environment.NewLine, result.Diagnostics.Select((x) => x));
                    Logger.Error($"Compilation errors found:{Environment.NewLine}{errors}");

                    warn.Fire();
                    condition.Force(true); // Fire the alarm
                }

                assemblyBinaryContent = assemblyStream.ToArray();
            }

            GC.Collect(); // Allows to force clear compilation stuff.

            Assembly assembly = Assembly.Load(assemblyBinaryContent);
            return assembly;
        }
    }
}