using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeClip.Core.Common;
using WeClip.Core.Model;
using WeClip.Core.Service;

namespace WeClip.Core.Repository
{
    class FriendListRepository
    {
        HttpCall call;
        CrashReportService crashReport = new CrashReportService();
        CrashReportModel CR = new CrashReportModel();

        public FriendListRepository()
        {
            call = new HttpCall();
        }

        public async Task<List<FriendsDetailModel>> GetAllFriend(string url)
        {
            List<FriendsDetailModel> friends = null;
            try
            {
                friends = await call.GetAll<FriendsDetailModel>(url);
                return friends;
            }
            catch (Exception ex)
            {
                CR.Filename = "FriendListRepository";
                CR.Eventname = "GetAllFriend";
                CR.UserID = GlobalClass.UserID == null ? "0" : GlobalClass.UserID;
                                CR.ErrorMsg = ex.Message+ ex.StackTrace;
                await crashReport.PostCrashReport(CR);
                return friends;
            }
        }

        public async Task<List<FriendsDetailModel>> GetAllConfirmed(string url)
        {
            List<FriendsDetailModel> friends = null;

            try
            {
                friends = await call.GetAll<FriendsDetailModel>(url);
                return friends;
            }
            catch (Exception ex)
            {
                CR.Filename = "FriendListRepository";
                CR.Eventname = "GetAllConfirmed";
                CR.UserID = GlobalClass.UserID == null ? "0" : GlobalClass.UserID;
                                CR.ErrorMsg = ex.Message+ ex.StackTrace;
                await crashReport.PostCrashReport(CR);
                return friends;
            }
        }

        public async Task<List<FriendListModel>> GetFriendList(string url)
        {
            List<FriendListModel> friends = null;

            try
            {
                friends = await call.GetAll<FriendListModel>(url);
                return friends;
            }
            catch (Exception ex)
            {
                CR.Filename = "FriendListRepository";
                CR.Eventname = "GetFriendList";
                CR.UserID = GlobalClass.UserID == null ? "0" : GlobalClass.UserID;
                                CR.ErrorMsg = ex.Message+ ex.StackTrace;
                await crashReport.PostCrashReport(CR);
                return friends;
            }
        }

        public async Task<List<FriendsDetailModel>> GetFriendRequest(string url)
        {
            List<FriendsDetailModel> friends = null;

            try
            {
                friends = await call.GetAll<FriendsDetailModel>(url);
                return friends;
            }
            catch (Exception ex)
            {
                CR.Filename = "FriendListRepository";
                CR.Eventname = "GetFriendRequest";
                CR.UserID = GlobalClass.UserID == null ? "0" : GlobalClass.UserID;
                                CR.ErrorMsg = ex.Message+ ex.StackTrace;
                await crashReport.PostCrashReport(CR);
                return friends;
            }
        }

        public async Task<JsonResult> FriendRequestConfirmed(FriendRequest model, string url)
        {
            JsonResult resp = null;
            try
            {
                resp = await call.Post<FriendRequest>(model, url);
                return resp;
            }
            catch (Exception ex)
            {
                CR.Filename = "FriendListRepository";
                CR.Eventname = "FriendRequestConfirmed";
                CR.UserID = GlobalClass.UserID == null ? "0" : GlobalClass.UserID;
                                CR.ErrorMsg = ex.Message+ ex.StackTrace;
                await crashReport.PostCrashReport(CR);
                return resp;
            }
        }

        public async Task<JsonResult> SendInvitations(SendInvitationModel model, string url)
        {
            JsonResult resp = null;
            try
            {
                resp = await call.Post<SendInvitationModel>(model, url);
                return resp;
            }
            catch (Exception ex)
            {
                CR.Filename = "FriendListRepository";
                CR.Eventname = "SendInvitations";
                CR.UserID = GlobalClass.UserID == null ? "0" : GlobalClass.UserID;
                                CR.ErrorMsg = ex.Message+ ex.StackTrace;
                await crashReport.PostCrashReport(CR);
                return resp;
            }

        }
    }
}
