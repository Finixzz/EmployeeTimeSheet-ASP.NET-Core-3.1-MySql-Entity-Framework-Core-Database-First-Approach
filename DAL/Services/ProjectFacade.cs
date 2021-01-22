using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DAL.Services
{
    public class ProjectFacade : IProjectFacade
    {
        private employeetimesheetdbContext _appDbContext;

        private IReportFacade _reportFacade;


        public ProjectFacade(employeetimesheetdbContext _appDbContext,
                            IReportFacade _reportFacade)
        {
            this._appDbContext = _appDbContext;
            this._reportFacade = _reportFacade;
        }

        public  async Task<IEnumerable<Project>> GetAllProjectsAssignedToWorker(string supervisorId)
        {
            return  await _appDbContext.Project.Where(c => c.AssignedByUserId == supervisorId).ToListAsync();               
        }

        public async Task<IEnumerable<Project>> GetAllProjectsWithWorkingHoursAsync()
        {
            var projectsWithWorkingHours = await _reportFacade.GetTotalWorkingHoursByProjectAsync();
            List<Project> modelList = new List<Project>();

            for(int i = 0; i < projectsWithWorkingHours.Count(); i++)
            {
                Project model = new Project()
                {
                    ProjectId = projectsWithWorkingHours.ElementAt(i).ProjectId,
                    Name = projectsWithWorkingHours.ElementAt(i).ProjectName
                };
                modelList.Add(model);
            }
            return modelList;
        }

        public async Task<IEnumerable<Project>> GetAllSupervisedProjectsWithWorkingHoursAsync(string supervisorId)
        {
            var projectsWithWorkingHours = await _reportFacade.GetTotalWorkingHoursBySupervisedProjectAsync(supervisorId);
            List<Project> modelList = new List<Project>();

            for (int i = 0; i < projectsWithWorkingHours.Count(); i++)
            {
                Project model = new Project()
                {
                    ProjectId = projectsWithWorkingHours.ElementAt(i).ProjectId,
                    Name = projectsWithWorkingHours.ElementAt(i).ProjectName
                };
                modelList.Add(model);
            }
            return modelList;
        }
    }
}
