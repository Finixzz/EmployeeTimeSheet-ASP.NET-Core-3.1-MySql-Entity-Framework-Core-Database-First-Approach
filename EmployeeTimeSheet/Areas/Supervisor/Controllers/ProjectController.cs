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

namespace EmployeeTimeSheet.Areas.Supervisor.Controllers
{
    [Area("Supervisor")]
    [Authorize(Roles = "Supervisor")]

    public class ProjectController : Controller
    {
        private IProjectSQLRepository _projectSQLRepository;
        private IActivitySQLRepository _activitySQLRepository;
        private UserManager<Aspnetusers> _userManager;
        private IProjectFacade _projectFacade;
        private IMapper _mapper;

        public ProjectController(IProjectSQLRepository _projectSQLRepository,
                                  IActivitySQLRepository _activitySQLRepository,
                                  IProjectFacade _projectFacade,
                                  UserManager<Aspnetusers> _userManager,
                                  IMapper _mapper)
        {
            this._projectSQLRepository = _projectSQLRepository;
            this._activitySQLRepository = _activitySQLRepository;
            this._projectFacade = _projectFacade;
            this._userManager = _userManager;
            this._mapper = _mapper;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _projectFacade.GetAllProjectsAssignedToWorker(_userManager.GetUserId(User));
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateNewProject()
        {
            return View(new CreateProjectViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewProject(CreateProjectViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            Project newProject = await _projectSQLRepository.SaveAsync(_mapper.Map<CreateProjectViewModel, Project>(model));

            if (newProject == null)
            {
                ModelState.AddModelError("", "Something went wrong.");
                return View(model);
            }

            return RedirectToAction("index");
        }

        [HttpGet]
        public IActionResult CreateNewActivity(int id)
        {
            CreateActivityViewModel model = new CreateActivityViewModel()
            {
                ProjectId = id
            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> CreateNewActivity(CreateActivityViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            Activity newProjectActivity = await _activitySQLRepository.SaveAsync(_mapper.Map<CreateActivityViewModel, Activity>(model));

            if (newProjectActivity == null)
            {
                ModelState.AddModelError("", "Something went wrong.");
                return View(model);
            }
            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<IActionResult> ProjectDetails(int id)
        {
            Project project = await _projectSQLRepository.GetByIdAsync(id);


            return View(project);
        }
    }
}

