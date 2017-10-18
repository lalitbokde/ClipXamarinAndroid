using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WeClip.Core.Common;
using WeClip.Core.Model;
using WeClip.Core.Service;

namespace WeClip.Core.Repository
{
    public class EventRepository
    {
        HttpCall call;
        CrashReportService crashReport = new CrashReportService();
        CrashReportModel CR = new CrashReportModel();


        public EventRepository()
        {
            call = new HttpCall();
        }

        public async Task<List<CategoryModel>> GetAllCategory(string url)
        {

            List<CategoryModel> lst = null;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(GlobalClass.BaseUrl);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + "" + url).Result;
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<CategoryModel>>(content);

                }
            }
            return lst;
        }

        public async Task<List<SubCategoryModel>> GetAllSubCategory(string url)
        {

            List<SubCategoryModel> lst = null;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(GlobalClass.BaseUrl);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + "" + url).Result;
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<SubCategoryModel>>(content);

                }
                return lst;
            }
        }

        public async Task<List<EventModel>> GetAllEvent(string url)
        {
            List<EventModel> lst = null;
            try
            {
                lst = await call.GetAll<EventModel>(url);
                return lst;
            }
            catch (Exception ex)
            {
                CR.Filename = "EventRepository";
                CR.Eventname = "GetAllEvent";
                CR.UserID = "0";
                CR.ErrorMsg = ex.Message + ex.StackTrace;
                await crashReport.PostCrashReport(CR);
                return lst;
            }
        }

        public async Task<List<EventFiles>> GetEventFileDetails(string url)
        {
            List<EventFiles> lst = null;
            try
            {
                lst = await call.GetAll<EventFiles>(url);
                return lst;
            }
            catch (Exception ex)
            {
                CR.Filename = "EventRepository";
                CR.Eventname = "GetEventFileDetails";
                CR.UserID = "0";
                CR.ErrorMsg = ex.Message + ex.StackTrace;
                await crashReport.PostCrashReport(CR);
                return lst;
            }
        }

        public async Task<JsonResult> SaveEvent(EventModel ev, string url)
        {
            JsonResult resp = null;
            try
            {
                resp = await call.Post<EventModel>(ev, url);
                return resp;
            }
            catch (Exception ex)
            {

                CR.Filename = "EventRepository";
                CR.Eventname = "SaveEvent";
                CR.UserID = "0";
                CR.ErrorMsg = ex.Message + ex.StackTrace;
                await crashReport.PostCrashReport(CR);
                return resp;
            }


        }

        public async Task<JsonResult> MakeEventadmin(List<EventAdmin> eventAdminList, string url)
        {
            JsonResult resp = null;
            try
            {
                resp = await call.Post<List<EventAdmin>>(eventAdminList, url);
                return resp;
            }
            catch (Exception ex)
            {

                CR.Filename = "EventRepository";
                CR.Eventname = "MakeEventadmin";
                CR.UserID = GlobalClass.UserID == null ? "0" : GlobalClass.UserID;
                CR.ErrorMsg = ex.Message + ex.StackTrace;
                await crashReport.PostCrashReport(CR);
                return resp;
            }
        }

        public async Task<JsonResult> SendPrivateEventNotification(List<PrivateEventNotification> ev, string url)
        {
            JsonResult resp = null;
            try
            {
                resp = await call.Post<List<PrivateEventNotification>>(ev, url);
                return resp;
            }
            catch (Exception ex)
            {
                CR.Filename = "EventRepository";
                CR.Eventname = "SendPrivateEventNotification";
                CR.UserID = GlobalClass.UserID == null ? "0" : GlobalClass.UserID;
                CR.ErrorMsg = ex.Message + ex.StackTrace;
                await crashReport.PostCrashReport(CR);
                return resp;
            }


        }

        public async Task<EventModel> GetEvent(string url)
        {

            EventModel lst = null;
            try
            {
                lst = await call.Get<EventModel>(url);
                return lst;
            }
            catch (Exception ex)
            {
                CR.Filename = "EventRepository";
                CR.Eventname = "GetEvent";
                CR.UserID = GlobalClass.UserID == null ? "0" : GlobalClass.UserID;
                CR.ErrorMsg = ex.Message + ex.StackTrace;
                await crashReport.PostCrashReport(CR);
                return lst;
            }
        }

        public async Task<JsonResult> UpdateEvent(EventModel ev, string url)
        {
            JsonResult resp = null;
            try
            {
                resp = await call.Put<EventModel>(ev, url);
                return resp;
            }
            catch (Exception ex)
            {
                CR.Filename = "EventRepository";
                CR.Eventname = "UpdateEvent";
                CR.UserID = GlobalClass.UserID == null ? "0" : GlobalClass.UserID;
                CR.ErrorMsg = ex.Message + ex.StackTrace;
                await crashReport.PostCrashReport(CR);
                return resp;
            }
        }

        public async Task<JsonResult> DeleteEvent(string url)
        {

            JsonResult lst = null;
            try
            {
                lst = await call.Delete<EventModel>(url);
                return lst;
            }
            catch (Exception ex)
            {
                CR.Filename = "EventRepository";
                CR.Eventname = "DeleteEvent";
                CR.UserID = GlobalClass.UserID == null ? "0" : GlobalClass.UserID;
                CR.ErrorMsg = ex.Message + ex.StackTrace;
                await crashReport.PostCrashReport(CR);
                return lst;
            }
        }

        public async Task<List<EventFeedModel>> GetAllFeed(string url)
        {

            List<EventFeedModel> lst = null;
            try
            {
                lst = await call.GetAll<EventFeedModel>(url);
                return lst;
            }
            catch (Exception ex)
            {
                CR.Filename = "EventRepository";
                CR.Eventname = "GetAllFeed";
                CR.UserID = GlobalClass.UserID == null ? "0" : GlobalClass.UserID;
                CR.ErrorMsg = ex.Message + ex.StackTrace;
                await crashReport.PostCrashReport(CR);
                return lst;
            }
        }

        public async Task<List<long?>> GetEventAdmin(string url)
        {

            List<long?> lst = null;
            try
            {
                lst = await call.GetAll<long?>(url);
                return lst;
            }
            catch (Exception ex)
            {
                CR.Filename = "EventRepository";
                CR.Eventname = "GetEventAdmin";
                CR.UserID = GlobalClass.UserID == null ? "0" : GlobalClass.UserID;
                CR.ErrorMsg = ex.Message + ex.StackTrace;
                await crashReport.PostCrashReport(CR);
                return lst;
            }
        }

        public async Task<JsonResult> PostFeed(EventFeedModel ev, string url)
        {
            JsonResult resp = null;
            try
            {
                resp = await call.Post<EventFeedModel>(ev, url);
                return resp;
            }
            catch (Exception ex)
            {
                CR.Filename = "EventRepository";
                CR.Eventname = "PostFeed";
                CR.UserID = GlobalClass.UserID == null ? "0" : GlobalClass.UserID;
                CR.ErrorMsg = ex.Message + ex.StackTrace;
                await crashReport.PostCrashReport(CR);
                return resp;
            }
        }

        public async Task<JsonResult> SetEventPicture(EventPictureModel ev, string url)
        {
            JsonResult resp = null;
            try
            {
                resp = await call.PutWithoutContent(url);
                return resp;
            }
            catch (Exception ex)
            {
                CR.Filename = "EventRepository";
                CR.Eventname = "SetEventPicture";
                CR.UserID = GlobalClass.UserID == null ? "0" : GlobalClass.UserID;
                CR.ErrorMsg = ex.Message + ex.StackTrace;
                await crashReport.PostCrashReport(CR);
                return resp;
            }
        }


        public async Task<JsonResult> RequestEventResponce(EventDetailsModel model, string url)
        {
            JsonResult resp = null;
            try
            {
                resp = await call.Put<EventDetailsModel>(model, url);
                return resp;
            }
            catch (Exception ex)
            {
                CR.Filename = "EventRepository";
                CR.Eventname = "RequestEventResponce";
                CR.UserID = GlobalClass.UserID == null ? "0" : GlobalClass.UserID;
                CR.ErrorMsg = ex.Message + ex.StackTrace;
                await crashReport.PostCrashReport(CR);
                return resp;
            }
        }

        public async Task<List<EventDetailsModel>> GetAllEventRequest(string url)
        {
            List<EventDetailsModel> eventRequest = null;
            try
            {
                eventRequest = await call.GetAll<EventDetailsModel>(url);
                return eventRequest;
            }
            catch (Exception ex)
            {
                CR.Filename = "EventRepository";
                CR.Eventname = "GetAllEventRequest";
                CR.UserID = GlobalClass.UserID == null ? "0" : GlobalClass.UserID;
                CR.ErrorMsg = ex.Message + ex.StackTrace;
                await crashReport.PostCrashReport(CR);
                return eventRequest;
            }
        }

        public async Task<WeClipSetting> GetWeClipLimit(string url)
        {
            WeClipSetting weClipSetting = null;
            try
            {
                weClipSetting = await call.Get<WeClipSetting>(url);
                return weClipSetting;
            }
            catch (Exception ex)
            {
                CR.Filename = "EventRepository";
                CR.Eventname = "GetWeClipLimit";
                CR.UserID = GlobalClass.UserID == null ? "0" : GlobalClass.UserID;
                CR.ErrorMsg = ex.Message + ex.StackTrace;
                await crashReport.PostCrashReport(CR);
                return weClipSetting;
            }
        }

    }
}
