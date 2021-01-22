using DAL.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IReportFacade
    {
        Task<IEnumerable<ReportDataModel>> GetTotalWorkingHoursByProjectAsync();

        Task<IEnumerable<ReportDataModel>> GetTotalWorkingHoursBySupervisedProjectAsync(string projectSupervisorId);
        
        dynamic GetWorkingHoursByOwnerParams(ProjectActivitiesOwnerParameters ownerParams);

        dynamic GetWorkingHoursByOwnerParamsBySupervisor(ProjectActivitiesOwnerParameters ownerParams,string supervisorId);


        dynamic GetUserEngagementByOwnerParams(UserEngagementOwnerParameters ownerParams);

        dynamic GetUserEngagementByOwnerParamsBySupervisor(UserEngagementOwnerParameters ownerParams,string supervisorId);

    }
}
