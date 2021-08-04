namespace Core.Scripting
{
    public interface IScript : IProperty
    {
        /// <summary>
        /// The <see cref="IScript"/> to execute
        /// </summary>
        void Execute();
    }
}