namespace WeClip.Core.Model
{
    public class Reset_AddNewPassword
    {
        public long userID { get; set; }
        public string code { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
