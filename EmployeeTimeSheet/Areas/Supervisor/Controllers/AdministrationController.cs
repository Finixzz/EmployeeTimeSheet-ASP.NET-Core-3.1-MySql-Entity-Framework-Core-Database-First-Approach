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

namespace EmployeeTimeSheet.Areas.Supervisor.Controllers
{
    [Area("Supervisor")]
    [Authorize(Roles = "Supervisor")]
    public class AdministrationController : Controller
    {
        private IUserSQLRepository _userRepository;
        private IMapper _mapper;
        private UserManager<Aspnetusers> _userManager;

        public AdministrationController(IUserSQLRepository _userRepository,
                                        UserManager<Aspnetusers> _userManager,
                                        IMapper _mapper)
        {
            this._userRepository = _userRepository;
            this._userManager = _userManager;
            this._mapper = _mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var usersInDb = await _userRepository.GetAllSupervisedUsersBySupervisorAsync(_userManager.GetUserId(User));
            IEnumerable<UserViewModel> listUserViewModels = _mapper.Map
                <IEnumerable<UserViewModelDTO>, IEnumerable<UserViewModel>>(usersInDb);


            for (int i = 0; i < listUserViewModels.Count(); i++)
            {
                string supervisorId = listUserViewModels.ElementAt(i).SupervisedBy;
                Aspnetusers supervisor = await _userRepository.GetUserByIdAsync(supervisorId);
                if (supervisor != null)
                    listUserViewModels.ElementAt(i).SupervisedBy = supervisor.UserName;
            }

            return View(listUserViewModels);
        }



        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> IsUsernameInUse(string username)
        {
            bool usernameInUse = await _userRepository.IsUsernameInUseAsync(username);

            if (usernameInUse)
                return Json($"Username {username} is already in use.");

            return Json(true);
        }

        [HttpGet]
        public IActionResult CreateNewUser()
        {
            CreateUserViewModel model = new CreateUserViewModel()
            {
                RoleName = "User",
                SupervisedBy = _userManager.GetUserId(User)
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewUser(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            Aspnetusers newUser = await _userRepository.CreateUserAsync(model.UserName, model.SupervisedBy, model.RoleName);
            if (newUser == null)
            {
                ModelState.AddModelError("", "Something went wrong.");
                return View(model);
            }
            return RedirectToAction("UserDetails", new { id = newUser.Id });

        }

        [HttpGet]
        public async Task<IActionResult> UserDetails(string id)
        {
            UserViewModel model = _mapper.Map<UserViewModelDTO, UserViewModel>(await _userRepository.GetUserViewModelDTOByIdAsync(id));
            Aspnetusers supervisor = await _userRepository.GetUserByIdAsync(model.SupervisedBy);
            if (supervisor != null)
                model.SupervisedBy = supervisor.UserName;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {

            await _userRepository.DeleteUserAsync(id);

            return RedirectToAction("users", "administration");

        }


        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            EditUserViewModel model = _mapper.Map<UserViewModelDTO, EditUserViewModel>(await _userRepository.GetUserViewModelDTOByIdAsync(id));
            Aspnetusers supervisor = await _userRepository.GetUserByIdAsync(model.SupervisedBy);
            if (supervisor != null)
                model.SupervisedBy = supervisor.UserName;
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            Aspnetusers newUser = await _userRepository.EditUserAsync(model.Id, model.UserName, model.SupervisedBy, model.RoleName);

            if (newUser == null)
            {
                ModelState.AddModelError("", "Something went wrong.");
                return View(model);
            }
            return RedirectToAction("UserDetails", new { id = newUser.Id });
        }


    }
}
