using System;

namespace WeClip.Core.Model
{
    public class EventDetails
    {
        public long Id { get; set; }
        public string EventName { get; set; }
        public string EventTag { get; set; }
        public string EventDescription { get; set; }
        public string EventCategory { get; set; }
        public string Address { get; set; }
        public DateTime? EventDate { get; set; }
        public string EventLocation { get; set; }
        public DateTime? EventStartTime { get; set; }
        public string HostName { get; set; }
        public string Going { get; set; }
        public string Maybe { get; set; }
        public string Invited { get; set; }
        public string TotalLikes { get; set; }
        public bool? isEventLike { get; set; }
        public string EventResponse { get; set; }
        public bool IsCohostOrOwnEvent { get; set; }
        public string EventPicUrl { get; set; }
        public string EventPic { get; set; }
        public long CreatorId { get; set; }
    }
}
