using System;

namespace Framework
{
    /// <summary>
    /// Provides the <b>default</b> paths used by the framework
    /// </summary>
    public static class Paths
    {
        /// <summary>
        /// The log folder
        /// </summary>
        public const string Logs = @"logs\";
        
        /// <summary>
        /// The error folder
        /// </summary>
        public const string Errors = Logs + @"errors\";

        /// <summary>
        /// The configuration folder
        /// </summary>
        public const string Configuration = @"config";

        /// <summary>
        /// The report folder
        /// </summary>
        public const string Reports = @"reports";

        /// <summary>
        /// The application folder
        /// </summary>
        public static string Application => AppDomain.CurrentDomain.BaseDirectory;
    }
}
