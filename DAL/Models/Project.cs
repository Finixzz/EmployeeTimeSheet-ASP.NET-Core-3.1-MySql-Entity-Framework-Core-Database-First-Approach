using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public  class Project
    {
        public Project()
        {
            Activity = new HashSet<Activity>();
            Projectinfo = new HashSet<Projectinfo>();
        }

        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string AssignedByUserId { get; set; }
        public string Description { get; set; }

        public short? IsActive { get; set; }

        public virtual Aspnetusers AssignedByUser { get; set; }
        public virtual ICollection<Activity> Activity { get; set; }
        public virtual ICollection<Projectinfo> Projectinfo { get; set; }
    }
}
