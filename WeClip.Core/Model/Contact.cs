namespace WeClip.Core.Model
{

    public class Contact
    {
        public long Id { get; set; }
        public string DisplayName { get; set; }
        public string PhotoId { get; set; }
        public string EmailID { get; set; }
        public string PhoneNo { get; set; }
        public bool isFriend { get; set; }
        public bool isInvited { get; set; }
        public bool isPublic { get; set; }
        public bool separator { get; set; }
        public bool isFriendRequestPending { get; set; }
        public bool isEmailAddress { get; set; }
        public long PhoneContactId { get; set; }
        public bool isWeClipUser { get; set; }

        private bool selected;
        public bool isSelected()
        {
            return selected;
        }
        public void setSelected(bool selected)
        {
            this.selected = selected;
        }
    }
}
