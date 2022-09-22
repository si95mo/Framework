using Core;
using Core.Conditions;

namespace Extensions
{
    /// <summary>
    /// Provides <see cref="ICondition"/>-related extension methods
    /// </summary>
    public static class ConditionExtensions
    {
        /// <summary>
        /// Add a description to an <see cref="ICondition"/> (in <see cref="ICondition.Description"/>
        /// </summary>
        /// <param name="condition">The source <see cref="ICondition"/></param>
        /// <param name="description">The description</param>
        public static void WithDescription(this ICondition condition, string description)
            => condition.Description = description;

        /// <summary>
        /// Create a new <see cref="ICondition"/> that is <see langword="true"/> when <paramref name="source"/> is <see langword="true"/>
        /// </summary>
        /// <param name="source">The source <see cref="ICondition"/></param>
        /// <returns>The new <see cref="ICondition"/></returns>
        public static ICondition IsTrue(this ICondition source)
        {
            FlyweightCondition result = new FlyweightCondition($"{source.Code}.IsTrue", source.Value);
            source.ConnectTo(result);

            return result;
        }

        /// <summary>
        /// Create a new <see cref="ICondition"/> that is <see langword="true"/> when <paramref name="source"/> is <see langword="false"/>
        /// </summary>
        /// <param name="source">The source <see cref="ICondition"/></param>
        /// <returns>The new <see cref="ICondition"/></returns>
        public static ICondition IsFalse(this ICondition source)
        {
            FlyweightCondition result = new FlyweightCondition($"{source.Code}.IsFalse", source.Value == false);
            source.ValueChanged += (sender, e) => UpdateIsFalseCondition(source, result);

            return result;
        }

        /// <summary>
        /// Create a new <see cref="ICondition"/> that is <see langword="true"/> when <paramref name="source"/> is <see langword="true"/>
        /// </summary>
        /// <param name="source">The source <see cref="IProperty{T}"/></param>
        /// <returns>The new <see cref="ICondition"/></returns>
        public static ICondition IsTrue(this IProperty<bool> source)
        {
            FlyweightCondition result = new FlyweightCondition($"{source.Code}.IsTrue", source.Value);
            source.ConnectTo(result);

            return result;
        }

        /// <summary>
        /// Create a new <see cref="ICondition"/> that is <see langword="true"/> when <paramref name="source"/> is <see langword="false"/>
        /// </summary>
        /// <param name="source">The source <see cref="IProperty{T}"/></param>
        /// <returns>The new <see cref="ICondition"/></returns>
        public static ICondition IsFalse(this IProperty<bool> source)
        {
            FlyweightCondition result = new FlyweightCondition($"{source.Code}.IsFalse", source.Value == false);
            source.ValueChanged += (sender, e) => UpdateIfFalseCondition(source, result);

            return result;
        }

        /// <summary>
        /// Update a <see cref="FlyweightCondition"/> to check if its value is <see langword="false"/>
        /// </summary>
        /// <param name="source">The source <see cref="ICondition"/></param>
        /// <param name="newCondition">The <see cref="FlyweightCondition"/> to update</param>
        private static void UpdateIsFalseCondition(ICondition source, FlyweightCondition newCondition)
            => newCondition.Value = source.Value == false;

        /// <summary>
        /// Update a <see cref="FlyweightCondition"/> to check if its value is <see langword="false"/>
        /// </summary>
        /// <param name="source">The source <see cref="IProperty{T}"/></param>
        /// <param name="newCondition">The <see cref="FlyweightCondition"/> to update</param>
        private static void UpdateIfFalseCondition(IProperty<bool> source, FlyweightCondition newCondition)
            => newCondition.Value = source.Value == false;

        /// <summary>
        /// Create a <see cref="FlyweightCondition"/> that concatenates itself with another <see cref="ICondition"/> with an <see langword="and"/> relation
        /// </summary>
        /// <param name="source">The source <see cref="ICondition"/></param>
        /// <param name="condition">The other condition to which concatenate</param>
        /// <returns>The concatenated <see cref="FlyweightCondition"/></returns>
        public static FlyweightCondition And(this ICondition source, ICondition condition)
        {
            FlyweightCondition andCondition = new FlyweightCondition($"{source.Code}.And.{condition.Code}", source.Value & condition.Value);

            source.ValueChanged += (sender, e) => UpdateAndCondition(source, andCondition);
            condition.ValueChanged += (sender, e) => UpdateAndCondition(condition, andCondition);

            return andCondition;
        }

        /// <summary>
        /// Update a <see cref="FlyweightCondition"/> by applying an <see langword="and"/> operand between two <see langword="bool"/> values
        /// </summary>
        /// <param name="changedCondition">The sender (the <see cref="ICondition"/> of which the value has changed)</param>
        /// <param name="andCondition">The <see cref="FlyweightCondition"/> result of the <see cref="And(ICondition, ICondition)"/> method</param>
        private static void UpdateAndCondition(ICondition changedCondition, FlyweightCondition andCondition)
            => andCondition.Value &= changedCondition.Value;

        /// <summary>
        /// Create a <see cref="FlyweightCondition"/> that concatenates itself with another <see cref="ICondition"/> with an <see langword="or"/> relation
        /// </summary>
        /// <param name="source">The source <see cref="ICondition"/></param>
        /// <param name="condition">The other condition to which concatenate</param>
        /// <returns>The concatenated <see cref="FlyweightCondition"/></returns>
        public static FlyweightCondition Or(this ICondition source, ICondition condition)
        {
            FlyweightCondition orCondition = new FlyweightCondition($"{source.Code}.And.{condition.Code}", source.Value | condition.Value);

            source.ValueChanged += (sender, e) => UpdateOrCondition(source, orCondition);
            condition.ValueChanged += (sender, e) => UpdateOrCondition(condition, orCondition);

            return orCondition;
        }

        /// <summary>
        /// Update a <see cref="FlyweightCondition"/> by applying an <see langword="or"/> operand between two <see langword="bool"/> values
        /// </summary>
        /// <param name="changedCondition">The sender (the <see cref="ICondition"/> of which the value has changed)</param>
        /// <param name="orCondition">The <see cref="FlyweightCondition"/> result of the <see cref="Or(ICondition, ICondition)"/> method</param>
        private static void UpdateOrCondition(ICondition changedCondition, FlyweightCondition orCondition)
            => orCondition.Value |= changedCondition.Value;

        /// <summary>
        /// Create a new <see cref="ICondition"/> that will be <see langword="true"/> when <paramref name="property"/> will be in range
        /// </summary>
        /// <param name="property">The <see cref="IProperty{T}"/></param>
        /// <param name="minimum">The range minimum</param>
        /// <param name="maximum">The range maximum</param>
        /// <param name="isMinimumExcluded">The minimum excluded option in the range check</param>
        /// <param name="isMaximumExcluded">The maximum excluded option in the range check</param>
        /// <returns>The new <see cref="ICondition"/></returns>
        public static ICondition IsInRange(IProperty<double> property, double minimum, double maximum, bool isMinimumExcluded = false, bool isMaximumExcluded = false)
        {
            PropertyValueInRange condition = new PropertyValueInRange($"{property.Code}.IsInRange", property, minimum, maximum, isMinimumExcluded, isMaximumExcluded);
            return condition;
        }
    }
}