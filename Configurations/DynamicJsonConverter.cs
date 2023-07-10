using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

/// <summary>
/// Define a converter from json to a <see langword="dynamic"/> <see cref="object"/>
/// </summary>
public sealed class DynamicJsonConverter : JavaScriptConverter
{
    /// <summary>
    /// Deserialize an <see cref="IDictionary"/>
    /// </summary>
    /// <param name="dictionary">The dictionary to deserialize</param>
    /// <param name="type">The type of the <paramref name="dictionary"/> values</param>
    /// <param name="serializer">The serializer</param>
    /// <returns>The deserialized object</returns>
    /// <exception cref="ArgumentException"/>
    public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
    {
        if (dictionary == null)
            throw new ArgumentNullException("dictionary");

        return type == typeof(object) ? new DynamicJsonObject(dictionary) : null;
    }

    /// <summary>
    /// Serialize an <see cref="object"/> to json
    /// </summary>
    /// <param name="obj">The object to serialize</param>
    /// <param name="serializer">The serializer</param>
    /// <returns>The serialized object</returns>
    /// <remarks>Not yet implemented! Throws a <see cref="NotImplementedException"/></remarks>
    /// <exception cref="NotImplementedException"/>
    public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override IEnumerable<Type> SupportedTypes
    {
        get { return new ReadOnlyCollection<Type>(new List<Type>(new[] { typeof(object) })); }
    }

    #region Nested type: DynamicJsonObject

    private sealed class DynamicJsonObject : DynamicObject
    {
        private readonly IDictionary<string, object> dictionary;

        public DynamicJsonObject(IDictionary<string, object> dictionary)
        {
            this.dictionary = dictionary ?? throw new ArgumentNullException("dictionary");
        }

        public override string ToString()
        {
            var sb = new StringBuilder("{");
            ToString(sb);

            return sb.ToString();
        }

        private void ToString(StringBuilder sb)
        {
            bool firstInDictionary = true;
            foreach (var pair in dictionary)
            {
                if (!firstInDictionary)
                    sb.Append(",");

                firstInDictionary = false;
                object value = pair.Value;
                string name = pair.Key;

                if (value is string)
                    sb.AppendFormat("{0}:\"{1}\"", name, value);
                else if (value is IDictionary<string, object>)
                    new DynamicJsonObject((IDictionary<string, object>)value).ToString(sb);
                else if (value is ArrayList)
                {
                    sb.Append(name + ":[");
                    var firstInArray = true;
                    foreach (var arrayValue in (ArrayList)value)
                    {
                        if (!firstInArray)
                            sb.Append(",");

                        firstInArray = false;

                        if (arrayValue is IDictionary<string, object>)
                            new DynamicJsonObject((IDictionary<string, object>)arrayValue).ToString(sb);
                        else if (arrayValue is string)
                            sb.AppendFormat("\"{0}\"", arrayValue);
                        else
                            sb.AppendFormat("{0}", arrayValue);
                    }
                    sb.Append("]");
                }
                else
                    sb.AppendFormat("{0}:{1}", name, value);
            }
            sb.Append("}");
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!dictionary.TryGetValue(binder.Name, out result))
            {
                // return null to avoid exception.  caller can check for null this way...
                result = null;
                return true;
            }

            result = WrapResultObject(result);
            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes.Length == 1 && indexes[0] != null)
            {
                if (!dictionary.TryGetValue(indexes[0].ToString(), out result))
                {
                    // return null to avoid exception.  caller can check for null this way...
                    result = null;
                    return true;
                }

                result = WrapResultObject(result);
                return true;
            }

            return base.TryGetIndex(binder, indexes, out result);
        }

        private static object WrapResultObject(object result)
        {
            IDictionary<string, object> dictionary = result as IDictionary<string, object>;
            if (dictionary != null)
                return new DynamicJsonObject(dictionary);

            ArrayList arrayList = result as ArrayList;
            if (arrayList != null && arrayList.Count > 0)
            {
                return arrayList[0] is IDictionary<string, object> ?
                    new List<object>(arrayList.Cast<IDictionary<string, object>>().Select(x => new DynamicJsonObject(x))) : new List<object>(arrayList.Cast<object>());
            }

            return result;
        }
    }

    #endregion Nested type: DynamicJsonObject
}