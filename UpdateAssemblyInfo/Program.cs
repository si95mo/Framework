using System;
using System.IO;
using System.Linq;

namespace UpdateAssemblyInfo
{
    /// <summary>
    /// Application main class
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Application entry-point
        /// </summary>
        /// <param name="args"></param>
        private static void Main()
        {
            string globalPath = @"..\..\..\";
            string assemblyInfoSubPath = @"\Properties\AssemblyInfo.cs";
            string[] directories = GetDirectories(globalPath);

            Console.WriteLine("Directories found:");
            foreach (string directory in directories)
                Console.WriteLine($"\t- {directory}");

            for(int i = 0; i < directories.Length; i++)
            {
                double percentage = (double)(i + 1) / directories.Length * 100d;
                Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} - Updating file number {i + 1} of {directories.Length} ({percentage:0.0}%)");

                string assemblyInfoPath = ConbinePath(globalPath, directories[i], assemblyInfoSubPath);
                string assemblyInfoText = GetAsseblyInfoText(assemblyInfoPath);

                UpdateAssemblyInfoFile(assemblyInfoText);
                SaveAssemblyInfo(assemblyInfoPath, assemblyInfoText);
            }

            Console.ReadLine();
        }

        /// <summary>
        /// Get the directories contained in a path
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>The directories found</returns>
        private static string[] GetDirectories(string path)
        {
            string[] allDirectories = Directory.GetDirectories(path);
            for (int i = 0; i < allDirectories.Length; i++)
                allDirectories[i] = new DirectoryInfo(allDirectories[i]).Name;

            Func<string, bool> condition = new Func<string, bool>((x) => !x.StartsWith(".") && !x.Contains("Documentation") && !x.Contains("Manual") && !x.Contains("packages"));
            string[] directories = allDirectories.Where((x) => condition(x)).Select(x => x).ToArray();

            return directories;
        }

        /// <summary>
        /// Get the assembly info text
        /// </summary>
        /// <param name="path">THe assembly info file path</param>
        /// <returns>The text retrieved</returns>
        private static string GetAsseblyInfoText(string path)
            => File.ReadAllText(path);

        /// <summary>
        /// Combine the sub paths
        /// </summary>
        /// <param name="globalpath">The global folder path</param>
        /// <param name="folder">The C# project folder path</param>
        /// <param name="subFolder">The AssemblyInfo.cs file path</param>
        /// <returns></returns>
        private static string ConbinePath(string globalpath, string folder, string subFolder)
            => globalpath + folder + subFolder;

        /// <summary>
        /// Update the assembly info file
        /// </summary>
        /// <param name="text">The file text</param>
        /// <param name="assemblyVersion">The assembly version</param>
        private static void UpdateAssemblyInfoFile(string text, string assemblyVersion = "\"0.1.0.0\"")
        {
            string year = DateTime.Now.Year.ToString();
            string companyName = "\"Meta s.r.l.\"";
            string copyright = $"\"Copyright © {companyName} {year}\"";

            string[] lines = text.Split(Environment.NewLine.ToCharArray());

            for(int i = 0; i < lines.Length; i++)
            {
                if(lines[i].Contains("AssemblyCompany"))
                    lines[i] = $"[assembly: AssemblyCompany({companyName})]";
                else
                {
                    if (lines[i].Contains("AssemblyCopyright"))
                        lines[i] = $"[assembly: AssemblyCopyright({copyright})]";
                    else
                    {
                        if (lines[i].Contains("AssemblyVersion"))
                            lines[i] = $"[assembly: AssemblyVersion({assemblyVersion})]";
                        else
                        {
                            if (lines[i].Contains("AssemblyFileVersion"))
                                lines[i] = $"[assembly: AssemblyFileVersion({assemblyVersion})]";
                        }
                    }
                }
            }
        }

        private static void SaveAssemblyInfo(string path, string text)
            => File.WriteAllText(path, text);
    }
}
