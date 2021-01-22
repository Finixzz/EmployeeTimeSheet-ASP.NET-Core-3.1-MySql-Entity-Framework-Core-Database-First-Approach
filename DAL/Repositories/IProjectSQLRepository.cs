using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IProjectSQLRepository
    {
        Task<IEnumerable<Project>> GetAllAsync();


        Task<Project> GetByIdAsync(int projectId);

        Task<Project> SaveAsync(Project project);

        Task<Project> EditAsync(Project project, int projectId);

        Task<Project> ChangeActivityStatusAsync(int projectId);

        Task<Project> DeleteAsync(int projectId);

        
    }
}
