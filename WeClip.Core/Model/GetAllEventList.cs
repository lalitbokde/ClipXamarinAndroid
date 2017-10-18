using System;
using System.Collections.Generic;

namespace WeClip.Core.Model
{
    public class GetAllEventList
    {
        public long EventID { get; set; }
        public string EventType { get; set; }
        public string EventName { get; set; }
        public string EventLocation { get; set; }
        public string EventDate { get; set; }
        public string EventPic { get; set; }
        public string creatorname { get; set; }
        public long creatorId { get; set; }
        public string creatorpic { get; set; }
        public List<String> listFiles { get; set; }
        public string CoHost { get; set; }
    }
}
