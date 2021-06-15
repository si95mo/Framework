using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardware
{
    public abstract class Resource : IResource
    {
        protected string code;
        protected Dictionary<string, IChannel> channels;

        /// <summary>
        /// The code of the <see cref="Resource"/>
        /// </summary>
        public string Code
        {
            get => code;
        }
    }
}
