using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardware.Opc.Ua
{
    public interface IOpcUaChannel : IProperty<double>
    {
        /// <summary>
        /// The <see cref="IOpcUaChannel"/> namespace configuration
        /// (e.g. ns=2;s=Temperature)
        /// </summary>
        string NamespaceConfiguration { get; set; }
            
    }
}
