namespace WeClip.Core.Model
{
    public class Token
    {
        public string access_token { get; set; }
        public string LoginUserName { get; set; }
        public string UserName { get; set; }
        public string EmailID { get; set; }
        public long UserID { get; set; }
        public string ProfilePic { get; set; }
        public bool? IsNotificationEnable { get; set; }
        public string DOB { get; set; }
        public bool? Success { get; set; }
        public string Message { get; set; }
        public string token_type { get; set; }
        public float expires_in { get; set; }
        public string PhoneNumber { get; set; }
        public string Bio { get; set; }
        public object IsPublic { get; set; }
        public int MaxImageForWeClip { get; set; }
        public long MaxVideoDurationInMinute { get; set; }
        public int MaxVideoForWeclip { get; set; }
        public long MaxVideoSize { get; set; }
        public string status { get; set; }


    }
}
