using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeClip.Core.Common;
using WeClip.Core.Model;
using WeClip.Core.Service;

namespace WeClip.Core.Repository
{
    public class AccountRepository
    {
        HttpCall call;
        CrashReportService crashReport = new CrashReportService();
        CrashReportModel CR = new CrashReportModel();

        public AccountRepository()
        {
            call = new HttpCall();
        }

        public async Task<Token> Login(LoginModel model)
        {

            Token token = null;
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(model);
                var sendContent = new StringContent(json, Encoding.UTF8, "application/json");

                client.BaseAddress = new Uri(GlobalClass.BaseUrl);

                HttpResponseMessage response = null;
                try
                {
                    response = await client.PostAsync(client.BaseAddress + "Account/Login", sendContent);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<Token>(content);
                    }
                }
                catch (System.Exception ex)
                {
                    token.access_token = "";
                    CR.Filename = "AccountRepository";
                    CR.Eventname = "Login";
                    CR.UserID = "0";
                    CR.ErrorMsg = ex.Message + ex.StackTrace;
                    await crashReport.PostCrashReport(CR);
                }
                return token;
            }
        }

        public async Task<JsonResult> NotificationSetting(string url)
        {
            JsonResult resp = null;
            try
            {
                resp = await call.Post("", url);
                return resp;
            }
            catch (Exception ex)
            {
                CR.Filename = "AccountRepository";
                CR.Eventname = "NotificationSetting";
                CR.UserID = "0";
                CR.ErrorMsg = ex.Message + ex.StackTrace;
                await crashReport.PostCrashReport(CR);
                return resp;
            }
        }

        public async Task<JsonResult> CreateAccount(RegistrationModel model, string url)
        {
            JsonResult resp = null;
            try
            {
                resp = await call.Post<RegistrationModel>(model, url);
                return resp;
            }
            catch (Exception ex)
            {
                CR.Filename = "AccountRepository";
                CR.Eventname = "CreateAccount";
                CR.UserID = "0";
                CR.ErrorMsg = ex.Message + ex.StackTrace;
                await crashReport.PostCrashReport(CR);
                return resp;
            }
        }

        public async Task<UserProfile> GetUserProfile(string url)
        {
            UserProfile user = null;
            try
            {
                user = await call.Get<UserProfile>(url);
                return user;
            }
            catch (Exception ex)
            {
                CR.Filename = "CreateAccount";
                CR.Eventname = "GetUserProfile";
                CR.UserID = "0";
                CR.ErrorMsg = ex.Message + ex.StackTrace;
                await crashReport.PostCrashReport(CR);
                return user;
            }
        }

        public async Task<JsonResult> UpdateDeviceID(UserDeviceInfo ud, string url)
        {
            JsonResult resp = null;
            try
            {
                resp = await call.Put<UserDeviceInfo>(ud, url);
                return resp;
            }
            catch (Exception ex)
            {
                CR.Filename = "CreateAccount";
                CR.Eventname = "UpdateDeviceID";
                CR.UserID = "0";
                CR.ErrorMsg = ex.Message + ex.StackTrace;
                await crashReport.PostCrashReport(CR);
                return resp;
            }
        }

        public async Task<JsonResult> SetProfile(UserProfile model, string url)
        {
            JsonResult resp = null;
            try
            {
                resp = await call.Post<UserProfile>(model, url);
                return resp;
            }
            catch (Exception ex)
            {
                CR.Filename = "AccountRepository";
                CR.Eventname = "SetProfile";
                CR.UserID = "0";
                CR.ErrorMsg = ex.Message + ex.StackTrace;
                await crashReport.PostCrashReport(CR);
                return resp;
            }
        }

    }
}
