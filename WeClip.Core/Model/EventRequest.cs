namespace WeClip.Core.Model
{
  public  class EventRequest
    {
        public long EventID { get; set; }
        public long SendToId { get; set; }
        public long FriendID { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
