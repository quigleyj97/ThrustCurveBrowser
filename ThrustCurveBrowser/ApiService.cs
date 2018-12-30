using System;
using System.Threading.Tasks;
using System.Net.Http;

namespace ThrustCurveBrowser
{
    public static class ApiService
    {
        private static readonly string BaseUrl = "http://www.thrustcurve.org/servlets/";
        private static readonly string SearchEndpoint = BaseUrl + "search/";
        private static readonly string DownloadEndpoint = BaseUrl + "download/";
        private static readonly string MetadataEndpoint = BaseUrl + "metadata/";

        private static readonly HttpClient client = new HttpClient();

        public static async void TestConnection()
        {
            HttpResponseMessage response = await client.GetAsync("http://www.thrustcurve.org");
            response.EnsureSuccessStatusCode();
            return;
        }
    }
}