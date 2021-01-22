using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DAL.Microservices;
using DAL.Helpers;

namespace DAL.CQRS.Reports.Queries
{
    public class GetTotalProjectWorkingHoursBySupervisorAsync :IQuery
    {
        private employeetimesheetdbContext _appDbContext;

        private string _supervisorId;
        public GetTotalProjectWorkingHoursBySupervisorAsync(string supervisorId)
        {
            _appDbContext = new employeetimesheetdbContext(MyDbContextOptions.optionsBuilder.Options);
            _supervisorId = supervisorId;
        }

        public async Task<dynamic> executeAsync()
        {
            var query = from p in _appDbContext.Project
                        join pi in _appDbContext.Projectinfo
                           on p.ProjectId equals pi.ProjectId into q1
                        from pi in q1
                        where p.AssignedByUserId == _supervisorId
                        group new { p.ProjectId, p.Name, pi.HoursWorked } by new { p.ProjectId, p.Name } into q
                        select new
                        {
                            ProjectId = q.Key.ProjectId,
                            Name = q.Key.Name,
                            ProjectWorkingHours = q.Sum(c => c.HoursWorked)
                        };
            return await query.OrderBy(c => c.ProjectWorkingHours).ToListAsync();


        }

    }
}
