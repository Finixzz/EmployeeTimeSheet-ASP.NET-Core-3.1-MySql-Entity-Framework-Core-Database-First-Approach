using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTimeSheet.ViewModels.Project
{
     
        public class CreateProjectViewModel
        {
        [Required]
        [Display(Name = "Project name")]
        [MinLength(5,ErrorMessage ="Project name must be atleast 5 characters long")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Project supervisor")]
        public string AssignedByUserId { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Project name must be atleast 5 characters long")]
        [MaxLength(255)]
        public string Description { get; set; }


        public short? IsActive { get; set; }

        public CreateProjectViewModel()
        {
            IsActive = 1;

        }

    }
}
