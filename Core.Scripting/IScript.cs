namespace Core.Scripting
{
    /// <summary>
    /// Describe a generic prototype for scripting
    /// </summary>
    public interface IScript : IProperty
    {
        /// <summary>
        /// The <see cref="IScript"/> to execute
        /// </summary>
        void Execute();
    }
}