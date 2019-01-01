using System;
using System.Collections.Generic;
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

        public static async Task<bool> TestConnection()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("http://www.thrustcurve.org");
                return response.IsSuccessStatusCode;
            }
            catch(Exception err)
            {
                Console.Error.WriteLine("Failed to query ThrustCurve homepage");
                Console.Error.WriteLine(err);
            }
            return false;
        }
    }
}