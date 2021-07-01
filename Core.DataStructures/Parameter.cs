using System;
using System.Reflection;

namespace Core.DataStructures
{
    /// <summary>
    /// Defines the parameter type.
    /// </summary>
    [Serializable]
    public enum ParameterType
    {
        /// <summary>
        /// A <see cref="string"/> type parameter.
        /// </summary>
        String = 0,

        /// <summary>
        /// An <see cref="int"/> type parameter.
        /// </summary>
        Int = 1,

        /// <summary>
        /// A <see cref="double"/> type parameter.
        /// </summary>
        Double = 2,

        /// <summary>
        /// A <see cref="bool"/> type parameter.
        /// </summary>
        Bool = 3
    }

    /// <summary>
    /// Defines a <see cref="Parameter"/> of a method.
    /// See also <see cref="Method"/>.
    /// </summary>
    [Serializable]
    public class Parameter
    {
        private object parameter;
        private ParameterType type;
        private string name;

        /// <summary>
        /// The value of the <see cref="Parameter"/>
        /// </summary>
        public object Value
        {
            get => parameter;
            set => ConvertValue(value);
        }

        /// <summary>
        /// The <see cref="Parameter"/> <see cref="ParameterType"/>
        /// </summary>
        public ParameterType Type => type;

        /// <summary>
        /// The name of the <see cref="Parameter"/>
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Instantiate the <see cref="Parameter"/> object
        /// </summary>
        /// <param name="parameterInfo">The <see cref="ParameterInfo"/></param>
        public Parameter(ParameterInfo parameterInfo)
        {
            this.parameter = new object();
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
        }

        /// <summary>
        /// Convert a general value to the actual one
        /// of the <see cref="Parameter"/>.
        /// </summary>
        /// <param name="value">The general value</param>
        private void ConvertValue(object value)
        {
            switch (type)
            {
                case ParameterType.Int:
                    int.TryParse(value.ToString(), out int toInt);
                    parameter = toInt;
                    break;

                case ParameterType.Double:
                    double.TryParse(value.ToString(), out double toDouble);
                    parameter = toDouble;
                    break;

                case ParameterType.Bool:
                    bool.TryParse(value.ToString(), out bool toBool);
                    parameter = toBool;
                    break;

                case ParameterType.String:
                    parameter = value.ToString();
                    break;
            }
        }

        /// <summary>
        /// Gives an alphabetical description of the <see cref="Parameter"/>
        /// </summary>
        /// <returns>A description of the object</returns>
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}