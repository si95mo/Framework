namespace Core.Conditions
{
    /// <summary>
    /// Create a <see cref="Condition"/> that will retain its value since its not connected
    /// to anything external
    /// </summary>
    internal class DummyCondition : Condition
    {
        /// <summary>
        /// Create a new instance of <see cref="DummyCondition"/>
        /// </summary>
        /// <param name="code">The code</param>
        public DummyCondition(string code) : base(code)
        { }

        /// <summary>
        /// Create a new instance of <see cref="DummyCondition"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="value">The initial value</param>
        public DummyCondition(string code, bool value) : this(code)
        {
            Value = value;
        }

        /// <summary>
        /// Force the <see cref="DummyCondition"/> value to change
        /// </summary>
        /// <remarks>This is the only way to change the <see cref="DummyCondition"/> value!</remarks>
        /// <param name="value">The new value</param>
        public void Force(bool value) => Value = value;
    }
}