using System.Linq;

namespace Rest.TransferModel.Info
{
    public class ModuleInformation : Information
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

        public override string ToString()
            => Name;
    }
}
