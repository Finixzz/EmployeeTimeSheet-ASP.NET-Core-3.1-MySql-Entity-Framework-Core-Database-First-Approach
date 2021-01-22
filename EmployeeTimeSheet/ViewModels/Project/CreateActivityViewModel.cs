using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTimeSheet.ViewModels.Project
{
    public class CreateActivityViewModel
    {
        public int ProjectId { get; set; }

        [Required]
        [Display(Name = "Project activity name")]
        [MinLength(5, ErrorMessage = "Project activity name must be atleast 5 characters long")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Project name must be atleast 5 characters long")]
        [MaxLength(255)]
        public string Description { get; set; }

        public short? IsActive { get; set; }

        public CreateActivityViewModel()
        {
            IsActive = 1;
        }
    }
}
