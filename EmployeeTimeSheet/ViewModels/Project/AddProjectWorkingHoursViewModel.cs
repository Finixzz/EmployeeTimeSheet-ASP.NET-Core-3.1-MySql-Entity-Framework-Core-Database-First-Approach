using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTimeSheet.ViewModels.Project
{
    public class AddProjectWorkingHoursViewModel
    {
        [Required]
        public int ProjectId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Range(1,8)]
        [Display(Name = "Hours worked")]
        public int HoursWorked { get; set; }


        public AddProjectWorkingHoursViewModel()
        {
            Date = DateTime.Now;
        }
    }
}
