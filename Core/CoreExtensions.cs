using System;
using System.Reflection;

namespace Core.Extensions
{
    public static class CoreExtensions
    {
        /// <summary>
        /// Clear all the <see langword="event"/> handlers associated with the instance
        /// </summary>
        /// <param name="source">The source <see cref="object"/></param>
        /// <param name="eventName">The event name to delete</param>
        public static void ClearEventInvocations(this object source, string eventName)
        {
            var fi = source.GetType().GetEventField(eventName);
            if (fi == null)
            {
                return;
            }

            fi.SetValue(source, null);
        }

        /// <summary>
        /// Get the <see langword="event"/> field information
        /// </summary>
        /// <param name="source">The source <see cref="Type"/></param>
        /// <param name="eventName">The <see langword="event"/> name</param>
        /// <returns>The retrieved <see cref="FieldInfo"/></returns>
        private static FieldInfo GetEventField(this Type source, string eventName)
        {
            FieldInfo field = null;
            while (source != null)
            {
                // Find events defined as field
                field = source.GetField(eventName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
                if (field != null && (field.FieldType == typeof(MulticastDelegate) || field.FieldType.IsSubclassOf(typeof(MulticastDelegate))))
                {
                    break;
                }

                // Find events defined as property { add; remove; }
                field = source.GetField("EVENT_" + eventName.ToUpper(), BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
                if (field != null)
                {
                    break;
                }

                source = source.BaseType;
            }

            return field;
        }
    }
}
