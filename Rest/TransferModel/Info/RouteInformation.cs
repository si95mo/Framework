namespace Rest.TransferModel.Info
{
    public class RouteInformation
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
    }
}
