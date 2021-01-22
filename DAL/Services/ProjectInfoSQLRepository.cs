using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class ProjectInfoSQLRepository : IProjectInfoSQLRepository
    {
        private employeetimesheetdbContext _appDbContext;

        public ProjectInfoSQLRepository(employeetimesheetdbContext _appDbContext)
        {
            this._appDbContext = _appDbContext;
        }
        public async Task<Projectinfo> DeleteAsync(int projectInfoId)
        {
            Projectinfo projectInfoInDb = await GetByIdAsync(projectInfoId);
            _appDbContext.Projectinfo.Remove(projectInfoInDb);
            await _appDbContext.SaveChangesAsync();
            return projectInfoInDb;
        }


        public async Task<IEnumerable<Projectinfo>> GetAllAsync()
        {
            return await _appDbContext.Projectinfo.ToListAsync();
        }

        public async Task<Projectinfo> GetByIdAsync(int projectInfoId)
        {
            return await _appDbContext.Projectinfo.SingleOrDefaultAsync(c=>c.ProjectInfoId==projectInfoId);
        }

        public async Task<Projectinfo> SaveAsync(Projectinfo projectInfo)
        {
            _appDbContext.Projectinfo.Add(projectInfo);
            await _appDbContext.SaveChangesAsync();
            return projectInfo;
        }
    }
}
