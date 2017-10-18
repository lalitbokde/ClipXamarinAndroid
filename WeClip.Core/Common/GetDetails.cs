using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WeClip.Core.Common
{
    public static class GetDetails
    {
        public static async Task<T> Get<T>(string url)
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

                var response = await client.GetAsync(client.BaseAddress + url).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(content);
                }
            }
            return default(T);
        }

        public static async Task<List<T>> GetAll<T>(string url)
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

                HttpResponseMessage response = await client.GetAsync(client.BaseAddress + "" + url).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<T>>(content);

                }
            }
            return lst;
        }
    }
}
