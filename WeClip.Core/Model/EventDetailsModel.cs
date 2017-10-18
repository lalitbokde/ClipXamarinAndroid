using System;
using System.Collections.Generic;

namespace WeClip.Core.Model
{
    public  class EventDetailsModel
    {
        public long ID { get; set; }
        public long UserID { get; set; }
        public long FriendID { get; set; }
        public long EventID { get; set; }
        public string FriendName { get; set; }
        public string EventName { get; set; }
        public System.DateTime RequestDate { get; set; }
        public string ResponseText { get; set; }
        public string Status { get; set; }
        public string EventPic { get; set; }
        public Nullable<System.DateTime> ResponseDate { get; set; }
        public List<String> listFiles { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    }
}
