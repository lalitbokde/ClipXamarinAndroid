using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WeClip.Core.Common
{
    public class HttpCall
    {
        public async Task<List<T>> GetAll<T>(string url)
        {
            var lst = new List<T>();
            using (var client = new HttpClient(new NativeMessageHandler()))
            {

                client.BaseAddress = new Uri(GlobalClass.BaseUrl);
                if (!string.IsNullOrEmpty(GlobalClass.AccessToken))
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GlobalClass.AccessToken);
                }
                else
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
               
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + "" + url).Result;
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<T>>(content);

                }
            }
            return lst;
        }

        public async Task<T> Get<T>(string url)
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                client.BaseAddress = new Uri(GlobalClass.BaseUrl);
                if (!string.IsNullOrEmpty(GlobalClass.AccessToken))
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GlobalClass.AccessToken);
                }

                else
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + url).Result;
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(content);
                }
            }
            return default(T);
        }

        public async Task<JsonResult> Post<T>(T obj, string url)
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

        public async Task<JsonResult> Put<T>(T obj, string url)
        {
            JsonResult resp = new JsonResult { Success = false, Message = "" };
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                client.BaseAddress = new Uri(GlobalClass.BaseUrl);
                var json = JsonConvert.SerializeObject(obj);
                var sendContent = new StringContent(json, Encoding.UTF8, "application/json");
                string AccessToken = GlobalClass.AccessToken;
                if (!string.IsNullOrEmpty(AccessToken))
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);
                }
              
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PutAsync(client.BaseAddress + url, sendContent);
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

        public async Task<JsonResult> PutWithoutContent(string url)
        {
            JsonResult resp = new JsonResult { Success = false, Message = "" };
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                client.BaseAddress = new Uri(GlobalClass.BaseUrl);
                string AccessToken = GlobalClass.AccessToken;
                if (!string.IsNullOrEmpty(AccessToken))
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);
                }
                else
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PutAsync(client.BaseAddress + url, null);
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

        public async Task<JsonResult> Delete<T>(string url)
        {
            JsonResult resp = new JsonResult { Success = false, Message = "" };
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                client.BaseAddress = new Uri(GlobalClass.BaseUrl);
                if (!string.IsNullOrEmpty(GlobalClass.AccessToken))
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GlobalClass.AccessToken);
                }

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.DeleteAsync(client.BaseAddress + url).Result;
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
