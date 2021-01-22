using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Projectinfo
    {
        public int ProjectInfoId { get; set; }
        public int ProjectId { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public int HoursWorked { get; set; }

        public virtual Project Project { get; set; }
        public virtual Aspnetusers User { get; set; }
    }
}
