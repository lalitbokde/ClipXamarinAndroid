using System;

namespace WeClip.Core.Model
{
    public class NotificationModel
    {
        public long ID { get; set; }
        public long UserID { get; set; }
        public long EventID { get; set; }
        public string NotificationFor { get; set; }
        public string Message { get; set; }
        public string Response { get; set; }
        public string Status { get; set; }
        public DateTime?  CreatedOn { get; set; }
        public string Type { get; set; }
        public string SenderImage { get; set; }
        public string SenderName { get; set; }
        public long SenderID { get; set; }
        public long? WeClipID { get; set; }
    }
}
