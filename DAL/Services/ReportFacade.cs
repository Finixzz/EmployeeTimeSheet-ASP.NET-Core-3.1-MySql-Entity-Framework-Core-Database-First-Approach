using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DAL.Helpers;
using System.Globalization;
using System.Collections;

namespace DAL.Services
{

    public class ReportFacade : IReportFacade
    {
        private employeetimesheetdbContext _appDbContext;

        public ReportFacade(employeetimesheetdbContext _appDbContext)
        {
            this._appDbContext = _appDbContext;
        }
        

        public async Task<IEnumerable<ReportDataModel>> GetTotalWorkingHoursByProjectAsync()
        {
            var query = from p in _appDbContext.Project
                        join pi in _appDbContext.Projectinfo
                           on p.ProjectId equals pi.ProjectId into q1
                        from pi in q1
                        group new { p.ProjectId, p.Name, pi.HoursWorked } by new { p.ProjectId, p.Name } into q
                        select new
                        {
                            q.Key,
                            ProjectWorkingHours = q.Sum(c => c.HoursWorked)
                        };
            var queryResult = await query.OrderBy(c => c.ProjectWorkingHours).ToListAsync();

            List<ReportDataModel> modelList = new List<ReportDataModel>();
            for (int i = 0; i < queryResult.Count(); i++)
            {

                int totalWorkingHours = queryResult.ElementAt(i).ProjectWorkingHours;

                if (totalWorkingHours != 0)
                {
                    ReportDataModel model = new ReportDataModel()
                    {
                        ProjectId = queryResult[i].Key.ProjectId,
                        ProjectName = queryResult[i].Key.Name,
                        TotalWorkingHours = totalWorkingHours
                    };
                    modelList.Add(model);
                }
            }
            return modelList;
        }



        private static int GetWeekNumberOfMonth(DateTime date)
        {
            date = date.Date;
            DateTime firstMonthDay = new DateTime(date.Year, date.Month, 1);
            DateTime firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            if (firstMonthMonday > date)
            {
                firstMonthDay = firstMonthDay.AddMonths(-1);
                firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            }
            return (date - firstMonthMonday).Days / 7 + 1;
        }

        public class ReportModelHelper
        {


            public int WeekNumber { get; set; }

            public int Month { get; set; }

            public int Year { get; set; }

            public int TotalWorkingHours { get; set; }

        };

        public dynamic GetWorkingHoursByOwnerParams(ProjectActivitiesOwnerParameters ownerParams)
        {

            if (ownerParams.DateFormatSelected == "dd")
            {
                var query = from p in _appDbContext.Project
                            join pi in _appDbContext.Projectinfo
                               on p.ProjectId equals pi.ProjectId into q1
                            from pi in q1
                            where p.ProjectId == ownerParams.ProjectId
                            group new { p.ProjectId, p.Name, pi.HoursWorked, pi.Date } by new { p.ProjectId, p.Name, pi.Date } into q
                            select new
                            {
                                ProjectId = q.Key.ProjectId,
                                Name = q.Key.Name,
                                Date = q.Key.Date,
                                Day = q.Key.Date.Day,
                                q.Key.Date.Month,
                                q.Key.Date.Year,
                                ProjectWorkingHours = q.Sum(c => c.HoursWorked)
                            };
                
                return query.OrderBy(c => c.Month).ThenBy(c => c.Day).ToList();
            }
            else if (ownerParams.DateFormatSelected == "mm")
            {
                var query = from p in _appDbContext.Project
                            join pi in _appDbContext.Projectinfo
                               on p.ProjectId equals pi.ProjectId into q1
                            from pi in q1.DefaultIfEmpty()
                            where p.ProjectId == ownerParams.ProjectId
                            group new { p.ProjectId, p.Name, pi.HoursWorked, pi.Date } by new { p.ProjectId, p.Name, pi.Date.Month, pi.Date.Year } into q
                            select new
                            {
                                ProjectId = q.Key.ProjectId,
                                Name = q.Key.Name,
                                q.Key.Month,
                                q.Key.Year,
                                ProjectWorkingHours = q.Sum(c => c.HoursWorked)
                            };
                return query.ToList().OrderBy(c => c.Month);

            }
            else if (ownerParams.DateFormatSelected == "ww")
            {

                var query = from p in _appDbContext.Project
                            join pi in _appDbContext.Projectinfo
                               on p.ProjectId equals pi.ProjectId into q1
                            from pi in q1.DefaultIfEmpty()
                            where p.ProjectId == ownerParams.ProjectId
                            group new { p.ProjectId, p.Name, pi.HoursWorked, pi.Date } by new { p.ProjectId, p.Name, pi.Date, pi.Date.Month, pi.Date.Year } into q
                            select new
                            {

                                ProjectId = q.Key.ProjectId,
                                Name = q.Key.Name,
                                WeekNumber = ReportFacade.GetWeekNumberOfMonth(q.Key.Date),
                                q.Key.Month,
                                q.Key.Year,
                                ProjectWorkingHours = q.Sum(c => c.HoursWorked)
                            };
                var queryResult = query.ToList();


                Dictionary<int, ReportModelHelper> modelDic = new Dictionary<int, ReportModelHelper>();

                for (int i = 0; i < queryResult.Count(); i++)
                {
                    int projectId = queryResult.ElementAt(i).ProjectId;
                    int weekNumber = queryResult.ElementAt(i).WeekNumber;
                    int monthNumber = queryResult.ElementAt(i).WeekNumber;

                    ReportModelHelper model = new ReportModelHelper()
                    {
                        WeekNumber = queryResult.ElementAt(i).WeekNumber,
                        Year = queryResult.ElementAt(i).Year,
                        Month = queryResult.ElementAt(i).Month,
                        TotalWorkingHours = queryResult
                            .Where(c => c.WeekNumber == weekNumber)

                            .Sum(c => c.ProjectWorkingHours)

                    };

                    if (!modelDic.ContainsKey(model.WeekNumber))
                    {
                        modelDic.Add(model.WeekNumber, model);
                    }



                }

                return modelDic.Select(c => c.Value).OrderBy(x => x.Month).ThenBy(c => c.WeekNumber).ToList();
            }
            return null;
        }

        public dynamic GetUserEngagementByOwnerParams(UserEngagementOwnerParameters ownerParams)
        {
            if (ownerParams.DateFormatSelected == "dd")
            {
                var query = from p in _appDbContext.Project
                            join pi in _appDbContext.Projectinfo
                               on p.ProjectId equals pi.ProjectId into q1
                            from pi in q1.DefaultIfEmpty()
                            where  pi.UserId==ownerParams.UserId
                            group new { pi.UserId, pi.Date, pi.HoursWorked } by new { pi.UserId, pi.Date } into q
                            select new
                            {
                                UserId=q.Key.UserId,
                                Date = q.Key.Date,
                                Day = q.Key.Date.Day,
                                q.Key.Date.Month,
                                q.Key.Date.Year,
                                ProjectWorkingHours = q.Sum(c => c.HoursWorked)
                            };

                return query.OrderBy(c => c.Year).ThenBy(c=>c.Month).ThenBy(c => c.Day).ToList();
            }
            else if (ownerParams.DateFormatSelected == "mm")
            {
                var query = from p in _appDbContext.Project
                            join pi in _appDbContext.Projectinfo
                               on p.ProjectId equals pi.ProjectId into q1
                            from pi in q1.DefaultIfEmpty()
                            where pi.UserId == ownerParams.UserId
                            group new { pi.UserId, pi.Date, pi.HoursWorked } by new { pi.UserId, pi.Date.Month, pi.Date.Year } into q
                            select new
                            {
                                UserId=q.Key.UserId,
                                q.Key.Month,
                                q.Key.Year,
                                ProjectWorkingHours = q.Sum(c => c.HoursWorked)
                            };
                return query.OrderBy(c => c.Year).ThenBy(c => c.Month).ToList();

            }
            else if (ownerParams.DateFormatSelected == "ww")
            {

                var query = from p in _appDbContext.Project
                            join pi in _appDbContext.Projectinfo
                               on p.ProjectId equals pi.ProjectId into q1
                            from pi in q1.DefaultIfEmpty()
                            where pi.UserId==ownerParams.UserId
                            group new { pi.UserId,pi.HoursWorked, pi.Date } by new { pi.UserId, pi.Date, pi.Date.Month, pi.Date.Year } into q
                            select new
                            {
                                UserId=q.Key.UserId,
                                WeekNumber = ReportFacade.GetWeekNumberOfMonth(q.Key.Date),
                                q.Key.Month,
                                q.Key.Year,
                                ProjectWorkingHours = q.Sum(c => c.HoursWorked)
                            };
                var queryResult = query.ToList();


                Dictionary<int, ReportModelHelper> modelDic = new Dictionary<int, ReportModelHelper>();

                for (int i = 0; i < queryResult.Count(); i++)
                {
                    int weekNumber = queryResult.ElementAt(i).WeekNumber;

                    ReportModelHelper model = new ReportModelHelper()
                    {
                        WeekNumber = queryResult.ElementAt(i).WeekNumber,
                        Year = queryResult.ElementAt(i).Year,
                        Month = queryResult.ElementAt(i).Month,
                        TotalWorkingHours = queryResult
                            .Where(c => c.WeekNumber == weekNumber)

                            .Sum(c => c.ProjectWorkingHours)

                    };

                    if (!modelDic.ContainsKey(model.WeekNumber))
                    {
                        modelDic.Add(model.WeekNumber, model);
                    }



                }

                return modelDic.Select(c => c.Value).OrderBy(x => x.Month).ThenBy(c => c.WeekNumber).ToList();
            }
            return null;
        }

        public async Task<IEnumerable<ReportDataModel>> GetTotalWorkingHoursBySupervisedProjectAsync(string projectSupervisorId)
        {
            var query = from p in _appDbContext.Project
                        join pi in _appDbContext.Projectinfo
                           on p.ProjectId equals pi.ProjectId into q1
                        from pi in q1
                        where p.AssignedByUserId == projectSupervisorId
                        group new { p.ProjectId, p.Name, pi.HoursWorked } by new { p.ProjectId, p.Name } into q
                        select new
                        {
                            q.Key,
                            ProjectWorkingHours = q.Sum(c => c.HoursWorked)
                        };
            var queryResult = await query.OrderBy(c=>c.ProjectWorkingHours).ToListAsync();

            List<ReportDataModel> modelList = new List<ReportDataModel>();
            for (int i = 0; i < queryResult.Count(); i++)
            {

                int totalWorkingHours = queryResult.ElementAt(i).ProjectWorkingHours;

                if (totalWorkingHours != 0)
                {
                    ReportDataModel model = new ReportDataModel()
                    {
                        ProjectId = queryResult[i].Key.ProjectId,
                        ProjectName = queryResult[i].Key.Name,
                        TotalWorkingHours = totalWorkingHours
                    };
                    modelList.Add(model);
                }
            }
            return modelList;
        }



        public dynamic GetWorkingHoursByOwnerParamsBySupervisor(ProjectActivitiesOwnerParameters ownerParams, string supervisorId)
        {
            if (ownerParams.DateFormatSelected == "dd")
            {
                var query = from p in _appDbContext.Project
                            join pi in _appDbContext.Projectinfo
                               on p.ProjectId equals pi.ProjectId into q1
                            from pi in q1
                            where p.ProjectId == ownerParams.ProjectId  && p.AssignedByUserId==supervisorId
                            group new { p.ProjectId, p.Name, pi.HoursWorked, pi.Date } by new { p.ProjectId, p.Name, pi.Date } into q
                            select new
                            {
                                ProjectId = q.Key.ProjectId,
                                Name = q.Key.Name,
                                Date = q.Key.Date,
                                Day = q.Key.Date.Day,
                                q.Key.Date.Month,
                                q.Key.Date.Year,
                                ProjectWorkingHours = q.Sum(c => c.HoursWorked)
                            };

                return query.OrderBy(c => c.Month).ThenBy(c => c.Day).ToList();
            }
            else if (ownerParams.DateFormatSelected == "mm")
            {
                var query = from p in _appDbContext.Project
                            join pi in _appDbContext.Projectinfo
                               on p.ProjectId equals pi.ProjectId into q1
                            from pi in q1.DefaultIfEmpty()
                            where p.ProjectId == ownerParams.ProjectId && p.AssignedByUserId==supervisorId
                            group new { p.ProjectId, p.Name, pi.HoursWorked, pi.Date } by new { p.ProjectId, p.Name, pi.Date.Month, pi.Date.Year } into q
                            select new
                            {
                                ProjectId = q.Key.ProjectId,
                                Name = q.Key.Name,
                                q.Key.Month,
                                q.Key.Year,
                                ProjectWorkingHours = q.Sum(c => c.HoursWorked)
                            };
                return query.ToList().OrderBy(c => c.Month);

            }
            else if (ownerParams.DateFormatSelected == "ww")
            {

                var query = from p in _appDbContext.Project
                            join pi in _appDbContext.Projectinfo
                               on p.ProjectId equals pi.ProjectId into q1
                            from pi in q1.DefaultIfEmpty()
                            where p.ProjectId == ownerParams.ProjectId && p.AssignedByUserId == supervisorId
                            group new { p.ProjectId, p.Name, pi.HoursWorked, pi.Date } by new { p.ProjectId, p.Name, pi.Date, pi.Date.Month, pi.Date.Year } into q
                            select new
                            {

                                ProjectId = q.Key.ProjectId,
                                Name = q.Key.Name,
                                WeekNumber = ReportFacade.GetWeekNumberOfMonth(q.Key.Date),
                                q.Key.Month,
                                q.Key.Year,
                                ProjectWorkingHours = q.Sum(c => c.HoursWorked)
                            };
                var queryResult = query.ToList();


                Dictionary<int, ReportModelHelper> modelDic = new Dictionary<int, ReportModelHelper>();

                for (int i = 0; i < queryResult.Count(); i++)
                {
                    int weekNumber = queryResult.ElementAt(i).WeekNumber;

                    ReportModelHelper model = new ReportModelHelper()
                    {
                        WeekNumber = queryResult.ElementAt(i).WeekNumber,
                        Year = queryResult.ElementAt(i).Year,
                        Month = queryResult.ElementAt(i).Month,
                        TotalWorkingHours = queryResult
                            .Where(c => c.WeekNumber == weekNumber)

                            .Sum(c => c.ProjectWorkingHours)

                    };

                    if (!modelDic.ContainsKey(model.WeekNumber))
                    {
                        modelDic.Add(model.WeekNumber, model);
                    }



                }

                return modelDic.Select(c => c.Value).OrderBy(x => x.Month).ThenBy(c => c.WeekNumber).ToList();
            }
            return null;
        }

        public dynamic GetUserEngagementByOwnerParamsBySupervisor(UserEngagementOwnerParameters ownerParams, string supervisorId)
        {
            if (ownerParams.DateFormatSelected == "dd")
            {
                var query = from p in _appDbContext.Project
                            join pi in _appDbContext.Projectinfo
                               on p.ProjectId equals pi.ProjectId into q1
                            from pi in q1.DefaultIfEmpty()
                            where pi.UserId == ownerParams.UserId && p.AssignedByUserId == supervisorId
                            group new { pi.UserId, pi.HoursWorked, pi.Date } by new { pi.UserId, pi.Date } into q
                            select new
                            {
                                UserId = q.Key.UserId,
                                Date = q.Key.Date,
                                Day = q.Key.Date.Day,
                                q.Key.Date.Month,
                                q.Key.Date.Year,
                                ProjectWorkingHours = q.Sum(c => c.HoursWorked)
                            };



                return query.OrderBy(c => c.Year).ThenBy(c => c.Month).ThenBy(c => c.Day).ToList();
            }
            else if (ownerParams.DateFormatSelected == "mm")
            {
                var query = from p in _appDbContext.Project
                            join pi in _appDbContext.Projectinfo
                               on p.ProjectId equals pi.ProjectId into q1
                            from pi in q1.DefaultIfEmpty()
                            where pi.UserId == ownerParams.UserId && p.AssignedByUserId == supervisorId
                            group new { pi.UserId, pi.HoursWorked, pi.Date } by new { pi.UserId, pi.Date.Month, pi.Date.Year } into q
                            select new
                            {
                                UserId = q.Key.UserId,
                                q.Key.Month,
                                q.Key.Year,
                                ProjectWorkingHours = q.Sum(c => c.HoursWorked)
                            };
                return query.OrderBy(c => c.Year).ThenBy(c => c.Month).ToList();

            }
            else if (ownerParams.DateFormatSelected == "ww")
            {

                var query = from p in _appDbContext.Project
                            join pi in _appDbContext.Projectinfo
                               on p.ProjectId equals pi.ProjectId into q1
                            from pi in q1.DefaultIfEmpty()
                            where pi.UserId == ownerParams.UserId && p.AssignedByUserId==supervisorId
                            group new { pi.UserId, pi.HoursWorked, pi.Date } by new { pi.UserId, p.ProjectId, p.Name, pi.Date, pi.Date.Month, pi.Date.Year } into q
                            select new
                            {
                                UserId = q.Key.UserId,
                                WeekNumber = ReportFacade.GetWeekNumberOfMonth(q.Key.Date),
                                q.Key.Month,
                                q.Key.Year,
                                ProjectWorkingHours = q.Sum(c => c.HoursWorked)
                            };
                var queryResult = query.ToList();


                Dictionary<int, ReportModelHelper> modelDic = new Dictionary<int, ReportModelHelper>();

                for (int i = 0; i < queryResult.Count(); i++)
                {
                    int weekNumber = queryResult.ElementAt(i).WeekNumber;
                    int monthNumber = queryResult.ElementAt(i).WeekNumber;

                    ReportModelHelper model = new ReportModelHelper()
                    {
                        WeekNumber = queryResult.ElementAt(i).WeekNumber,
                        Year = queryResult.ElementAt(i).Year,
                        Month = queryResult.ElementAt(i).Month,
                        TotalWorkingHours = queryResult
                            .Where(c => c.WeekNumber == weekNumber)

                            .Sum(c => c.ProjectWorkingHours)

                    };

                    if (!modelDic.ContainsKey(model.WeekNumber))
                    {
                        modelDic.Add(model.WeekNumber, model);
                    }



                }

                return modelDic.Select(c => c.Value).OrderBy(x => x.Month).ThenBy(c => c.WeekNumber).ToList();
            }
            return null;
        }
    }
}
