using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTimeSheet.ViewModels.Project
{
    public class ProjectDetailsViewModel
    {
        public Projectinfo Projectinfo { get; set; }

        public List<Activity> Activities { get; set; }


        public ProjectDetailsViewModel()
        {
            Activities = new List<Activity>();
        }
    }
}
