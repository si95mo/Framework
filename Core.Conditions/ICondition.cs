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
    }
}