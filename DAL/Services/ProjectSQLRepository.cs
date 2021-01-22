using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace DAL.Services
{
    public class ProjectSQLRepository : IProjectSQLRepository
    {
        private employeetimesheetdbContext _appDbContext;

        public ProjectSQLRepository(employeetimesheetdbContext _appDbContext)
        {
            this._appDbContext = _appDbContext;
        }

        public async Task<Project> ChangeActivityStatusAsync(int projectId)
        {
            Project projectInDb = await GetByIdAsync(projectId);
            if (projectInDb.IsActive == 1)
                projectInDb.IsActive = 0;
            else
                projectInDb.IsActive = 0;

            await _appDbContext.SaveChangesAsync();
            return projectInDb;
        }

        public async Task<Project> DeleteAsync(int projectId)
        {
            Project projectInDb = await GetByIdAsync(projectId);
            _appDbContext.Project.Remove(projectInDb);
            await _appDbContext.SaveChangesAsync();
            return projectInDb;
        }

        public async Task<Project> EditAsync(Project project, int projectId)
        {
            Project projectInDb = await GetByIdAsync(projectId);
            projectInDb.Name = project.Name;
            projectInDb.Description = project.Description;
            projectInDb.IsActive = project.IsActive;
            projectInDb.AssignedByUserId = project.AssignedByUserId;
            return project;
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _appDbContext.Project.ToListAsync();
        }

        

        public async Task<Project> GetByIdAsync(int projectId)
        {
            return await _appDbContext.Project.SingleOrDefaultAsync(c => c.ProjectId == projectId);
        }

        public async Task<Project> SaveAsync(Project project)
        {
            _appDbContext.Project.Add(project);
            await _appDbContext.SaveChangesAsync();
            return project;
        }

        
    }
}
