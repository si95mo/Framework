namespace Core.Conditions
{
    /// <summary>
    /// Implement a flyweight condition (i.e. a basic type of condition)
    /// </summary>
    public class ConditionFlyweight : Condition
    {
        /// <summary>
        /// Create a new instance of <see cref="ConditionFlyweight"/>
        /// </summary>
        /// <param name="code">The code</param>
        public ConditionFlyweight(string code) : base(code)
        { }

        /// <summary>
        /// Create a new instance of <see cref="ConditionFlyweight"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="value">The initial value</param>
        public ConditionFlyweight(string code, bool value) : this(code)
        {
            Value = value;
        }
    }
}