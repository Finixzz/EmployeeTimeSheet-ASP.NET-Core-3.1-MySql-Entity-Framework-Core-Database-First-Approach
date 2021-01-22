using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Activity
    {
       

        public int ActivityId { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        public virtual Project Project { get; set; }
    }
}
