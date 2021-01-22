using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IProjectFacade
    {
        Task<IEnumerable<Project>> GetAllProjectsWithWorkingHoursAsync();

        Task<IEnumerable<Project>> GetAllSupervisedProjectsWithWorkingHoursAsync(string supervisorId);

        Task<IEnumerable<Project>> GetAllProjectsAssignedToWorker(string supervisorId);
    }
}
