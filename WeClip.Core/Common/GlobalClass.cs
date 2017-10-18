using System;
using System.Collections.Generic;
using WeClip.Core.Model;

namespace WeClip.Core.Common
{
    public static class GlobalClass
    {
        public static string BaseUrl = "http://api.weclipapp.com/api/";

        //public static string Audiofile = "http://api.weclipapp.com/weclip/Audio/File/";
        //public static string Audiofilepic = "http://api.weclipapp.com/weclip/Audio/Thumb/";

        //public static string BaseUrl = "http://weclip.cybermatrixsolutions.com/api/";
        //public static string Audiofile = "http://weclip.cybermatrixsolutions.com/weclip/Audio/File/";
        //public static string Audiofilepic = "http://weclip.cybermatrixsolutions.com/weclip/Audio/Thumb/";

        //public static string BaseUrl = "http://192.168.1.102:1421/api/";
        //public const string strImagePath = "http://192.168.1.102:1421/EventFiles/";
        //public static string ProfileUrl = "http://192.168.1.102:1421/Images/";
        //public static string Audiofile = "http://192.168.1.102:1421/Audio/File/";
        //public static string Audiofilepic = "http://192.168.1.102:1421/Audio/Thumb/";

        public static int MinNoOfSelectedFile = 3;
        public static int MaxVideoMinute = 3;
        public static int maxSelectedImage = 20;
        public static long maxStorageSize = 5000; //in MB 
        public static List<ContactsModel> ContactList { get; set; }

        public static string AccessToken { get; set; }

        public static string UserID { get; set; }

        public static string UserName { get; set; }

        public static string UserEmail { get; set; }

        public static string UserPhone { get; set; }

        public static string ProfilePicture { get; set; }

        public static string Address { get; set; }

        public static string DOB { get; set; }

        public static string Gender { get; set; }

        public static string TokenType { get; set; }

        public static DateTime IssuedDate { get; set; }

        public static int ExpiresIn { get; set; }

        public static DateTime ExpiryDate { get; set; }

        public static UserProfile Profile { get; set; }
    }
}
