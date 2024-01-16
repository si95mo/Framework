using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Conditions
{
    public static class CoditionsExtensions
    {
        /// <summary>
        /// Add a description to an <see cref="ICondition"/> (in <see cref="ICondition.Description"/>
        /// </summary>
        /// <param name="condition">The source <see cref="ICondition"/></param>
        /// <param name="description">The description</param>
        public static void WithDescription(this ICondition condition, string description)
            => condition.Description = description;

        /// <summary>
        /// Create a new <see cref="ICondition"/> (<see cref="ConstantCondition"/>) based on <paramref name="source"/> value
        /// </summary>
        /// <param name="source">The source to convert</param>
        /// <returns>The result <see cref="ICondition"/></returns>
        public static ICondition AsConstantCondition(this bool source)
        {
            ICondition condition = new ConstantCondition("As constant condition", source);
            return condition;
        }

        #region IsTrue

        /// <summary>
        /// Create a new <see cref="ICondition"/> that is <see langword="true"/> when <paramref name="source"/> is <see langword="true"/>
        /// </summary>
        /// <param name="source">The source <see cref="ICondition"/></param>
        /// <returns>The new <see cref="ICondition"/></returns>
        public static ICondition IsTrue(this ICondition source)
        {
            ICondition result = new FlyweightCondition($"{source.Code}.IsTrue", source.Value);
            source.ConnectTo(result);

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

        #endregion IsTrue

        #region IsFalse

        /// <summary>
        /// Create a new <see cref="ICondition"/> that is <see langword="true"/> when <paramref name="source"/> is <see langword="false"/>
        /// </summary>
        /// <param name="source">The source <see cref="ICondition"/></param>
        /// <returns>The new <see cref="ICondition"/></returns>
        public static ICondition IsFalse(this ICondition source)
        {
            ICondition result = new FlyweightCondition($"{source.Code}.IsFalse", source.Value == false);
            source.ValueChanged += (sender, e) => UpdateIsFalseCondition(source, result);

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
        private static void UpdateIsFalseCondition(ICondition source, ICondition newCondition)
            => newCondition.Value = source.Value == false;

        /// <summary>
        /// Update a <see cref="FlyweightCondition"/> to check if its value is <see langword="false"/>
        /// </summary>
        /// <param name="source">The source <see cref="IProperty{T}"/></param>
        /// <param name="newCondition">The <see cref="FlyweightCondition"/> to update</param>
        private static void UpdateIfFalseCondition(IProperty<bool> source, FlyweightCondition newCondition)
            => newCondition.Value = source.Value == false;

        #endregion IsFalse

        #region Boolean logic

        #region And

        /// <summary>
        /// Create a <see cref="FlyweightCondition"/> that concatenates itself with another <see cref="ICondition"/> with an <see langword="and"/> relation
        /// </summary>
        /// <param name="source">The source <see cref="ICondition"/></param>
        /// <param name="condition">The other condition to which concatenate</param>
        /// <returns>The concatenated <see cref="ICondition"/></returns>
        public static ICondition And(this ICondition source, ICondition condition)
        {
            ICondition andCondition = new FlyweightCondition($"{source.Code}.And.{condition.Code}", source.Value & condition.Value);

            source.ValueChanged += (sender, e) => andCondition.Value = UpdateAndCondition(source, condition);
            condition.ValueChanged += (sender, e) => andCondition.Value = UpdateAndCondition(source, condition);

            return andCondition;
        }

        /// <summary>
        /// Update a <see cref="FlyweightCondition"/> by applying an <see langword="and"/> operand between two <see langword="bool"/> values
        /// </summary>
        /// <param name="firstCondition">The sender (the <see cref="ICondition"/> of which the value has changed)</param>
        /// <param name="secondCondition">The <see cref="FlyweightCondition"/> result of the <see cref="And(ICondition, ICondition)"/> method</param>
        private static bool  UpdateAndCondition(ICondition firstCondition, ICondition secondCondition)
            => secondCondition.Value && firstCondition.Value;

        /// <summary>
        /// Ands all the <see cref="ICondition"/> contained in <paramref name="source"/>
        /// </summary>
        /// <remarks>
        /// If <paramref name="source"/> only have one element, than the result will be that element itself. If <paramref name="source"/> contains no
        /// elements, then the result will be <see langword="null"/>
        /// </remarks>
        /// <param name="source">The <see cref="IEnumerable{T}"/> with all the <see cref="ICondition"/> to and</param>
        /// <returns>The result <see cref="ICondition"/></returns>
        public static ICondition And(this IEnumerable<ICondition> source)
        {
            ICondition andCondition = null;
            if (source.Count() > 1) // At lest 2 conditions in the collection
            {
                andCondition = source.First();
                foreach (ICondition condition in source.Skip(1)) // First element already taken into account
                {
                    andCondition = andCondition.And(condition); // And through all the collection elements
                }
            }

            return andCondition;
        }

        /// <summary>
        /// Ands all the <see cref="ICondition"/> contained in <paramref name="conditions"/> with <paramref name="source"/>
        /// </summary>
        /// <param name="source">The initial <see cref="ICondition"/>to and</param>
        /// <param name="conditions">The collection of <see cref="ICondition"/> to and</param>
        /// <returns>The result <see cref="ICondition"/></returns>
        public static ICondition And(this ICondition source, params ICondition[] conditions)
            => conditions.And().And(source);

        #endregion And

        #region Or

        /// <summary>
        /// Create a <see cref="FlyweightCondition"/> that concatenates itself with another <see cref="ICondition"/> with an <see langword="or"/> relation
        /// </summary>
        /// <param name="source">The source <see cref="ICondition"/></param>
        /// <param name="condition">The other condition to which concatenate</param>
        /// <returns>The concatenated <see cref="FlyweightCondition"/></returns>
        public static ICondition Or(this ICondition source, ICondition condition)
        {
            ICondition orCondition = new FlyweightCondition($"{source.Code}.And.{condition.Code}", source.Value | condition.Value);

            source.ValueChanged += (sender, e) => orCondition.Value = UpdateOrCondition(source, condition);
            condition.ValueChanged += (sender, e) => orCondition.Value = UpdateOrCondition(source, condition);

            return orCondition;
        }

        /// <summary>
        /// Update a <see cref="FlyweightCondition"/> by applying an <see langword="or"/> operand between two <see langword="bool"/> values
        /// </summary>
        /// <param name="firstCondition">The sender (the <see cref="ICondition"/> of which the value has changed)</param>
        /// <param name="secondCondition">The <see cref="FlyweightCondition"/> result of the <see cref="Or(ICondition, ICondition)"/> method</param>
        private static bool UpdateOrCondition(ICondition firstCondition, ICondition secondCondition)
            => secondCondition.Value || firstCondition.Value;

        /// <summary>
        /// Ors all the <see cref="ICondition"/> contained in <paramref name="source"/>
        /// </summary>
        /// <remarks>
        /// If <paramref name="source"/> only have one element, than the result will be that element itself. If <paramref name="source"/> contains no
        /// elements, then the result will be <see langword="null"/>
        /// </remarks>
        /// <param name="source">The <see cref="IEnumerable{T}"/> with all the <see cref="ICondition"/> to or</param>
        /// <returns>The result <see cref="ICondition"/></returns>
        public static ICondition Or(this IEnumerable<ICondition> source)
        {
            ICondition orCondition = null;
            if (source.Any()) // At lest 2 conditions in the collection
            {
                orCondition = source.First();
                foreach (ICondition condition in source.Skip(1)) // First element already taken into account
                {
                    orCondition = orCondition.Or(condition); // And through all the collection elements
                }
            }

            return orCondition;
        }

        /// <summary>
        /// Ors all the <see cref="ICondition"/> contained in <paramref name="conditions"/> with <paramref name="source"/>
        /// </summary>
        /// <param name="source">The initial <see cref="ICondition"/>to or</param>
        /// <param name="conditions">The collection of <see cref="ICondition"/> to or</param>
        /// <returns>The result <see cref="ICondition"/></returns>
        public static ICondition Or(this ICondition source, params ICondition[] conditions)
            => conditions.Or().Or(source);

        #endregion Or

        #region Xor

        /// <summary>
        /// Create a <see cref="ICondition"/> that concatenates itself with another <see cref="ICondition"/> with an <see langword="xor"/> relation
        /// </summary>
        /// <param name="source">The source <see cref="ICondition"/></param>
        /// <param name="condition">The other condition to which concatenate</param>
        /// <returns>The concatenated <see cref="ICondition"/></returns>
        public static ICondition Xor(this ICondition source, ICondition condition)
        {
            FlyweightCondition xorCondition = new FlyweightCondition($"{source.Code}.Xor.{condition.Code}", source.Value | condition.Value);

            source.ValueChanged += (sender, e) => xorCondition.Value = UpdateXorCondition(source, xorCondition);
            condition.ValueChanged += (sender, e) => xorCondition.Value = UpdateXorCondition(condition, xorCondition);

            return xorCondition;
        }

        /// <summary>
        /// Create a <see cref="ICondition"/> that xors all <see cref="ICondition"/> contained in the <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <remarks>
        /// This method returns a <see langword="null"/> if <paramref name="source"/> contains no elements
        /// </remarks>
        /// <param name="source">The source <see cref="ICondition"/></param>
        /// <returns>The concatenated <see cref="ICondition"/></returns>
        public static ICondition Xor(this IEnumerable<ICondition> source)
        {
            ICondition xorCondition = null;
            if (source.Any())
            {
                xorCondition = source.ElementAt(0);
                foreach (ICondition newCondition in source.Skip(1))
                {
                    xorCondition = xorCondition.Xor(newCondition);
                }
            }

            return xorCondition;
        }

        /// <summary>
        /// Xors all the <see cref="ICondition"/> contained in <paramref name="conditions"/> with <paramref name="source"/>
        /// </summary>
        /// <param name="source">The initial <see cref="ICondition"/>to xor</param>
        /// <param name="conditions">The collection of <see cref="ICondition"/> to xor</param>
        /// <returns>The result <see cref="ICondition"/></returns>
        public static ICondition Xor(this ICondition source, params ICondition[] conditions)
            => conditions.Xor().Xor(source);

        /// <summary>
        /// Update a <see cref="FlyweightCondition"/> by applying an <see langword="xor"/> operand between two <see langword="bool"/> values
        /// </summary>
        /// <param name="firstCondition">The sender (the <see cref="ICondition"/> of which the value has changed)</param>
        /// <param name="secondCondition">The <see cref="FlyweightCondition"/> result of the <see cref="Or(ICondition, ICondition)"/> method</param>
        private static bool UpdateXorCondition(ICondition firstCondition, FlyweightCondition secondCondition)
            => secondCondition.Value ^ firstCondition.Value;

        #endregion Xor

        #region Nand

        /// <summary>
        /// Create a <see cref="ICondition"/> that concatenates itself with another <see cref="ICondition"/> with an <see langword="nand"/> relation
        /// </summary>
        /// <param name="source">The source <see cref="ICondition"/></param>
        /// <param name="condition">The other condition to which concatenate</param>
        /// <returns>The concatenated <see cref="ICondition"/></returns>
        public static ICondition Nand(this ICondition source, ICondition condition)
        {
            ICondition nandCondition = source.And(condition).Not();
            return nandCondition;
        }

        /// <summary>
        /// Create a <see cref="ICondition"/> that nands all the <see cref="ICondition"/> contained in the <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <remarks>
        /// This method returns a <see langword="null"/> if <paramref name="source"/> contains no elements
        /// </remarks>
        /// <param name="source">The source <see cref="ICondition"/></param>
        /// <returns>The concatenated <see cref="ICondition"/></returns>
        public static ICondition Nand(this IEnumerable<ICondition> source)
        {
            ICondition nandCondition = null;
            if (source.Any())
            {
                nandCondition = source.ElementAt(0);
                foreach (ICondition newCondition in source.Skip(1))
                {
                    nandCondition = nandCondition.Nand(newCondition);
                }
            }

            return nandCondition;
        }

        /// <summary>
        /// Nands all the <see cref="ICondition"/> contained in <paramref name="conditions"/> with <paramref name="source"/>
        /// </summary>
        /// <param name="source">The initial <see cref="ICondition"/>to nand</param>
        /// <param name="conditions">The collection of <see cref="ICondition"/> to nand</param>
        /// <returns>The result <see cref="ICondition"/></returns>
        public static ICondition Nand(this ICondition source, params ICondition[] conditions)
            => conditions.Nand().Nand(source);

        #endregion Nand

        #region Not

        /// <summary>
        /// Update a <see cref="FlyweightCondition"/> by applying a <see langword="not"/> operand
        /// </summary>
        /// <param name="source">The source <see cref="ICondition"/> of which negate the value</param>
        /// <returns>The resulted negated <see cref="ICondition"/></returns>
        public static ICondition Not(this ICondition source)
        {
            FlyweightCondition condition = new FlyweightCondition($"{source.Code}.Negated", !source.Value);
            source.ValueChanged += (sender, e) => condition.Value = !e.NewValueAsBool;

            return condition;
        }

        #endregion Not

        #endregion Boolean logic

        #region In range

        /// <summary>
        /// Create a new <see cref="ICondition"/> that will be <see langword="true"/> when <paramref name="property"/> will be in range
        /// </summary>
        /// <param name="property">The <see cref="IProperty{T}"/></param>
        /// <param name="minimum">The range minimum</param>
        /// <param name="maximum">The range maximum</param>
        /// <param name="isMinimumExcluded">The minimum excluded option in the range check</param>
        /// <param name="isMaximumExcluded">The maximum excluded option in the range check</param>
        /// <returns>The new <see cref="ICondition"/></returns>
        public static ICondition IsInRange(this IProperty<double> property, double minimum, double maximum, bool isMinimumExcluded = false, bool isMaximumExcluded = false)
        {
            ICondition condition = new PropertyValueInRange($"{property.Code}.IsInRange", property, minimum, maximum, isMinimumExcluded, isMaximumExcluded);
            return condition;
        }

        /// <summary>
        /// Create a new <see cref="ICondition"/> that will be <see langword="true"/> when <paramref name="property"/> will be in range
        /// </summary>
        /// <param name="property">The <see cref="IProperty{T}"/></param>
        /// <param name="minimum">The range minimum</param>
        /// <param name="maximum">The range maximum</param>
        /// <param name="isMinimumExcluded">The minimum excluded option in the range check</param>
        /// <param name="isMaximumExcluded">The maximum excluded option in the range check</param>
        /// <returns>The new <see cref="ICondition"/></returns>
        public static ICondition IsInRange<T>(this IProperty<T> property, T minimum, T maximum, bool isMinimumExcluded = false, bool isMaximumExcluded = false)
            where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            ICondition condition;
            if (property is IProperty<double> castedProperty)
            {
                condition = new PropertyValueInRange(
                    $"{property.Code}.IsInRange",
                    castedProperty,
                    Convert.ToDouble(minimum),
                    Convert.ToDouble(maximum),
                    isMinimumExcluded,
                    isMaximumExcluded
                );
            }
            else
            {
                condition = new DummyCondition($"{property.Code}.IsInRange", false);
            }

            return condition;
        }

        /// <summary>
        /// Create a new <see cref="ICondition"/> that will be <see langword="true"/> when <paramref name="property"/> will be in range
        /// </summary>
        /// <param name="property">The <see cref="IProperty{T}"/></param>
        /// <param name="minimum">The range minimum</param>
        /// <param name="maximum">The range maximum</param>
        /// <param name="isMinimumExcluded">The minimum excluded option in the range check</param>
        /// <param name="isMaximumExcluded">The maximum excluded option in the range check</param>
        /// <returns>The new <see cref="ICondition"/></returns>
        public static ICondition IsInRange(this IProperty<double> property, IProperty<double> minimum, IProperty<double> maximum,
            bool isMinimumExcluded = false, bool isMaximumExcluded = false)
        {
            ICondition condition;
            if (property is IProperty<double> castedProperty)
            {
                condition = new PropertyValueInRange(
                    $"{property.Code}.IsInRange",
                    castedProperty,
                    minimum,
                    maximum,
                    isMinimumExcluded,
                    isMaximumExcluded
                );
            }
            else
            {
                condition = new DummyCondition($"{property.Code}.IsInRange", false);
            }

            return condition;
        }

        #endregion In range

        #region Not in range

        /// <summary>
        /// Create a new <see cref="ICondition"/> that will be <see langword="true"/> when <paramref name="property"/> will not be in range
        /// </summary>
        /// <param name="property">The <see cref="IProperty{T}"/></param>
        /// <param name="minimum">The range minimum</param>
        /// <param name="maximum">The range maximum</param>
        /// <param name="isMinimumExcluded">The minimum excluded option in the range check</param>
        /// <param name="isMaximumExcluded">The maximum excluded option in the range check</param>
        /// <returns>The new <see cref="ICondition"/></returns>
        public static ICondition IsNotInRange(this IProperty<double> property, double minimum, double maximum, bool isMinimumExcluded = false, bool isMaximumExcluded = false)
            => property.IsInRange(minimum, maximum, isMinimumExcluded, isMaximumExcluded).Not();

        /// <summary>
        /// Create a new <see cref="ICondition"/> that will be <see langword="true"/> when <paramref name="property"/> will not be in range
        /// </summary>
        /// <param name="property">The <see cref="IProperty{T}"/></param>
        /// <param name="minimum">The range minimum</param>
        /// <param name="maximum">The range maximum</param>
        /// <param name="isMinimumExcluded">The minimum excluded option in the range check</param>
        /// <param name="isMaximumExcluded">The maximum excluded option in the range check</param>
        /// <returns>The new <see cref="ICondition"/></returns>
        public static ICondition IsNotInRange<T>(this IProperty<T> property, T minimum, T maximum, bool isMinimumExcluded = false, bool isMaximumExcluded = false)
            where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
            => property.IsInRange(minimum, maximum, isMinimumExcluded, isMaximumExcluded).Not();

        /// <summary>
        /// Create a new <see cref="ICondition"/> that will be <see langword="true"/> when <paramref name="property"/> will not be in range
        /// </summary>
        /// <param name="property">The <see cref="IProperty{T}"/></param>
        /// <param name="minimum">The range minimum</param>
        /// <param name="maximum">The range maximum</param>
        /// <param name="isMinimumExcluded">The minimum excluded option in the range check</param>
        /// <param name="isMaximumExcluded">The maximum excluded option in the range check</param>
        /// <returns>The new <see cref="ICondition"/></returns>
        public static ICondition IsNotInRange(this IProperty<double> property, IProperty<double> minimum, IProperty<double> maximum,
            bool isMinimumExcluded = false, bool isMaximumExcluded = false)
            => property.IsInRange(minimum, maximum, isMinimumExcluded, isMaximumExcluded).Not();

        #endregion Not in range

        #region Is stable for

        /// <summary>
        /// Create a new <see cref="ICondition"/> that will be <see langword="true"/> when <paramref name="source"/>
        /// will be stable for at least <paramref name="stabilizationTime"/>
        /// </summary>
        /// <param name="source">The source <see cref="ICondition"/></param>
        /// <param name="stabilizationTime">The stabilization time</param>
        /// <returns>The stabilized <see cref="ICondition"/></returns>
        public static ICondition IsStableFor(this ICondition source, TimeSpan stabilizationTime)
        {
            TimeElapsedCondition condition = new TimeElapsedCondition($"{source.Code}.IsStableFor", stabilizationTime);
            condition.Start();

            source.ValueChanged += (s, e) => condition.Start();

            return condition;
        }

        /// <summary>
        /// Create a new <see cref="ICondition"/> that will be <see langword="true"/> when <paramref name="source"/>
        /// will be stable for at least <paramref name="stabilizationTimeInMilliseconds"/>
        /// </summary>
        /// <param name="source">The source <see cref="ICondition"/></param>
        /// <param name="stabilizationTimeInMilliseconds">The stabilization time in milliseconds</param>
        /// <returns>The stabilized <see cref="ICondition"/></returns>
        public static ICondition IsStableFor(this ICondition source, double stabilizationTimeInMilliseconds)
            => source.IsStableFor(TimeSpan.FromMilliseconds(stabilizationTimeInMilliseconds));

        #endregion Is stable for

        #region Equality

        #region Is equal

        /// <summary>
        /// Create a new <see cref="ICondition"/> that will be <see langword="true"/> when <paramref name="source"/> will have its <see cref="IProperty{T}.Value"/>
        /// equal to <paramref name="value"/>
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IProperty{T}"/> and <paramref name="value"/></typeparam>
        /// <param name="source">The source <see cref="IProperty{T}"/></param>
        /// <param name="value">The value to test</param>
        /// <returns>The equal to <see cref="ICondition"/></returns>
        public static ICondition IsEqualTo<T>(this IProperty<T> source, T value)
        {
            ICondition condition = new PropertyValueEqualTo<T>($"{source.Code}.IsEuqualTo", source, value);
            return condition;
        }

        public static ICondition IsEqualTo<T>(this IProperty<T> source, IProperty<T> value)
        {
            ICondition condition = new PropertyValueEqualTo<T>($"{source.Code}.IsEuqualTo", source, value);
            return condition;
        }

        public static ICondition IsEqualTo(this IProperty<double> property, double target)
        {
            FlyweightCondition condition = new FlyweightCondition($"{property.Code}.IsEqualTo", property.Value == target);
            property.ValueChanged += (s, e) => condition.Value = property.Value == target;

            return condition;
        }

        #endregion Is equal

        #region Is not equal to

        /// <summary>
        /// Create a new <see cref="ICondition"/> that will be <see langword="true"/> when <paramref name="source"/> will have its <see cref="IProperty{T}.Value"/>
        /// not equal to <paramref name="value"/>
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IProperty{T}"/> and <paramref name="value"/></typeparam>
        /// <param name="source">The source <see cref="IProperty{T}"/></param>
        /// <param name="value">The value to test</param>
        /// <returns>The not equal to <see cref="ICondition"/></returns>
        public static ICondition IsNotEqualTo<T>(this IProperty<T> source, T value)
        {
            ICondition condition = source.IsEqualTo(value);
            condition = condition.Not();

            return condition;
        }

        public static ICondition IsNotEqualTo<T>(this IProperty<T> source, IProperty<T> value)
        {
            ICondition condition = source.IsEqualTo(value);
            condition = condition.Not();

            return condition;
        }

        public static ICondition IsNotEqualTo(this IProperty<double> source, double value)
        {
            ICondition condition = source.IsEqualTo(value);
            condition = condition.Not();

            return condition;
        }

        #endregion Is not equal to

        /// <summary>
        /// Create a new <see cref="ICondition"/> that will be <see langword="true"/> when <paramref name="property"/> will have its <see cref="IProperty{T}.Value"/>
        /// less than <paramref name="target"/>
        /// </summary>
        /// <param name="property">The source <see cref="IProperty{T}"/></param>
        /// <param name="target">The value to test</param>
        /// <returns>The new <see cref="ICondition"/></returns>
        public static ICondition IsLessThan(this IProperty<double> property, double target)
        {
            FlyweightCondition condition = new FlyweightCondition($"{property.Code}.IsLessThan", property.Value < target);
            property.ValueChanged += (s, e) => condition.Value = property.Value < target;

            return condition;
        }

        /// <summary>
        /// Create a new <see cref="ICondition"/> that will be <see langword="true"/> when <paramref name="property"/> will have its <see cref="IProperty{T}.Value"/>
        /// less than or equal to <paramref name="target"/>
        /// </summary>
        /// <param name="property">The source <see cref="IProperty{T}"/></param>
        /// <param name="target">The value to test</param>
        /// <returns>The new <see cref="ICondition"/></returns>
        public static ICondition IsLessThanOrEqualTo(this IProperty<double> property, double target)
        {
            FlyweightCondition condition = new FlyweightCondition($"{property.Code}.IsLessThanOrEqualTo", property.Value <= target);
            property.ValueChanged += (s, e) => condition.Value = property.Value <= target;

            return condition;
        }

        /// <summary>
        /// Create a new <see cref="ICondition"/> that will be <see langword="true"/> when <paramref name="property"/> will have its <see cref="IProperty{T}.Value"/>
        /// greater than <paramref name="target"/>
        /// </summary>
        /// <param name="property">The source <see cref="IProperty{T}"/></param>
        /// <param name="target">The value to test</param>
        /// <returns>The new <see cref="ICondition"/></returns>
        public static ICondition IsGreaterThan(this IProperty<double> property, double target)
        {
            FlyweightCondition condition = new FlyweightCondition($"{property.Code}.IsGreater", property.Value > target);
            property.ValueChanged += (s, e) => condition.Value = property.Value > target;

            return condition;
        }

        /// <summary>
        /// Create a new <see cref="ICondition"/> that will be <see langword="true"/> when <paramref name="property"/> will have its <see cref="IProperty{T}.Value"/>
        /// greater than or equal to <paramref name="target"/>
        /// </summary>
        /// <param name="property">The source <see cref="IProperty{T}"/></param>
        /// <param name="target">The value to test</param>
        /// <returns>The new <see cref="ICondition"/></returns>
        public static ICondition IsGreaterThanOrEqualTo(this IProperty<double> property, double target)
        {
            FlyweightCondition condition = new FlyweightCondition($"{property.Code}.IsGreaterThanOrEqualTo", property.Value >= target);
            property.ValueChanged += (s, e) => condition.Value = property.Value >= target;

            return condition;
        }

        #endregion Equality

        #region WaitFor

        /// <summary>
        /// Wait for an <see cref="ICondition"/> to be <see langword="true"/> without timeout
        /// </summary>
        /// <param name="_">The source</param>
        /// <param name="condition">The <see cref="ICondition"/> to wait</param>
        /// <returns>The (async) <see cref="Task"/></returns>
        public static async Task WaitFor(this IProperty _, ICondition condition)
        {
            if (!condition.Value)
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                EventHandler<ValueChangedEventArgs> eventHandler = (__, e) =>
                {
                    if ((bool)e.NewValue)
                    {
                        tokenSource.Cancel();
                    }
                };

                condition.ValueChanged += eventHandler;
                await Task.Delay(-1, tokenSource.Token).ContinueWith((x) => { }); // Prevent the exception throw;
                condition.ValueChanged -= eventHandler;
            }
            else
            {
                await Task.CompletedTask;
            }
        }

        /// <summary>
        /// Wait for an <see cref="ICondition"/> to be <see langword="true"/> with a timeout
        /// </summary>
        /// <param name="_">The source</param>
        /// <param name="condition">The <see cref="ICondition"/> to wait</param>
        /// <param name="timeout">The timeout (in milliseconds)</param>
        /// <returns>
        /// The (async) <see cref="Task{T}"/> (in which the result will be <see langword="true"/> if
        /// the <paramref name="condition"/> became <see langword="true"/> before <paramref name="timeout"/> occurred,
        /// <see langword="false"/> otherwise)
        /// </returns>
        public static async Task<bool> WaitFor(this IProperty _, ICondition condition, int timeout)
        {
            bool result = true;

            if (!condition.Value)
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                void eventHandler(object __, ValueChangedEventArgs e)
                {
                    if ((bool)e.NewValue)
                    {
                        tokenSource.Cancel();
                    }
                }

                condition.ValueChanged += eventHandler;

                Stopwatch timer = Stopwatch.StartNew();
                await Task.Delay(timeout, tokenSource.Token).ContinueWith((x) => { }); // Prevent the exception throw
                timer.Stop();

                condition.ValueChanged -= eventHandler;

                result = timer.Elapsed.TotalMilliseconds <= timeout;
            }

            return result;
        }

        #endregion WaitFor
    }
}