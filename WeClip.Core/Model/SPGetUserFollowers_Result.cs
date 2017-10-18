namespace WeClip.Core.Model
{
    public class SPGetUserFollowers_Result
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string ProfilePic { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public bool IsFriend { get; set; }
        public bool IsFriendRequestPending { get; set; }
    }
}
