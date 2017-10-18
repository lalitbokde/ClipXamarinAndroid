using System;

namespace WeClip.Core.Model
{
    public class Authentication
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string userName { get; set; }
        public long userId { get; set; }
        public DateTime issued { get; set; }
        public DateTime expires { get; set; }
        public string EmailID { get; set; }
        public string ProfilePic { get; set; }
        public bool? IsNotificationEnable { get; set; }
        public string DOB { get; set; }
        public bool? Success { get; set; }
        public string Message { get; set; }
    }
}

