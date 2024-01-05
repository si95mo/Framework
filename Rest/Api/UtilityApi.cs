using Nancy;
using Newtonsoft.Json;
using Rest.TransferModel.Utility;

namespace Rest.Api
{
    /// <summary>
    /// Provides basic REST calls 
    /// </summary>
    public class UtilityApi : NancyModule
    {
        public UtilityApi()
        {
            Get("utility/echo/{value}", args =>
                {
                    EchoInformation echo = new EchoInformation(args.value);
                    string json = JsonConvert.SerializeObject(echo);

                    return json;
                }
            );
        }
    }
}
