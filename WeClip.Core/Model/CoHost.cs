using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeClip.Core.Model
{
    public class CoHost
    {
        public long ID { get; set; }
        public string CoHostName { get; set; }
        public long CoHostUserID { get; set; }
        public string CoHostProfilePic { get; set; }
        public bool IsAdmin { get; set; }

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
