namespace Engines
{
    /// <summary>
    /// Define a generic interface for a script engine
    /// </summary>
    public interface IEngine
    {
        /// <summary>
        /// Execute a <paramref name="script"/> with a <see cref="bool"/> return value
        /// </summary>
        /// <param name="script">The script to execute (without the <see langword="return"/> keyword)</param>
        /// <returns>The return value of the <paramref name="script"/></returns>
        bool ExecuteAsBool(string script);

        /// <summary>
        /// Execute a <paramref name="script"/> with a <see cref="double"/> return value
        /// </summary>
        /// <param name="script">The script to execute (without the <see langword="return"/> keyword)</param>
        /// <returns>The return value of the <paramref name="script"/></returns>
        double ExecuteAsDouble(string script);

        /// <summary>
        /// Execute a <paramref name="script"/> with a <see cref="string"/> return value
        /// </summary>
        /// <param name="script">The script to execute (without the <see langword="return"/> keyword)</param>
        /// <returns>The return value of the <paramref name="script"/></returns>
        string ExecuteAsString(string script);
    }
}