using System;

namespace WeClip.Core.Model
{
    public class Audio
    {
        public int ID { get; set; }
        public string FileName { get; set; }
        public string Thumb { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string Name { get; set; }
        public string FileUrl { get; set; }


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
