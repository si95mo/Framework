using Diagnostic;
using OX.Copyable;
using System;

namespace Extensions
{
    /// <summary>
    /// Provides a method for performing a deep copy of an object. <br/>
    /// Reflection is used to perform the deep copy
    /// </summary>
    /// <remarks>
    /// Reflection is used instead of a binary deep copy
    /// for security reasons. <br/>
    /// In fact, the MSDN binary formatter will gradually be
    /// removed in following .NET releases
    /// </remarks>
    public static class SystemExtension
    {
        /// <summary>
        /// Perform a deep copy of the object via serialization
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The deep copy of the object.</returns>
        public static T DeepCopy<T>(this T source)
        {
            try
            {
                object copy = source.Copy();
                return (T)copy;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return default;
            }
        }
    }
}
