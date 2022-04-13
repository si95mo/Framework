using Diagnostic;
using IO;
using Newtonsoft.Json;
using OX.Copyable;
using System;
using System.Threading.Tasks;

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

        /// <summary>
        /// Serialize an <see cref="object"/> to a json file
        /// </summary>
        /// <param name="source">The object to serialize</param>
        /// <param name="path">The json file path</param>
        /// <returns>The (sync) <see cref="Task"/></returns>
        public static async Task Serialize(this object source, string path)
        {
            string json = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(source, Formatting.Indented));
            await FileHandler.SaveAsync(json, path);
        }
    }
}