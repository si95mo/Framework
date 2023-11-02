namespace Core.Conditions
{
    /// <summary>
    /// Define a new <see cref="Condition"/> that will have its <see cref="Condition.Value"/> locked to the initial value
    /// </summary>
    public class ConstantCondition : Condition
    {
        private bool value;

        /// <summary>
        /// The <see cref="ICondition"/> value
        /// </summary>
        /// <remarks>Cannot be changed, will always have the same value used in <see cref="ConstantCondition(string, bool)"/></remarks>
        public override bool Value { get => value; set => _ = value; } // _ = value implies that this.value will be unchanged

        /// <summary>
        /// Create a new instance of <see cref="ConstantCondition"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="value">The initial (and only) value</param>
        public ConstantCondition(string code, bool value) : base(code)
        {
            Value = value;
        }
    }
}
