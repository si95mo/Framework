using Accord.IO;
using MatFileHandler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.File
{
    public class MAT
    {
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
