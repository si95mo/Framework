using System.Reflection;

namespace Core.Scripting
{
    /// <summary>
    /// Describe a generic script prototype
    /// </summary>
    public interface IScript : IProperty
    {
        /// <summary>
        /// The <see cref="IScript"/> description
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// The ran status
        /// </summary>
        bool Ran { get; }

        /// <summary>
        /// The cleared status
        /// </summary>
        bool Cleared { get; }

        /// <summary>
        /// The method that will be called at startup
        /// </summary>
        void Run();

        /// <summary>
        /// The method that will be called at shutdown
        /// </summary>
        void Clear();

        /// <summary>
        /// Create a new instance of <see cref="IScript"/>
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/></param>
        /// <param name="code">The code</param>
        /// <param name="type">The name of the actual type </param>
        /// <returns>The created <see cref="IScript"/></returns>
        IScript New(Assembly assembly, string code, string type);
    }
}