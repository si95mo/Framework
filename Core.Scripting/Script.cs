using Core.DataStructures;
using Diagnostic;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Core.Scripting
{
    /// <summary>
    /// Implement a script class
    /// </summary>
    public abstract class Script : IScript
    {
        /// <summary>
        /// The code
        /// </summary>
        public string Code { get; protected set; }

        /// <summary>
        /// The message to show
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The value as object
        /// </summary>
        public object ValueAsObject { get => Code; set => _ = value; }

        /// <summary>
        /// The <see cref="System.Type"/>
        /// </summary>
        public Type Type => GetType();

        /// <summary>
        /// Initialize the new instance with default parameters
        /// </summary>
        /// <param name="code">The code</param>
        protected Script(string code)
        {
            Code = code;
            Message = string.Empty;

            if (ServiceBroker.CanProvide<ScriptsService>())
                ServiceBroker.GetService<ScriptsService>().Add(this);
            else
                Logger.Error($"{nameof(ServiceBroker)} cannot provide {nameof(ScriptsService)}, unable to add the script with code {Code} to the repository");
        }

        /// <summary>
        /// Initialize the new instance
        /// </summary>
        protected Script() : this(Guid.NewGuid().ToString())
        { }

        public abstract void Run();

        public abstract void Clear();

        public virtual IScript New(Assembly assembly, string code, string typeName)
            => NewInstance(assembly, code, typeName);

        /// <summary>
        /// Create a new instance of <see cref="Script"/> from a <see cref="string"/>
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/></param>
        /// <param name="code">The code</param>
        /// <param name="typeName">The class name</param>
        /// <returns>The new instance of <see cref="Script"/></returns>
        public static IScript NewInstance(Assembly assembly, string code, string typeName)
        {
            List<ConstructorInfo> constructors = new List<ConstructorInfo>();
            Type[] types = assembly.GetTypes();

            foreach (Type type in types)
                if (type.Name.Contains(typeName))
                    constructors.AddRange(type.GetConstructors());

            ConstructorInfo constructor = constructors[0];
            Script script = constructor.Invoke(new object[] { code }) as Script;

            return script;
        }
    }
}