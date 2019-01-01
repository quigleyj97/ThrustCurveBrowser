using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.Net.Http;
using ThrustCurveBrowser.Models;
using System.IO;
using System.Xml;

namespace ThrustCurveBrowser
{
    public static class ApiService
    {
        private static readonly string BaseUrl = "http://www.thrustcurve.org/servlets/";
        private static readonly string SearchEndpoint = BaseUrl + "search";
        private static readonly string DownloadEndpoint = BaseUrl + "download";
        private static readonly string MetadataEndpoint = BaseUrl + "metadata";

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

        public static async Task<searchresponseResult[]> SearchMotors(searchrequest criteria)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(searchrequest));
            StringContent requestBody;
            using (StringWriter stringWriter = new StringWriter())
            using (XmlWriter writer = XmlWriter.Create(stringWriter, new XmlWriterSettings() { OmitXmlDeclaration = true }))
            {
                serializer.Serialize(writer, criteria);
                requestBody = new StringContent(stringWriter.ToString());
            }

            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync(SearchEndpoint, requestBody);
                response.EnsureSuccessStatusCode();
            }

            catch (HttpRequestException err)
            {
                Console.WriteLine("Failed to query ThrustCurve API");
                Console.WriteLine(err);
                return new searchresponseResult[] { };
            }
            Stream responseBody = await response.Content.ReadAsStreamAsync();
            XmlSerializer deserializer = new XmlSerializer(typeof(searchresponse), "http://www.thrustcurve.org/2015/SearchResponse");
            try
            {
                searchresponse responseModel = deserializer.Deserialize(responseBody) as searchresponse;
                return responseModel.results;
            }
            catch (Exception err)
            {
                Console.Error.WriteLine("Failed to deserialize API response");
                Console.Error.WriteLine(err);
                responseBody.Seek(0, SeekOrigin.Begin);
                Console.WriteLine(new StreamReader(responseBody).ReadToEnd());
                return new searchresponseResult[] { };
            }
            
        }
    }
}