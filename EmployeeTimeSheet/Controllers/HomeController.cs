using DAL.Models;
using DAL.Repositories;
using EmployeeTimeSheet.Models;
using EmployeeTimeSheet.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTimeSheet.Controllers
{
    public class HomeController : Controller
    {
        private IUserSQLRepository _userSQLRepository;

        public HomeController(IUserSQLRepository _userSQLRepository)
        {
            this._userSQLRepository = _userSQLRepository;
        }

        public IActionResult Index()
        {
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);


            var result = await _userSQLRepository.SignInAsync(model.Username, model.Password, model.RememberMe);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                return View(model);
            }
                

            return RedirectToAction("index", "home");
            
        }

     
    }
}
