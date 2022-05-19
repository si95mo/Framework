using System;
using System.Reflection;

namespace Core.DataStructures
{
    /// <summary>
    /// Defines the <see cref="MethodParameter"/> type
    /// </summary>
    [Serializable]
    public enum ParameterType
    {
        /// <summary>
        /// A <see cref="string"/> type parameter
        /// </summary>
        String = 0,

        /// <summary>
        /// An <see cref="int"/> type parameter
        /// </summary>
        Int = 1,

        /// <summary>
        /// A <see cref="double"/> type parameter
        /// </summary>
        Double = 2,

        /// <summary>
        /// A <see cref="bool"/> type parameter
        /// </summary>
        Bool = 3,

        /// <summary>
        /// An <see cref="System.Enum"/> type parameter
        /// </summary>
        Enum = 4
    }

    /// <summary>
    /// Defines a <see cref="MethodParameter"/> of a <see cref="Method"/>.
    /// See also <see cref="Method"/>
    /// </summary>
    [Serializable]
    public class MethodParameter
    {
        private object value;
        private ParameterType type;
        private Type exactType;
        private string name;

        /// <summary>
        /// The value of the <see cref="MethodParameter"/>
        /// </summary>
        public object Value
        {
            get => value;
            set => ConvertValue(value);
        }

        /// <summary>
        /// The <see cref="MethodParameter"/> <see cref="ParameterType"/>
        /// </summary>
        public ParameterType Type => type;

        /// <summary>
        /// The <see cref="MethodParameter"/> exact <see cref="System.Type"/>
        /// (i.e. the actual <see cref="System.Type"/> and not the base one)
        /// </summary>
        public Type ExactType => exactType;

        /// <summary>
        /// The name of the <see cref="MethodParameter"/>
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Instantiate the <see cref="MethodParameter"/> object
        /// </summary>
        /// <param name="parameterInfo">The <see cref="ParameterInfo"/></param>
        public MethodParameter(ParameterInfo parameterInfo)
        {
            this.value = new object();
            name = parameterInfo.Name;

            Type parameterType = parameterInfo.ParameterType;

            if (parameterType == typeof(int))
                type = ParameterType.Int;

            if (parameterType == typeof(double))
                type = ParameterType.Double;

            if (parameterType == typeof(bool))
                type = ParameterType.Bool;

            if (parameterType == typeof(string))
                type = ParameterType.String;

            if (parameterType.IsEnum)
                type = ParameterType.Enum;

            exactType = parameterType;
        }

        /// <summary>
        /// Convert a general value to the actual one
        /// of the <see cref="MethodParameter"/>.
        /// </summary>
        /// <param name="value">The general value</param>
        private void ConvertValue(object value)
        {
            switch (type)
            {
                case ParameterType.Int:
                    int.TryParse(value.ToString(), out int toInt);
                    this.value = toInt;
                    break;

                case ParameterType.Double:
                    double.TryParse(value.ToString(), out double toDouble);
                    this.value = toDouble;
                    break;

                case ParameterType.Bool:
                    bool.TryParse(value.ToString(), out bool toBool);
                    this.value = toBool;
                    break;

                case ParameterType.String:
                    this.value = value.ToString();
                    break;

                case ParameterType.Enum:
                    this.value = Enum.Parse(exactType, value.ToString());
                    break;
            }
        }

        /// <summary>
        /// Gives an alphabetical description of the <see cref="MethodParameter"/>
        /// </summary>
        /// <returns>A description of the object</returns>
        public override string ToString()
        {
            return $"{value}";
        }

        /// <summary>
        /// Gives an extended alphabetical description of the <see cref="MethodParameter"/>
        /// </summary>
        /// <returns>An extended description of the object</returns>
        public string ExtendedToString()
        {
            return $"{name} = {value}";
        }
    }
}