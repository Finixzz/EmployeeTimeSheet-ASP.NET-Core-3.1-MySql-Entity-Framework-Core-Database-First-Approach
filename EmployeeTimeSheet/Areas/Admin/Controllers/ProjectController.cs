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

namespace EmployeeTimeSheet.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class ProjectController : Controller
    {
        private IProjectSQLRepository _projectSQLRepository;
        private IActivitySQLRepository _activitySQLRepository;
        private UserManager<Aspnetusers> _userManager;
        private IMapper _mapper;

        public ProjectController( IProjectSQLRepository _projectSQLRepository,
                                  IActivitySQLRepository _activitySQLRepository,
                                  UserManager<Aspnetusers> _userManager,
                                  IMapper _mapper)
        {
            this._projectSQLRepository = _projectSQLRepository;
            this._activitySQLRepository = _activitySQLRepository;
            this._userManager = _userManager;
            this._mapper = _mapper;
        }

        public async Task<IActionResult> Index()
        {
            var model = await  _projectSQLRepository.GetAllAsync();
            for(int i = 0; i < model.Count(); i++)
            {
                string userId = model.ElementAt(i).AssignedByUserId;
                model.ElementAt(i).AssignedByUser = await _userManager.FindByIdAsync(userId);
            }
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

            Activity newProjectActivity = await  _activitySQLRepository.SaveAsync(_mapper.Map<CreateActivityViewModel, Activity>(model));

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
            ProjectDetailsViewModel model = new ProjectDetailsViewModel()
            {

            };
            Project project =await _projectSQLRepository.GetByIdAsync(id);

            
            return View(project);
        }
    }
}
