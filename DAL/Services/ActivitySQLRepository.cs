using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class ActivitySQLRepository : IActivitySQLRepository
    {
        private employeetimesheetdbContext _appDbContext;

        public  ActivitySQLRepository(employeetimesheetdbContext _appDbContext)
        {
            this._appDbContext = _appDbContext;
        }

      

        public async Task<Activity> DeleteAsync(int activityId)
        {
            Activity activityInDb = await GetByIdAsync(activityId);
            _appDbContext.Activity.Remove(activityInDb);
            await _appDbContext.SaveChangesAsync();
            return activityInDb;
        }

        public async Task<Activity> EditAsync(Activity activity, int activityId)
        {
            Activity activityInDb = await GetByIdAsync(activityId);
            activityInDb.ProjectId = activity.ProjectId;
            activityInDb.Name = activity.Name;
            await _appDbContext.SaveChangesAsync();
            return activityInDb;
        }

        public async Task<IEnumerable<Activity>> GetAllByProjectIdAsync(int projectId)
        {
            return await _appDbContext.Activity.Where(c => c.ProjectId == projectId).ToListAsync();
        }

        public async Task<Activity> GetByIdAsync(int activityId)
        {
            return await _appDbContext.Activity.SingleOrDefaultAsync(c => c.ActivityId == activityId);
        }

        public async Task<Activity> SaveAsync(Activity activity)
        {
            _appDbContext.Activity.Add(activity);
            await _appDbContext.SaveChangesAsync();
            return activity;
        }
    }
}
