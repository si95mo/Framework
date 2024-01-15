using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rest
{
    internal static class Helpers
    {
        /// <summary>
        /// Create the html document document for a table
        /// </summary>
        /// <returns>The created html <see cref="StringBuilder"/></returns>
        public static StringBuilder CreateHtmlDocument()
        {
            StringBuilder html = new StringBuilder(
                "<!DOCTYPE html>\r\n" +
                "<html>\r\n" +
                "<head>\r\n" +
                "<style>\r\n" +
                "table {\r\n" +
                "  font-family: arial, sans-serif;\r\n" +
                "  border-collapse: collapse;\r\n" +
                "  width: 100%;\r\n" +
                "}\r\n" +
                "\r\n" +
                "td, th {\r\n" +
                "  border: 1px solid #dddddd;\r\n" +
                "  text-align: left;\r\n" +
                "  padding: 16px;\r\n" +
                "}\r\n" +
                "\r\n" +
                "tr:nth-child(even) {\r\n" +
                "  background-color: #dddddd;\r\n" +
                "}\r\n" +
                "</style>\r\n" +
                "</head>\r\n" +
                "<body>\r\n"
            );
            return html;
        }

        /// <summary>
        /// Add a title to an existing html <see cref="string"/>
        /// </summary>
        /// <param name="html">The existing html <see cref="StringBuilder"/></param>
        /// <param name="title">The title</param>
        /// <returns>The updated <paramref name="html"/> <see cref="StringBuilder"/></returns>
        public static StringBuilder AddTitle(StringBuilder html, string title)
        {
            html.Append($"<h2>{title}</h2>\r\n");
            return html;
        }

        /// <summary>
        /// Add a table to an existing html document
        /// </summary>
        /// <param name="html">The existing html <see cref="StringBuilder"/></param>
        /// <param name="headers">The table headers</param>
        /// <param name="contents">The table contents</param>
        /// <returns>The updated <paramref name="html"/> <see cref="StringBuilder"/></returns>
        public static StringBuilder AddTable(StringBuilder html, IEnumerable<string> headers, IEnumerable<IEnumerable<string>> contents)
        {
            html.Append("<table>\r\n  <tr>\r\n");
            foreach (string header in headers)
            {
                html.Append($"    <th>{header}</th>\r\n");
            }
            foreach (IEnumerable<string> content in contents)
            {
                html.Append("  </tr>\r\n  <tr>\r\n");

                foreach (string item in content)
                {
                    html.Append($"    <td>{item}</td>\r\n");
                }
            }
            html.Append("  </tr>\r\n</table>\r\n");

            return html;
        }

        /// <summary>
        /// Close the html document
        /// </summary>
        /// <param name="html">The html <see cref="StringBuilder"/></param>
        /// <returns>The entire html document as <see cref="string"/></returns>
        public static string CloseHtmlDocument(StringBuilder html)
        {
            html.Append("\r\n</body>\r\n</html>");
            return html.ToString();
        }
    }
}
