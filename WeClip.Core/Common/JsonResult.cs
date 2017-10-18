namespace WeClip.Core.Common
{
    public class JsonResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ImagePath { get; set; }
        public string ImageName { get; set; }
        public long ID { get; set; }
    }
}
