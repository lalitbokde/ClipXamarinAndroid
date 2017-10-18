using System;

namespace WeClip.Core.Model
{
    public class FriendRequest
    {
        public long ID { get; set; }
        public long UserID { get; set; }
        public string SendToEmail { get; set; }
        public long SendToId { get; set; }
        public string SendToMobile { get; set; }
        public string RequestStatus { get; set; }
        public string ResponseStatus { get; set; }
        public System.DateTime RequestDate { get; set; }
        public Nullable<System.DateTime> ResponseDate { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string Username { get; set; }
        public string UserPhoto { get; set; }
    }
}
