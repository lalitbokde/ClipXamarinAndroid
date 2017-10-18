namespace WeClip.Core.Model
{
    public class UserData
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public bool IsInvited { get; set; }
        public string UserPhoto { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public bool IsFriend { get; set; }
        public bool IsPublicProfile { get; set; }
    }
}
