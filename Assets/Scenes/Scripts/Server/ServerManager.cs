using System.Net.Http;
using System.Threading.Tasks;
using Unity.Plastic.Newtonsoft.Json;


namespace Assets.Scenes.Scripts
{
    public class ServerManager
    {
        private const string url = "https://localhost:7201/";
        public static async Task<T> GetRequest<T>(string request)
        {
            HttpClient httpClient = new HttpClient();
            using HttpResponseMessage response = await httpClient.GetAsync(url + request);
            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        public static async Task<HttpContent> PostRequest(string request)
        {
            HttpClient httpClient = new HttpClient();
            using HttpResponseMessage response = await httpClient.PostAsync(url + request, null);
            await response.Content.ReadAsStringAsync();
            return response.Content;
        }

        public static async Task<T> PostRequest<T>(string request)
        {
            HttpClient httpClient = new HttpClient();
            using HttpResponseMessage response = await httpClient.PostAsync(url + request, null);
            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }
        //public static async Task<AnswerType> PostRequest<ContentType, AnswerType>(string request, ContentType content)
        //{
        //    string answer = await (await PostRequest(request, content)).ReadAsStringAsync();
        //    return JsonConvert.DeserializeObject<AnswerType>(answer);
        //}

        //public static async Task<HttpContent> PostRequest<ContentType>(string request, ContentType content)
        //{
        //    HttpClient httpClient = new HttpClient();
        //    string json_content = JsonConvert.SerializeObject(content);
        //    HttpContent httpContent = new JsonContent(json_content);

        //    using HttpResponseMessage response = await httpClient.PostAsync(url + request, httpContent);
        //    return response.Content;
        //}
    }
}
