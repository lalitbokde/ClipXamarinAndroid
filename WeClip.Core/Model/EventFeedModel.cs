using System;

namespace WeClip.Core.Model
{
    public class EventFeedModel
    {
        public long UserID { get; set; }
        public long EventID { get; set; }
        public string Message { get; set; }
        public string EventCreaterName { get; set; }
        public string EventName { get; set; }
        public Nullable<DateTime> FeedDate { get; set; }
        public string UserProfilePicUrl { get; set; }
        public string UserName { get; set; }
    }
}

