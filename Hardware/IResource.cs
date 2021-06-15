using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardware
{
    public interface IResource
    {
        /// <summary>
        /// The code of the <see cref="IResource"/>
        /// </summary>
        string Code
        { get; }
    }
}
