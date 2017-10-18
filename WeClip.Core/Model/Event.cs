using System;

namespace WeClip.Core.Model
{
    public class Event
    {
        public long ID { get; set; }
        public long UserID { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public int EventSubCategoryID { get; set; }
        public string EventLocation { get; set; }
        public string EventCategory { get; set; }
        public string EventSubCategory { get; set; }
        public Nullable<System.DateTime> EventDate { get; set; }
        public Nullable<System.DateTime> EventStartTime { get; set; }
        public Nullable<System.DateTime> EventEndTime { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string EventPic { get; set; }
        public string Category { get; set; }
        public Nullable<bool> IsDefaultEvent { get; set; }
        public string EventTag { get; set; }
    }
}
