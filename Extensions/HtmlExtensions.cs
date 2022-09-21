using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Extensions
{
    /// <summary>
    /// Provide html-related extension methods
    /// </summary>
    public static class HtmlExtensions
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
            string htmlTable = enumerable.ToHtmlTable(string.Empty, string.Empty, string.Empty, string.Empty, propertiesToIncludeAsColumns);
            return htmlTable;
        }

        /// <summary>
        /// Convert an <see cref="IEnumerable{T}"/> to an html table
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="enumerable">The <see cref="IEnumerable{T}"/></param>
        /// <param name="tableStyle">The table style</param>
        /// <param name="headerStyle">The header style</param>
        /// <param name="rowStyle">The row style</param>
        /// <param name="alternateRowStyle">The alternate row style</param>
        /// <param name="propertiesToIncludeAsColumns">The properties to include as columns</param>
        /// <returns></returns>
        public static string ToHtmlTable<T>(this IEnumerable<T> enumerable, string tableStyle, string headerStyle, string rowStyle, string alternateRowStyle, string propertiesToIncludeAsColumns = "")
        {
            StringBuilder htmlTable = new StringBuilder();

            if (string.IsNullOrEmpty(tableStyle))
                htmlTable.Append("<table id=\"" + typeof(T).Name + "Table\">");
            else
                htmlTable.Append(Convert.ToString("<table id=\"" + typeof(T).Name + "Table\" class=\"") + tableStyle + "\">");

            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (string.IsNullOrEmpty(propertiesToIncludeAsColumns) || propertiesToIncludeAsColumns.Contains(property.Name + ","))
                {
                    if (string.IsNullOrEmpty(headerStyle))
                        htmlTable.AppendFormat("<th>{0}</th>", property.Name);
                    else
                        htmlTable.AppendFormat("<th class=\"{0}\">{1}</th>", headerStyle, property.Name);
                }
            }

            for (int i = 0; i <= enumerable.Count() - 1; i++)
            {
                if (!string.IsNullOrEmpty(rowStyle) && !string.IsNullOrEmpty(alternateRowStyle))
                    htmlTable.AppendFormat("<tr class=\"{0}\">", i % 2 == 0 ? rowStyle : alternateRowStyle);
                else
                    htmlTable.AppendFormat("<tr>");

                foreach (PropertyInfo property in properties)
                {
                    if (string.IsNullOrEmpty(propertiesToIncludeAsColumns) || propertiesToIncludeAsColumns.Contains(property.Name + ","))
                    {
                        object value = property.GetValue(enumerable.ElementAt(i), null);
                        htmlTable.AppendFormat("<td>{0}</td>", value ?? string.Empty);
                    }
                }
                htmlTable.AppendLine("</tr>");
            }
            htmlTable.Append("</table>");

            return htmlTable.ToString();
        }
    }
}