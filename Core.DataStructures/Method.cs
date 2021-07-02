using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.DataStructures
{
    /// <summary>
    /// A <see cref="ParameterList{T}"/> with OnAdd event handler.
    /// See <see cref="List{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the list items</typeparam>
    [Serializable]
    public class ParameterList<T> : List<T>
    {
        public event EventHandler OnAdd;

        public new void Add(T item)
        {
            if (null != OnAdd)
                OnAdd(this, null);

            base.Add(item);
        }
    }

    /// <summary>
    /// Class that represent a <see cref="Method"/>.
    /// See also <see cref="MethodParameter"/>.
    /// </summary>
    [Serializable]
    public class Method
    {
        private ParameterList<MethodParameter> parameters;
        private int parametersCount;
        private object obj;
        private MethodInfo info;
        private string name;
        private object result;

        /// <summary>
        /// The <see cref="List{T}"/> containing the <see cref="Method"/> <see cref="MethodParameter"/>.
        /// </summary>
        public ParameterList<MethodParameter> Parameters => parameters;

        /// <summary>
        /// The <see cref="Method"/> <see cref="MethodParameter"/> count
        /// </summary>
        public int ParametersCount => parametersCount;

        /// <summary>
        /// The object on which to invoke the method or constructor.
        /// See <see cref="MethodBase.Invoke(object, object[])"/>
        /// </summary>
        public object Object
        {
            get => obj;
            set => obj = value;
        }

        /// <summary>
        /// The <see cref="Method"/> name.
        /// </summary>
        public string Name => name;

        /// <summary>
        /// The <see cref="MethodInfo"/> of the <see cref="Method"/>
        /// </summary>
        public MethodInfo Info
        {
            get => info;
            set => info = value;
        }

        /// <summary>
        /// The result of the method.
        /// If the method doesn't have a result,
        /// than this property is <see langword="null"/>
        /// </summary>
        public object Result => result;

        /// <summary>
        /// <see langword="true"/> if the <see cref="Method"/>
        /// has at least one <see cref="MethodParameter"/>.
        /// </summary>
        public bool HasParameters => parametersCount > 0;

        /// <summary>
        /// Initialize a new <see cref="Method"/> object with default values.
        /// See also <see cref="MethodParameter"/>.
        /// </summary>
        public Method()
        {
            obj = new object();
            result = null;

            parameters = new ParameterList<MethodParameter>();
            parametersCount = 0;

            parameters.OnAdd += Parameters_OnAdd;
        }

        /// <summary>
        /// Initialize a new <see cref="Method"/> object with default values
        /// and assign it a name.
        /// Also see <see cref="Method.Method"/>.
        /// </summary>
        /// <param name="name">The <see cref="Method"/> name</param>
        public Method(string name) : this()
        {
            this.name = name;
        }

        /// <summary>
        /// <see cref="EventHandler"/> for parameters on add.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="EventArgs"/></param>
        private void Parameters_OnAdd(object sender, EventArgs e)
        {
            parametersCount++;
        }

        /// <summary>
        /// Extract the parameters' value from a <see cref="string"/> array
        /// containing all the parameters.
        /// </summary>
        /// <param name="values">The <see cref="string"/> array of parameters</param>
        private void ExtractParameterValue(string[] values)
        {
            if (values != null)
                for (int i = 0; i < Parameters.Count; i++)
                    parameters[i].Value = values[i];
        }

        /// <summary>
        /// Invoke a <see cref="Method"/>.
        /// See <see cref="MethodBase.Invoke(object, object[])"/>.
        /// </summary>
        /// <returns>The result (<see langword="null"/> if not present)</returns>
        public object Invoke(string[] parameters)
        {
            object[] p = new object[] { };

            ExtractParameterValue(parameters);

            if (parameters != null)
            {
                int n = parameters.Length;
                p = new object[n];

                for (int i = 0; i < n; i++)
                    p[i] = this.parameters[i].Value;
            }

            result = info.Invoke(obj, p);

            return result;
        }

        /// <summary>
        /// Perform the method action.
        /// See also <see cref="Method.Invoke"/>
        /// </summary>
        /// <returns>The <see cref="Action"/> wrapped in the <see cref="Method"/></returns>
        public Action UnwrapAction()
        {
            Action action = parameters.Count == 0 ?
                new Action(() => Invoke(null)) :
                new Action(() => Invoke(parameters.Select(x => x.ToString()).ToArray())
            );

            return action;
        }

        /// <summary>
        /// Gives an alphabetical representation of the <see cref="Method"/>
        /// </summary>
        /// <returns>The description of the <see cref="Method"/></returns>
        public override string ToString()
        {
            string description = Name + "(";

            for (int i = 0; i < Parameters.Count; i++)
            {
                MethodParameter parameter = Parameters[i];
                description += parameter.Value;

                if (i < Parameters.Count - 1)
                    description += ", ";
            }

            description += ")";

            if (result != null)
                description += " :: " + result.ToString();

            return description;
        }

        /// <summary>
        /// Gives an extended alphabetical representation of the <see cref="Method"/>.
        /// See also <see cref="ToString"/>
        /// </summary>
        /// <returns>The extended description of the <see cref="Method"/></returns>
        public string ExtendedToString()
        {
            string description = Name + "(";

            for (int i = 0; i < Parameters.Count; i++)
            {
                MethodParameter parameter = Parameters[i];
                description += parameter.Name + ": " + parameter.Value;

                if (i < Parameters.Count - 1)
                    description += ", ";
            }

            description += ")";

            if (result != null)
                description += " :: " + result.ToString();

            return description;
        }
    }
}