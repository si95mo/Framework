using System.Runtime.CompilerServices;

namespace Core.Conditions
{
    /// <summary>
    /// Define the basic prototype for a condition
    /// </summary>
    public interface ICondition : IProperty<bool>
    {
        /// <summary>
        /// The <see cref="ICondition"/> description
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Get the <see cref="TaskAwaiter"/> to use the <see langword="await"/> keyword. Will await until the <see cref="ICondition"/> will be true
        /// </summary>
        /// <returns>The <see cref="TaskAwaiter"/></returns>
        TaskAwaiter GetAwaiter();
    }
}