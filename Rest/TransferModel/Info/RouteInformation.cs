using Nancy.Routing;

namespace Rest.TransferModel.Info
{
    public class RouteInformation : Information
    {
        public string Method { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;

        public RouteInformation()
        { }

        public RouteInformation(string method, string path)
        {
            Method = method;
            Path = path;
        }

        public RouteInformation(Route route) : this(route.Description.Method, route.Description.Path)
        { }

        public override string ToString()
        {
            string description = $"{Method}\t{Path}";
            return description;
        }
    }
}
