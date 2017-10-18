using System;

namespace WeClip.Core.Model
{
    public class FriendListModel 
    {
        public long ID { get; set; }
        public string PhoneNumber { get; set; }
        public string Picture { get; set; }
        public string FriendName { get; set; }
        public string IsFriend { get; set; }
        public string isInvitedFriend { get; set; }
        public string InvitedContact { get; set; }
        public string Email { get; set; }
        public string SignUpType { get; set; }
        public string ResponseText { get; set; }

        public int FriendUserID { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string Gender { get; set; }
        public bool IsContact { get; set; }
        public bool IsEmail { get; set; }

        public long PhoneContactId { get; set; }
        private bool selected;

        public bool IsFollow { get; set; }

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
