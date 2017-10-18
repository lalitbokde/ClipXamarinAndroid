using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeClip.Core.Model
{
    public class SPEventRequestList_Result
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
    }
}
