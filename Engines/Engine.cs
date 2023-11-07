using System;

namespace Engines
{
    /// <summary>
    /// Define an abstract class that map <see cref="IEngine"/>
    /// </summary>
    public abstract class Engine : IEngine
    {
        public abstract bool ExecuteAsBool(string script);

        public abstract double ExecuteAsDouble(string script);

        public abstract string ExecuteAsString(string script);

        /// <summary>
        /// Define a generic method that execute a <paramref name="script"/>
        /// </summary>
        /// <typeparam name="T">The return type of the <paramref name="script"/></typeparam>
        /// <param name="script">The script to execute (without the <see langword="return"/> keyword)</param>
        /// <returns>The return value of the <paramref name="script"/></returns>
        protected abstract T Execute<T>(string script) where T : IConvertible;
    }
}