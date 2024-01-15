using Rest.TransferModel.Info;
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
                "  font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', " +
                "       Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif, sans-serif;\r\n" +
                "  border-collapse: collapse;\r\n" +
                "  width: 100%;\r\n" +
                "}\r\n" +
                "\r\n" +
                "td, th {\r\n" +
                "  border: 1px solid #dddddd;\r\n" +
                "  text-align: left;\r\n" +
                "  color: #f0f0f0ff;\r\n" +
                "  padding: 16px;\r\n" +
                "}\r\n" +
                "\r\n" +
                "tr:nth-child(even) {\r\n" +
                "  background-color: #2e2e2eff;\r\n" +
                "}\r\n" +
                "tr:nth-child(odd) {\r\n" +
                "  background-color: #121212ff;\r\n" +
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
            html.Append($"<h2 style=\"text-align: center; color: #dc2f2fff;background-color: #2e2e2e2e;\">{title}</h2>");
            return html;
        }

        /// <summary>
        /// Add a table to an existing html document
        /// </summary>
        /// <param name="html">The existing html <see cref="StringBuilder"/></param>
        /// <param name="modules">The table headers</param>
        /// <param name="routes">The table contents</param>
        /// <returns>The updated <paramref name="html"/> <see cref="StringBuilder"/></returns>
        public static StringBuilder AddTable(StringBuilder html, IEnumerable<ModuleInformation> modules, IEnumerable<IEnumerable<RouteInformation>> routes)
        {
            html.Append("<table>\r\n");

            int counter = 0;
            foreach (ModuleInformation module in modules)
            {
                html.Append($"  <tr>\r\n    <th>{module}</th>\r\n");
                html.Append($"  </tr>\r\n  <tr>\r\n");

                IEnumerable<RouteInformation> moduleRoutes = routes.Skip(counter++).FirstOrDefault();
                foreach (RouteInformation route in moduleRoutes)
                {
                    html.Append($"    <td>{route}</td>\r\n");
                }

                html.Append($"  </tr>\r\n");
            }

            html.Append("</table>\r\n");

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
