using System;

namespace WeClip.Core.Model
{
    public class GetRequestedFriend_Result
    {
        public long ID { get; set; }
        public long SenderID { get; set; }
        public string RequestStatus { get; set; }
        public string ResponseStatus { get; set; }
        public Nullable<System.DateTime> ResponseDate { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string SendrPhoneNumber { get; set; }
        public string SendrEmail { get; set; }
        public string SenderUserName { get; set; }
        public string SenderProfilePic { get; set; }
    }
}
