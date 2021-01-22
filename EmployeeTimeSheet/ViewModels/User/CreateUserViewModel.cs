using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTimeSheet.ViewModels
{
    public class CreateUserViewModel
    {

        [Remote(action: "IsUsernameInUse", controller: "Administration")]
        [Required]
        [StringLength(15)]
        [MinLength(6,
            ErrorMessage = "Username must be atleast 6 characters long.")]
        [Display(Name = "Username")]
        [RegularExpression(@"^[a-z]+$",
         ErrorMessage = "Username must be lowercase, special character and numbers are not allowed.")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }

        public string SupervisedBy { get; set; }
    }
}
