using System;

namespace WeClip.Core.Model
{
    public class UserProfile
    {
        public long UserID { get; set; }

        public string UserName { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string ProfilePic { get; set; }
        public string PhoneNo { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string Bio { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string DeviceID { get; set; }
        public Nullable<System.Boolean> IsNotificationEnable { get; set; }
        public int? TotalEvents { get; set; }
        public string WeclipImagePath { get; set; }
        public string WeclipVideoPath { get; set; }
        public string isOwnProfile { get; set; }
        public string EmailId { get; set; }
        public string Following { get; set; }
        public string Follwers { get; set; }
        public string WeClipTitle { get; set; }
        public string WeClipTag { get; set; }
        public long DefultEventId { get; set; }
        public string DefultEventName { get; set; }
        public DateTime? EventDate { get; set; }
        public string RequestStatus { get; set; }
        public string WeclipVideoName { get; set; }
    }
}
