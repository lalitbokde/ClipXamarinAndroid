using System.Collections.Generic;
using System.Threading.Tasks;
using WeClip.Core.Common;
using WeClip.Core.Model;
using WeClip.Core.Repository;

namespace WeClip.Core.Service
{
    public class FriendListService
    {
        private static FriendListRepository repo = new FriendListRepository();

        public async Task<List<FriendsDetailModel>> GetAllFriend(string url)
        {
            return await repo.GetAllFriend(url);
        }

        public async Task<List<FriendsDetailModel>> GetAllConfirmed(string url)
        {
            return await repo.GetAllConfirmed(url);
        }
        public async Task<List<FriendListModel>> GetFriendList(string url)
        {
            return await repo.GetFriendList(url);
        }

        public async Task<List<FriendsDetailModel>> GetFriendRequest(string url)
        {
            return await repo.GetFriendRequest(url);
        }

        public async Task<JsonResult> FriendRequestConfirmed(FriendRequest model, string url)
        {
            return await repo.FriendRequestConfirmed(model, url);
        }
        public async Task<JsonResult> SendInvitations(SendInvitationModel model, string url)
        {
            return await repo.SendInvitations(model, url);
        }
    }
}
