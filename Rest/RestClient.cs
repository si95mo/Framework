using Core;
using Diagnostic;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Rest
{
    /// <summary>
    /// Implement a REST client
    /// </summary>
    public class RestClient : IProperty
    {
        private string code;
        private string request;
        private HttpResponseMessage response;
        private string result;
        private Uri uri;
        private HttpClient client;

        /// <summary>
        /// The <see cref="RestClient"/> code
        /// </summary>
        public string Code => code;

        /// <summary>
        /// The <see cref="RestClient"/> value as <see cref="object"/>
        /// </summary>
        public object ValueAsObject
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        /// <summary>
        /// The <see cref="RestClient"/> <see cref="System.Type"/>
        /// </summary>
        public Type Type => typeof(RestClient);

        /// <summary>
        /// The <see cref="RestClient"/> request in the form of key=value
        /// </summary>
        public string Request
        {
            get => request;
            set => request = value;
        }

        /// <summary>
        /// The <see cref="RestClient"/> response
        /// </summary>
        public HttpResponseMessage Response
        {
            get => response;
            set => response = value;
        }

        /// <summary>
        /// The <see cref="RestClient"/> query result
        /// </summary>
        public string Result => result;

        /// <summary>
        /// The <see cref="System.Uri"/>
        /// </summary>
        public Uri Uri
        {
            get => uri;
            set => uri = value;
        }

        /// <summary>
        /// Create a new instance of <see cref="RestClient"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="uri">The <see cref="System.Uri"/></param>
        /// <param name="request">The request</param>
        public RestClient(string code, Uri uri, string request = "")
        {
            this.code = code;
            this.uri = uri;
            this.request = request;

            response = new HttpResponseMessage();
            result = "";

            client = new HttpClient();
        }

        /// <summary>
        /// Perform and async GET with the actual
        /// <see cref="Request"/>
        /// </summary>
        /// <returns>The GET result</returns>
        public async Task<string> Get()
        {
            string actualRequest = $"{uri}/{request}";

            try
            {
                response = await client.GetAsync(actualRequest);
                result = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }

            return result;
        }

        /// <summary>
        /// Perform and async POST with the actual
        /// <see cref="Request"/>
        /// </summary>
        /// <param name="data">The data to send (as json)</param>
        /// <returns>The POST result</returns>
        public async Task<string> Post(object data)
        {
            string json = JsonConvert.SerializeObject(data);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                response = await client.PostAsync(uri, content);
                result = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }

            return result;
        }
    }
}