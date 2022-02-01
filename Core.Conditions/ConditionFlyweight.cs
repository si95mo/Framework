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

        public override ICondition IsTrue()
        {
            ConditionFlyweight result = new ConditionFlyweight($"{Code}.IsTrue", Value);
            ConnectTo(result);

            return result;
        }

        public override ICondition IsFalse()
        {
            ConditionFlyweight result = new ConditionFlyweight($"{Code}.IsFalse", Value == false);
            ValueChanged += (sender, e) => UpdateIsFalseCondition(result);

            return result;
        }

        /// <summary>
        /// Update a <see cref="ConditionFlyweight"/> to check if its value is <see langword="false"/>
        /// </summary>
        /// <param name="newCondition">The <see cref="ConditionFlyweight"/> to update</param>
        private void UpdateIsFalseCondition(ConditionFlyweight newCondition)
            => newCondition.Value = Value == false;

        /// <summary>
        /// Create a <see cref="ConditionFlyweight"/> that concatenates itself with another <see cref="ICondition"/>
        /// with an <see langword="and"/> relation
        /// </summary>
        /// <param name="condition">The other condition to which concatenate</param>
        /// <returns>The concatenated <see cref="ConditionFlyweight"/></returns>
        public ConditionFlyweight And(ICondition condition)
        {
            ConditionFlyweight andCondition = new ConditionFlyweight($"{Code}.And.{condition.Code}", Value & condition.Value);

            ValueChanged += (sender, e) => UpdateAndCondition(this, andCondition);
            condition.ValueChanged += (sender, e) => UpdateAndCondition(condition, andCondition);

            return andCondition;
        }

        /// <summary>
        /// Update a <see cref="ConditionFlyweight"/> by applying an
        /// <see langword="and"/> operand between two <see langword="bool"/> values
        /// </summary>
        /// <param name="changedCondition">The sender (the <see cref="ICondition"/> of which the value has changed)</param>
        /// <param name="andCondition">The <see cref="ConditionFlyweight"/> result of the <see cref="And(ICondition)"/> method</param>
        private void UpdateAndCondition(ICondition changedCondition, ConditionFlyweight andCondition)
            => andCondition.Value &= changedCondition.Value;

        /// <summary>
        /// Create a <see cref="ConditionFlyweight"/> that concatenates itself with another <see cref="ICondition"/>
        /// with an <see langword="or"/> relation
        /// </summary>
        /// <param name="condition">The other condition to which concatenate</param>
        /// <returns>The concatenated <see cref="ConditionFlyweight"/></returns>
        public ConditionFlyweight Or(ICondition condition)
        {
            ConditionFlyweight orCondition = new ConditionFlyweight($"{Code}.And.{condition.Code}", Value | condition.Value);

            ValueChanged += (sender, e) => UpdateOrCondition(this, orCondition);
            condition.ValueChanged += (sender, e) => UpdateOrCondition(condition, orCondition);

            return orCondition;
        }

        /// <summary>
        /// Update a <see cref="ConditionFlyweight"/> by applying an
        /// <see langword="or"/> operand between two <see langword="bool"/> values
        /// </summary>
        /// <param name="changedCondition">The sender (the <see cref="ICondition"/> of which the value has changed)</param>
        /// <param name="andCondition">The <see cref="ConditionFlyweight"/> result of the <see cref="And(ICondition)"/> method</param>
        private void UpdateOrCondition(ICondition changedCondition, ConditionFlyweight andCondition)
            => andCondition.Value |= changedCondition.Value;
    }
}