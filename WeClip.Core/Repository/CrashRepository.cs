using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WeClip.Core.Common;
using WeClip.Core.Model;

namespace WeClip.Core.Repository
{
    public class CrashRepository
    {

        public async Task<JsonResult> SendCrashReport(CrashReportModel ev)
        {
            JsonResult resp = new JsonResult { Success = false, Message = "" };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalClass.BaseUrl);
                var json = JsonConvert.SerializeObject(ev);

                var sendContent = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = null;
                try
                {
                    response = await client.PostAsync(client.BaseAddress + "Log/CrashReport", sendContent);
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