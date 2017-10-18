using System;

namespace WeClip.Core.Model
{
    public class SPGetUserFollowings_Result
    {
        public long ParentUserID { get; set; }
        public string UserName { get; set; }
        public string ProfilePic { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public bool IsFriend { get; set; }
        public bool isFriendRequestPending { get; set; }
    }
}
