using System;
using System.IO;

namespace IO
{
    /// <summary>
    /// Provides the <b>default</b> paths used by the framework
    /// </summary>
    public static class Paths
    {
        /// <summary>
        /// The log folder
        /// </summary>
        public static readonly string Logs = @"logs" + Path.DirectorySeparatorChar;
        
        /// <summary>
        /// The error folder
        /// </summary>
        public static readonly string Errors = Path.Combine(Logs, "errors");

        /// <summary>
        /// The configuration folder
        /// </summary>
        public static readonly string Configuration = @"config" + Path.DirectorySeparatorChar;

        /// <summary>
        /// The report folder
        /// </summary>
        public static readonly string Reports = @"reports" + Path.DirectorySeparatorChar;

        /// <summary>
        /// The script folder
        /// </summary>
        public static readonly string Scripts = @"scripts" + Path.DirectorySeparatorChar;

        /// <summary>
        /// The application folder
        /// </summary>
        public static string Application => AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// The users folder
        /// </summary>
        public static string Users => "users" + Path.DirectorySeparatorChar;
    }
}
