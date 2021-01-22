using AutoMapper;
using DAL.Models;
using DAL.Repositories;
using EmployeeTimeSheet.ViewModels.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTimeSheet.Controllers
{
    [Authorize(Roles =("User,Supervisor"))]
    public class WorkingHoursController : Controller
    {
        private IProjectSQLRepository _projectRepository;
        private IProjectInfoSQLRepository _projectInfoRepository;
        private IActivitySQLRepository _activityRepository;
        private IProjectFacade _projectFacade;
        private UserManager<Aspnetusers> _userManager;
        private IMapper _mapper;

        public WorkingHoursController(IProjectSQLRepository _projectRepository,
                                      IProjectInfoSQLRepository _projectInfoRepository,
                                      IActivitySQLRepository _activityRepository,
                                      IProjectFacade _projectFacade,
                                      UserManager<Aspnetusers> _userManager,
                                      IMapper _mapper)
        {
            this._projectFacade = _projectFacade;
            this._projectRepository = _projectRepository;
            this._projectInfoRepository = _projectInfoRepository;
            this._activityRepository = _activityRepository;
            this._projectInfoRepository = _projectInfoRepository;
            this._userManager = _userManager;
            this._mapper = _mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Aspnetusers user = await _userManager.FindByNameAsync(User.Identity.Name);
            var model = await _projectFacade.GetAllProjectsAssignedToWorker(user.SupervisedBy);
            return View(model);
        }

        [HttpGet]
        public IActionResult Project(int id)
        {
            Projectinfo model = new Projectinfo()
            {
                ProjectId = id
            };
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> SaveProjectWorkingHours(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            AddProjectWorkingHoursViewModel model = new AddProjectWorkingHoursViewModel()
            {
                ProjectId = id,
                UserId=user.Id
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveProjectWorkingHours(AddProjectWorkingHoursViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);


            Projectinfo newProjectInfo=await _projectInfoRepository.SaveAsync(_mapper.Map<AddProjectWorkingHoursViewModel,Projectinfo>(model));

            if (newProjectInfo == null)
            {
                ModelState.AddModelError("", "Something went wrong.");
                return View(model);
            }
            return View("success","Working hours added successfuly!");
        }


   
      


    }
}
