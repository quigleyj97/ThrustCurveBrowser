using System;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.Net.Http;
using ThrustCurveBrowser.Models;
using System.IO;
using System.Xml;

namespace ThrustCurveBrowser
{
    public class ApiService : IDisposable
    {
        private static readonly string BaseUrl = "http://www.thrustcurve.org/servlets/";
        private static readonly string SearchEndpoint = BaseUrl + "search";
        private static readonly string DownloadEndpoint = BaseUrl + "download";
        private static readonly string MetadataEndpoint = BaseUrl + "metadata";

        private readonly HttpClient client = new HttpClient();

        public void Dispose()
        {
            client.Dispose();
        }

        public async Task<bool> TestConnection()
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

        public async Task<searchresponseResult[]> SearchMotors(searchrequest criteria)
        {
            return (await MakeApiRequest<searchrequest, searchresponse>(criteria, SearchEndpoint)).results;
        }

        public async Task<downloadresponseResult[]> SearchDownloads(downloadrequest criteria)
        {
            return (await MakeApiRequest<downloadrequest, downloadresponse>(criteria, DownloadEndpoint)).results;
        }

        public Task<metadataresponse> GetSearchMetadata(metadatarequest criteria)
        {
            return MakeApiRequest<metadatarequest, metadataresponse>(criteria, MetadataEndpoint);
        }

        private async Task<U> MakeApiRequest<T, U>(T requestModel, string endpoint) where U : class, new()
        {
            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync(endpoint, BodyForRequest(requestModel));
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException err)
            {
                Console.WriteLine("Failed to query ThrustCurve metadata API");
                Console.WriteLine(err);
                return new U { };
            }
            Stream responseBody = await response.Content.ReadAsStreamAsync();
            return ModelFromRequest<U>(responseBody);
        }

        private HttpContent BodyForRequest<T>(T model)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringWriter stringWriter = new StringWriter())
            // Skip the XML encoding, since a.) that's implicit in HTTP and b.) XmlWriter thinks it's UTF-16
            using (XmlWriter writer = XmlWriter.Create(stringWriter, new XmlWriterSettings() { OmitXmlDeclaration = true }))
            {
                serializer.Serialize(writer, model);
                return new StringContent(stringWriter.ToString());
            }
        }

        private T ModelFromRequest<T>(Stream body) where T : class, new()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            try
            {
                return deserializer.Deserialize(body) as T;
            }
            catch (Exception err)
            {
                Console.Error.WriteLine("Failed to deserialize API response");
                Console.Error.WriteLine(err);
                body.Seek(0, SeekOrigin.Begin);
                Console.WriteLine(new StreamReader(body).ReadToEnd());
                return new T { };
            }
        }
    }
}