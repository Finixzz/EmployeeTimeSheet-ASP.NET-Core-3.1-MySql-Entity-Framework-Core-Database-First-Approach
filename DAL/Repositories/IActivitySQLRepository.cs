using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IActivitySQLRepository
    {
        Task<IEnumerable<Activity>> GetAllByProjectIdAsync(int projectId);
        Task<Activity> GetByIdAsync(int activityId);

        Task<Activity> SaveAsync(Activity activity);

        Task<Activity> EditAsync(Activity activity, int activityId);

        Task<Activity> DeleteAsync(int activityId);
    }
}
