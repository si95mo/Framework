﻿using Diagnostic;
using IO;
using OX.Copyable;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

        /// <summary>
        /// Serialize an <see cref="object"/> to a json file
        /// </summary>
        /// <param name="source">The object to serialize</param>
        /// <returns>The (sync) <see cref="Task"/> with serialization result</returns>
        public static async Task<string> Serialize(this object source)
            => await Task.Factory.StartNew(() => JsonConvert.SerializeObject(source, Formatting.Indented));

        /// <summary>
        /// Deserialize a json <see cref="string"/> into an <see cref="object"/> of type <see cref="T"/>
        /// </summary>
        /// <typeparam name="T">The type of the deserialization</typeparam>
        /// <param name="source">The source to deserialize</param>
        /// <returns>The deserialized <see cref="object"/></returns>
        public static async Task<T> Deserialize<T>(this string source)
            => await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(source));

        /// <summary>
        /// Define weather a <see cref="Type"/> is numeric or not
        /// </summary>
        /// <param name="type">The <see cref="Type"/></param>
        /// <returns><see langword="true"/> if the <see cref="Type"/> is numeric, <see langword="false"/> otherwise</returns>
        public static bool IsNumeric(this Type type)
        {
            bool isNumeric = false;

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    isNumeric = true;
                    break;
            }

            return isNumeric;
        }
    }
}