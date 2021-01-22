using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper.Configuration;
using DAL.Models;

namespace DAL.CQRS.Reports
{
   
    public class GetTotalWorkingHoursByProjectAsync : IQuery
    {
        private employeetimesheetdbContext _appDbContext;

        public GetTotalWorkingHoursByProjectAsync()
        {
            _appDbContext = new employeetimesheetdbContext(MyDbContextOptions.optionsBuilder.Options);
        }


        public async Task<dynamic> executeAsync()
        {
            var query = from p in _appDbContext.Project
                        join pi in _appDbContext.Projectinfo
                           on p.ProjectId equals pi.ProjectId into q1
                        from pi in q1
                        group new { p.ProjectId, p.Name, pi.HoursWorked } by new { p.ProjectId, p.Name } into q
                        select new
                        {
                            ProjectId=q.Key.ProjectId,
                            Name=q.Key.Name,
                            ProjectWorkingHours = q.Sum(c => c.HoursWorked)
                        };
            return await query.OrderBy(c => c.ProjectWorkingHours).ToListAsync();


        }
    }
}
