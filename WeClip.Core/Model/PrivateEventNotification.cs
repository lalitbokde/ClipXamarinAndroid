namespace WeClip.Core.Model
{
    public class PrivateEventNotification
    {
        public long UserID { get; set; }
        public long EventID { get; set; }
        public long FriendUserID { get; set; }
        public string Username { get; set; }
        public string SendToEmail { get; set; }
        public string SendToMobile { get; set; }
    }
}
