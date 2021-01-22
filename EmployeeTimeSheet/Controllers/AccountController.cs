using AutoMapper;
using DAL.DTOS;
using DAL.Models;
using DAL.Repositories;
using EmployeeTimeSheet.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTimeSheet.Controllers
{
    [Authorize(Roles ="Admin,Supervisor,User")]
    public class AccountController : Controller
    {
        private IUserSQLRepository _userRepository;
        private UserManager<Aspnetusers> _userManager;
        private SignInManager<Aspnetusers> _signInManager;
        private IMapper _mapper;

        public AccountController(IUserSQLRepository _userRepository,
                                 UserManager<Aspnetusers> _userManager,
                                 SignInManager<Aspnetusers> _signInManager,
                                 IMapper _mapper)
        {
            this._userRepository = _userRepository;
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._mapper = _mapper;
        }
        public async Task< IActionResult> Index()
        {
            UserViewModel model = 
                _mapper.Map<UserViewModelDTO,UserViewModel>
                (await _userRepository.GetUserViewModelDTOByIdAsync(_userManager.GetUserId(User)));

            Aspnetusers supervisor = await _userRepository.GetUserByIdAsync(model.SupervisedBy);
            if (supervisor != null)
                model.SupervisedBy = supervisor.UserName;

            return View(model);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                // ChangePasswordAsync changes the user password
                var result = await _userManager.ChangePasswordAsync(user,
                    model.CurrentPassword, model.NewPassword);

                // The new password did not meet the complexity rules or
                // the current password is incorrect. Add these errors to
                // the ModelState and rerender ChangePassword view
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }

                // Upon successfully changing the password refresh sign-in cookie
                await _signInManager.RefreshSignInAsync(user);
                return View("Success","Your password was changed successfuly!");
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _userRepository.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        

       
        
    }
}
