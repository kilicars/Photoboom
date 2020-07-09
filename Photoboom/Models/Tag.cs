using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Photoboom.Models
{
    public class Tag
    {
        public int TagId { get; set; }

        public string TagName { get; set; }

        public int PhotoId { get; set; }

        public Photo Photo { get; set; }
    }
}
