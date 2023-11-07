using Diagnostic;
using IO;
using Newtonsoft.Json;
using OX.Copyable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        /// <summary>
        /// Define weather an <see cref="object"/> is numeric or not
        /// </summary>
        /// <param name="source">The <see cref="object"/></param>
        /// <returns><see langword="true"/> if the <see cref="object"/> is numeric, <see langword="false"/> otherwise</returns>
        public static bool IsNumeric(this object source)
        {
            Type type = source.GetType();
            return type.IsNumeric();
        }

        /// <summary>
        /// Convert a pascal case <see cref="string"/> to an sentence string (i.e. PascalCase -> Pascal case)
        /// </summary>
        /// <param name="source">The <see cref="string"/> to convert</param>
        /// <returns>The converted <see cref="string"/></returns>
        public static string FromPascalToSentenceCase(this string source)
            => Regex.Replace(source, "[a-z][A-Z]", (x) => $"{x.Value[0]} {char.ToLower(x.Value[1])}");

        /// <summary>
        /// Convert a <see cref="string"/> to a pascal case string (i.e. PascalCase)
        /// </summary>
        /// <param name="source">The <see cref="string"/> to convert</param>
        /// <returns>The converted <see cref="string"/></returns>
        public static string ToPascalCase(this string source)
        {
            Regex invalidChars = new Regex("[^_a-zA-Z0-9]");
            Regex whiteSpace = new Regex(@"(?<=\s)");
            Regex startsWithLowerCaseChar = new Regex("^[a-z]");
            Regex firstCharFollowedByUpperCasesOnly = new Regex("(?<=[A-Z])[A-Z0-9]+$");
            Regex lowerCaseNextToNumber = new Regex("(?<=[0-9])[a-z]");
            Regex upperCaseInside = new Regex("(?<=[A-Z])[A-Z]+?((?=[A-Z][a-z])|(?=[0-9]))");

            IEnumerable<string> pascalCase = invalidChars.Replace(whiteSpace.Replace(source, "_"), string.Empty) // White spaces with _, invalid chars with ""
                .Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries) // Underscores
                .Select(w => startsWithLowerCaseChar.Replace(w, m => m.Value.ToUpper())) // First letter to uppercase
                .Select(w => firstCharFollowedByUpperCasesOnly.Replace(w, m => m.Value.ToLower())) // Other letters to lower case
                .Select(w => lowerCaseNextToNumber.Replace(w, m => m.Value.ToUpper())) // Upper case in case of numbers
                .Select(w => upperCaseInside.Replace(w, m => m.Value.ToLower())); // Lower case bwetween upper case

            string result = string.Concat(pascalCase);
            return result;
        }

        private const string Indentation = "    ";

        /// <summary>
        /// Format a <see cref="string"/> with json contents, beautifying it
        /// </summary>
        /// <param name="source">The <see cref="string"/> to format</param>
        /// <returns>The formatted <see cref="string"/></returns>
        public static string FormatJson(this string source)
        {
            int indent = 0;
            bool quoted = false;
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < source.Length; i++)
            {
                char character = source[i];
                switch (character)
                {
                    case '{':
                    case '[':
                        stringBuilder.Append(character);

                        if (!quoted)
                        {
                            stringBuilder.AppendLine();
                            Enumerable.Range(0, ++indent).ForEach((x) => stringBuilder.Append(Indentation));
                        }
                        break;

                    case '}':
                    case ']':
                        if (!quoted)
                        {
                            stringBuilder.AppendLine();
                            Enumerable.Range(0, --indent).ForEach((x) => stringBuilder.Append(Indentation));
                        }

                        stringBuilder.Append(character);
                        break;

                    case '"':
                        stringBuilder.Append(character);
                        bool escaped = false;
                        int index = i;

                        while (index > 0 && source[--index] == '\\')
                            escaped = !escaped;

                        if (!escaped)
                            quoted = !quoted;
                        break;

                    case ',':
                        stringBuilder.Append(character);
                        if (!quoted)
                        {
                            stringBuilder.AppendLine();
                            Enumerable.Range(0, indent).ForEach((x) => stringBuilder.Append(Indentation));
                        }
                        break;

                    case ':':
                        stringBuilder.Append(character);
                        if (!quoted)
                            stringBuilder.Append(" ");
                        break;

                    default:
                        stringBuilder.Append(character);
                        break;
                }
            }

            string formattedSource = stringBuilder.ToString();
            return formattedSource;
        }

        private static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
                action(item);
        }
    }
}