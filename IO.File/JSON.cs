using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace IO.File
{
    /// <summary>
    /// Handles basic IO operation for JSON files
    /// </summary>
    public class JSON
    {
        /// <summary>
        /// Read a JSON file from disk
        /// </summary>
        /// <param name="path">The file to read path</param>
        /// <returns>The <see cref="JObject"/> relative to the read file</returns>
        public static JObject ReadJSON(string path)
        {
            JObject json = JObject.Parse(System.IO.File.ReadAllText(path));

            return json;
        }

        /// <summary>
        /// Save a JSON file to disk
        /// </summary>
        /// <param name="obj">The object to serialize and save to file</param>
        /// <param name="path">The file path</param>
        public static void SaveJSON(object obj, string path)
        {
            System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(obj));

            // serialize JSON directly to a file
            using (StreamWriter file = System.IO.File.CreateText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, obj);
            }
        }

        /// <summary>
        /// Save a <see cref="JObject"/> to disk.
        /// </summary>
        /// <param name="obj">The <see cref="JObject"/> to save</param>
        /// <param name="path">The json file path</param>
        public static void SaveJSON(JObject obj, string path)
        {
            System.IO.File.WriteAllText(path, obj.ToString());
        }
    }
}