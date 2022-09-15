using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Diagnostic.Report
{
    /// <summary>
    /// Provide html-related extension methods
    /// </summary>
    public static class HtmlExtensionMethods
    {
        /// <summary>
        /// Convert an <see cref="IEnumerable{T}"/> to an html table
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="enumerable">The <see cref="IEnumerable{T}"/></param>
        /// <param name="propertiesToIncludeAsColumns">The properties to include as columns</param>
        /// <returns></returns>
        public static string ToHtmlTable<T>(this IEnumerable<T> enumerable, string propertiesToIncludeAsColumns = "")
        {
            return ToHtmlTable(enumerable, string.Empty, string.Empty, string.Empty, string.Empty, propertiesToIncludeAsColumns);
        }

        /// <summary>
        /// Convert an <see cref="IEnumerable{T}"/> to an html table
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="enumerable">The <see cref="IEnumerable{T}"/></param>
        /// <param name="tableSyle">The table style</param>
        /// <param name="headerStyle">The header style</param>
        /// <param name="rowStyle">The row style</param>
        /// <param name="alternateRowStyle">The alternate row style</param>
        /// <param name="propertiesToIncludeAsColumns">The properties to include as columns</param>
        /// <returns></returns>
        public static string ToHtmlTable<T>(this IEnumerable<T> enumerable, string tableSyle, string headerStyle, string rowStyle, string alternateRowStyle, string propertiesToIncludeAsColumns = "")
        {
            StringBuilder result = new StringBuilder();

            if (string.IsNullOrEmpty(tableSyle))
                result.Append("<table id=\"" + typeof(T).Name + "Table\">");
            else
                result.Append(Convert.ToString("<table id=\"" + typeof(T).Name + "Table\" class=\"") + tableSyle + "\">");

            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (string.IsNullOrEmpty(propertiesToIncludeAsColumns) || propertiesToIncludeAsColumns.Contains(property.Name + ","))
                {
                    if (string.IsNullOrEmpty(headerStyle))
                        result.AppendFormat("<th>{0}</th>", property.Name);
                    else
                        result.AppendFormat("<th class=\"{0}\">{1}</th>", headerStyle, property.Name);
                }
            }

            for (int i = 0; i <= enumerable.Count() - 1; i++)
            {
                if (!string.IsNullOrEmpty(rowStyle) && !string.IsNullOrEmpty(alternateRowStyle))
                    result.AppendFormat("<tr class=\"{0}\">", i % 2 == 0 ? rowStyle : alternateRowStyle);
                else
                    result.AppendFormat("<tr>");

                foreach (PropertyInfo property in properties)
                {
                    if (string.IsNullOrEmpty(propertiesToIncludeAsColumns) || propertiesToIncludeAsColumns.Contains(property.Name + ","))
                    {
                        object value = property.GetValue(enumerable.ElementAt(i), null);
                        result.AppendFormat("<td>{0}</td>", value ?? string.Empty);
                    }
                }
                result.AppendLine("</tr>");
            }

            result.Append("</table>");

            return result.ToString();
        }
    }
}