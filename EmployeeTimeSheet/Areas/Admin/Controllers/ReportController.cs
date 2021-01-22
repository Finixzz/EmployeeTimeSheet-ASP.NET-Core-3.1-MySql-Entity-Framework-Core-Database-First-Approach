using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTimeSheet.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReportController : Controller
    {


        public IActionResult ProjectActivities()
        {
            return View();
        }

        public IActionResult UserEngagment()
        {
            return View();
        }
    }
}
