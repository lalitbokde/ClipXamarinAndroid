namespace WeClip.Core.Model
{
    public class FriendsDetailModel
    {
        public long ID { get; set; }

        public long UserID { get; set; }

        public string SendToEmail { get; set; }

        public string SendToMobile { get; set; }

        public string RequestStatus { get; set; }

        public string ResponseStatus { get; set; }

        public string RequestDate { get; set; }

        public string ResponseDate { get; set; }

        public string CreatedOn { get; set; }

        public string ModifiedOn { get; set; }

        public string Username { get; set; }
    }
}
