﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTimeSheet.Areas.Supervisor.Controllers
{
    [Area("Supervisor")]
    [Authorize(Roles = "Supervisor")]

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
