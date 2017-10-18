using System.Collections.Generic;
using System.Threading.Tasks;
using WeClip.Core.Common;
using WeClip.Core.Model;
using WeClip.Core.Repository;

namespace WeClip.Core.Service
{
    public class EventService
    {
        private static EventRepository repo = new EventRepository();

        public async Task<List<CategoryModel>> GetAllCategory(string url)
        {
            return await repo.GetAllCategory(url);
        }

        public async Task<List<SubCategoryModel>> GetAllSubCategory(string url)
        {
            return await repo.GetAllSubCategory(url);
        }



        public async Task<List<EventModel>> GetAllEvent(string url)
        {
            return await repo.GetAllEvent(url);
        }
        

        public async Task<JsonResult> SaveEvent(EventModel ev, string url)
        {
            return await repo.SaveEvent(ev, url);
        }

        public async Task<JsonResult> SendNotification(List<PrivateEventNotification> ev, string url)
        {
            return await repo.SendPrivateEventNotification(ev, url);
        }

        public async Task<EventModel> GetEvent(string url)
        {
            return await repo.GetEvent(url);
        }

        public async Task<List<EventFiles>> GetEventFiles(string url)
        {
            return await repo.GetEventFileDetails(url);
        }


        public async Task<JsonResult> UpdateEvent(EventModel ev, string url)
        {
            return await repo.UpdateEvent(ev, url);
        }

        public async Task<JsonResult> DeleteEvent(string url)
        {
            return await repo.DeleteEvent(url);
        }

        public async Task<List<EventFeedModel>> GetAllFeed(string url)
        {
            return await repo.GetAllFeed(url);
        }

        public async Task<List<long?>> GetEventAdmin(string url)
        {
            return await repo.GetEventAdmin(url);
        }

        public async Task<JsonResult> PostFeed(EventFeedModel ev, string url)
        {
            return await repo.PostFeed(ev, url);
        }

        public async Task<JsonResult> SetEventPicture(EventPictureModel ev, string url)
        {
            return await repo.SetEventPicture(ev, url);
        }
        public async Task<JsonResult> EventResponse(EventDetailsModel model, string url)
        {
            return await repo.RequestEventResponce(model, url);
        }

        public async Task<List<EventDetailsModel>> GetAllRequest(string url)
        {
            return await repo.GetAllEventRequest(url);
        }

        public async Task<JsonResult> MakeAdmin(List<EventAdmin> eventAdminList, string url)
        {
            return await repo.MakeEventadmin(eventAdminList, url);
        }

        public async Task<WeClipSetting> GetWeClipLimit(string url)
        {
            return await repo.GetWeClipLimit(url);

        }
    }
}
