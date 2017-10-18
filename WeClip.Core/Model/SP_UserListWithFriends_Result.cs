namespace WeClip.Core.Model
{
    public class SP_UserListWithFriends_Result
    {
        public long ID { get; set; }
        public string PhoneNumber { get; set; }
        public string IsFriend { get; set; }
        public string isFriendRequestPending { get; set; }
        public string InvitedContact { get; set; }
        public string Email { get; set; }
        public string SignUpType { get; set; }
    }
}
