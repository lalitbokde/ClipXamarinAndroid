using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WeClip.Core.Common
{
    public static class PostData
    {
        public static async Task<JsonResult> Post<T>(T obj, string url)
        {
            JsonResult resp = new JsonResult { Success = false, Message = "" };
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                client.BaseAddress = new Uri(GlobalClass.BaseUrl);

                var json = JsonConvert.SerializeObject(obj);
                var sendContent = new StringContent(json, Encoding.UTF8, "application/json");
                if (!string.IsNullOrEmpty(GlobalClass.AccessToken))
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GlobalClass.AccessToken);
                }
                else
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response = new HttpResponseMessage();
                try
                {
                    response = await client.PostAsync(client.BaseAddress + url, sendContent);
                }
                catch (Exception ex)
                {
                    resp.Message = ex.Message;
                }

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<JsonResult>(content);
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<JsonResult>(content);
                }
            }
        }

    }
}
