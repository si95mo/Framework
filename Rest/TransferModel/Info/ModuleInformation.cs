using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rest.TransferModel.Info
{
    public class ModuleInformation
    {
        public string Name { get; set; } = string.Empty;
        public RouteInformation[] Routes { get; set; } = Enumerable.Empty<RouteInformation>().ToArray();

        public ModuleInformation()
        { }

        public ModuleInformation(string name, RouteInformation[] routes)
        {
            Name = name;
            Routes = routes;
        }
    }
}
