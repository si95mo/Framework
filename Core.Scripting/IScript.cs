using System.Threading.Tasks;

namespace Core.Scripting
{
    /// <summary>
    /// Describe a generic script prototype
    /// </summary>
    public interface IScript : IProperty
    {
        /// <summary>
        /// The method that will be called at startup
        /// </summary>
        void Run();

        /// <summary>
        /// The method that will be called at shutdown
        /// </summary>
        void Clear();
    }
}