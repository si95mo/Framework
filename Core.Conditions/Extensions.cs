using Core.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Core.Conditions
{
    public static class Extensions
    {
        /// <summary>
        /// Add a description to an <see cref="ICondition"/> (in <see cref="ICondition.Description"/>
        /// </summary>
        /// <param name="condition">The source <see cref="ICondition"/></param>
        /// <param name="description">The description</param>
        public static void WithDescription(this ICondition condition, string description)
            => condition.Description = description;

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

            source.ValueChanged += (sender, e) => UpdateAndCondition(source, andCondition);
            condition.ValueChanged += (sender, e) => UpdateAndCondition(condition, andCondition);

            return andCondition;
        }

        /// <summary>
        /// Update a <see cref="FlyweightCondition"/> by applying an <see langword="and"/> operand between two <see langword="bool"/> values
        /// </summary>
        /// <param name="changedCondition">The sender (the <see cref="ICondition"/> of which the value has changed)</param>
        /// <param name="andCondition">The <see cref="FlyweightCondition"/> result of the <see cref="And(ICondition, ICondition)"/> method</param>
        private static void UpdateAndCondition(ICondition changedCondition, ICondition andCondition)
            => andCondition.Value &= changedCondition.Value;

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
            ICondition result;
            if (source.Count() > 1) // At lest 2 conditions in the collection
            {
                result = source.First();
                foreach(ICondition condition in source.Skip(1)) // First element already taken into account
                    result = result.And(condition); // And through all the collection elements
            }
            else if (source.Count() == 1)
                result = source.ElementAt(0);
            else
                result = null;

            return result;
        }

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

            source.ValueChanged += (sender, e) => UpdateOrCondition(source, orCondition);
            condition.ValueChanged += (sender, e) => UpdateOrCondition(condition, orCondition);

            return orCondition;
        }

        /// <summary>
        /// Update a <see cref="FlyweightCondition"/> by applying an <see langword="or"/> operand between two <see langword="bool"/> values
        /// </summary>
        /// <param name="changedCondition">The sender (the <see cref="ICondition"/> of which the value has changed)</param>
        /// <param name="orCondition">The <see cref="FlyweightCondition"/> result of the <see cref="Or(ICondition, ICondition)"/> method</param>
        private static void UpdateOrCondition(ICondition changedCondition, ICondition orCondition)
            => orCondition.Value |= changedCondition.Value;

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
            ICondition result;
            if (source.Count() > 1) // At lest 2 conditions in the collection
            {
                result = source.First();
                foreach (ICondition condition in source.Skip(1)) // First element already taken into account
                    result = result.Or(condition); // And through all the collection elements
            }
            else if (source.Count() == 1)
                result = source.ElementAt(0);
            else
                result = null;

            return result;
        }

        #endregion Or

        /// <summary>
        /// Update a <see cref="FlyweightCondition"/> by applying a <see langword="not"/> operand
        /// </summary>
        /// <param name="source">The source <see cref="ICondition"/> of which negate the value</param>
        /// <returns>The resulted negated <see cref="ICondition"/></returns>
        public static ICondition Negate(this ICondition source)
        {
            FlyweightCondition condition = new FlyweightCondition($"{source.Code}.Negated", !source.Value);
            source.ValueChanged += (sender, e) => condition.Value = !e.NewValueAsBool;

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
        /// <returns>The new <see cref="PropertyValueInRange"/></returns>
        public static PropertyValueInRange IsInRange(IProperty<double> property, double minimum, double maximum, bool isMinimumExcluded = false, bool isMaximumExcluded = false)
        {
            PropertyValueInRange condition = new PropertyValueInRange($"{property.Code}.IsInRange", property, minimum, maximum, isMinimumExcluded, isMaximumExcluded);
            return condition;
        }

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
    }
}