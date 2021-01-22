using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Aspnetusers : IdentityUser
    {
        public Aspnetusers()
        {
            Aspnetuserclaims = new HashSet<Aspnetuserclaims>();
            Aspnetuserlogins = new HashSet<Aspnetuserlogins>();
            Aspnetuserroles = new HashSet<Aspnetuserroles>();
            Aspnetusertokens = new HashSet<Aspnetusertokens>();
            InverseSupervisedByNavigation = new HashSet<Aspnetusers>();
            Project = new HashSet<Project>();
            Projectinfo = new HashSet<Projectinfo>();
        }

        public string SupervisedBy { get; set; }

        public virtual Aspnetusers SupervisedByNavigation { get; set; }
        public virtual ICollection<Aspnetuserclaims> Aspnetuserclaims { get; set; }
        public virtual ICollection<Aspnetuserlogins> Aspnetuserlogins { get; set; }
        public virtual ICollection<Aspnetuserroles> Aspnetuserroles { get; set; }
        public virtual ICollection<Aspnetusertokens> Aspnetusertokens { get; set; }
        public virtual ICollection<Aspnetusers> InverseSupervisedByNavigation { get; set; }
        public virtual ICollection<Project> Project { get; set; }
        public virtual ICollection<Projectinfo> Projectinfo { get; set; }
    }
}
