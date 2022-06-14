using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.DataStructures
{
    /// <summary>
    /// A wrapper for <see cref="Method"/>.
    /// </summary>
    public static class MethodWrapper
    {
        /// <summary>
        /// Wrap all methods of an <see cref="object"/> into
        /// a <see cref="List{T}"/> of <see cref="Method"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> of which wrap all methods</param>
        /// <returns>The <see cref="List{T}"/> of <see cref="Method"/></returns>
        public static List<Method> Wrap(object obj)
        {
            List<Method> methods = new List<Method>();

            IEnumerable<MethodInfo> methodsEnumerable = obj.GetType().GetMethods()
                .Where(m => m.DeclaringType != typeof(object));

            //MethodInfo[] methods = new MethodInfo[methodsEnumerable.Count()];
            foreach (MethodInfo method in methodsEnumerable)
            {
                if (method.IsPublic && !method.IsSpecialName && method.Name.CompareTo("ToString") != 0)
                    methods.Add(Wrap(method, obj));
            }

            return methods;
        }

        /// <summary>
        /// Wrap a single <see cref="object"/> method into a <see cref="Method"/>.
        /// </summary>
        /// <param name="methodInfo">The <see cref="MethodInfo"/></param>
        /// <param name="obj">The <see cref="object"/> of which wrap a <see cref="Method"/></param>
        /// <returns>The <see cref="Method"/> wrapped</returns>
        private static Method Wrap(MethodInfo methodInfo, object obj)
        {
            Method method = new Method(methodInfo.Name)
            {
                Info = methodInfo,
                Object = obj
            };

            var paramters = methodInfo.GetParameters().ToList();
            foreach (var param in paramters)
                method.Parameters.Add(new MethodParameter(param));

            return method;
        }
    }
}