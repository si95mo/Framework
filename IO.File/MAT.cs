using MatFileHandler;
using System.IO;

namespace IO.File
{
    /// <summary>
    /// Handle basic IO operation for (Matlab) MAT files. <br/> 
    /// See also <see cref="MatFileHandler"/> and relative data structures
    /// (like <see cref="IStructureArray"/> or <see cref="IArrayOf{T}"/>)
    /// </summary>
    public class MAT
    {
        /// <summary>
        /// Read a struct from a MAT file
        /// </summary>
        /// <param name="path">The path of the MAT file to read</param>
        /// <param name="structName">The struct name</param>
        /// <returns>The <see cref="IStructureArray"/> with the data</returns>
        /// <example> 
        /// This example show the usage of the method.
        /// <code>
        /// IStructureArray data = MAT.ReadStructFromFile(path, "data");
        /// dynamic dynamicData = new ExpandoObject();
        /// dynamicData.time = data["time"];
        /// 
        /// IArrayOf<double> time = data.time as IArrayOf<double>;
        /// </code>
        /// </example>
        public static IStructureArray ReadStructFromFile(string path, string structName)
        {
            IMatFile matFile;
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                var reader = new MatFileReader(fileStream);
                matFile = reader.Read();
            }

            IStructureArray data = matFile[structName].Value as IStructureArray;

            return data;
        }
    }
}