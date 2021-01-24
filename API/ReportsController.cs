using DAL.CQRS;
using DAL.CQRS.Brokers.ReportBroker;
using DAL.CQRS.Reports;
using DAL.CQRS.Reports.Queries;
using DAL.Helpers;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    

    
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private IReportFacade _reportFacade;
        private IProjectSQLRepository _projectSQLRepository;
        private IUserSQLRepository userSQLRepository;
        private employeetimesheetdbContext _appDbContext;
        private IBroker _reportBroker;

        public ReportsController(IReportFacade _reportFacade,
                                 IProjectSQLRepository _projectSQLRepository,
                                 IUserSQLRepository userSQLRepository,
                                 IBroker _reportBroker,
                                 IQuery _query,
        employeetimesheetdbContext _appDbContext)
        {
            this._reportFacade = _reportFacade;
            this._projectSQLRepository = _projectSQLRepository;
            this.userSQLRepository = userSQLRepository;
            this._appDbContext = _appDbContext;
            this._reportBroker = _reportBroker;
        }




        /// <summary>
        /// Return workings by projects
        /// </summary>
        /// Sample request:
        ///  GET /api/reports/workingHoursByProjects
        /// <param name="queryParams"></param>
        /// <returns></returns>
        [Route("api/[controller]/admin/workingHoursByProjects")]
        [HttpGet]
        public async Task<IActionResult> GetWorkingHoursByProjects()
        {
            return Ok(await _reportFacade.GetTotalWorkingHoursByProjectAsync());
        }

        /// <summary>
        /// Return working hours by project based on query params
        /// </summary>
        /// Sample request:
        ///  GET /api/reports/admin/workingHoursBy?ProjectId=&DateFormatSelected=dd
        /// <param name="queryParams"></param>
        /// <returns></returns>
        [Route("api/[controller]/admin/workingHoursBy")]
        [HttpGet]
        public IActionResult GetWorkingHoursByProject([FromQuery] ProjectActivitiesOwnerParameters ownerParams)
        {

            if (ownerParams.ProjectId == 0 || ownerParams.DateFormatSelected.Length == 0)
            {
                return BadRequest("Please check owner parameters. Project and DateFormat must be selected");
            }
            return Ok(_reportFacade.GetWorkingHoursByOwnerParams(ownerParams));
        }

        /// <summary>
        /// Return working hours by project based on query params
        /// </summary>
        /// Sample request:
        ///  GET /api/reports/workingHoursBy?UserId=1&DateFormatSelected=dd
        /// <param name="queryParams"></param>
        /// <returns></returns>
        [Route("api/[controller]/admin/userWorkingHoursBy")]
        [HttpGet]
        public IActionResult GetWorkingHoursByUser([FromQuery] UserEngagementOwnerParameters ownerParams)
        {

            if (ownerParams.UserId.Length == 0 || ownerParams.DateFormatSelected.Length == 0)
            {
                return BadRequest("Please check owner parameters. User and DateFormat must be selected");
            }
            return Ok(_reportFacade.GetUserEngagementByOwnerParams(ownerParams));
        }


        /// <summary>
        /// Return workings by projects
        /// </summary>
        /// Sample request:
        ///  GET /api/reports/supervisor/workingHoursByProjects?SupervisorId=
        /// <param name="queryParams"></param>
        /// <returns></returns>
        [Route("api/[controller]/supervisor/workingHoursByProjects")]
        [HttpGet]
        public async Task<IActionResult> GetWorkingHoursByProjects([FromQuery] string supervisorId)
        {
            return Ok(await _reportFacade.GetTotalWorkingHoursBySupervisedProjectAsync(supervisorId));

        }

       

        /// <summary>
        /// Return working hours by project based on query params
        /// </summary>
        /// Sample request:
        ///  GET /api/reports/supervisor/workingHoursBy?SupervisorId=&ProjectId=&DateFormatSelected=dd
        /// <param name="queryParams"></param>
        /// <returns></returns>
        [Route("api/[controller]/supervisor/workingHoursBy")]
        [HttpGet]
        public IActionResult GetWorkingHoursByProject([FromQuery] string supervisorId,[FromQuery] ProjectActivitiesOwnerParameters ownerParams)
        {

            if (supervisorId.Length==0 || ownerParams.ProjectId == 0 || ownerParams.DateFormatSelected.Length == 0)
            {
                return BadRequest("Please check owner parameters Project and DateFormat must be selected");
            }
            return Ok(_reportFacade.GetWorkingHoursByOwnerParamsBySupervisor(ownerParams,supervisorId));
        }

        [Route("api/[controller]/supervisor/userWorkingHoursBy")]
        [HttpGet]
        public IActionResult GetUserWorkingHoursByProject([FromQuery] string supervisorId, [FromQuery] UserEngagementOwnerParameters ownerParams)
        {

            if (supervisorId.Length == 0 || ownerParams.UserId.Length == 0 || ownerParams.DateFormatSelected.Length == 0)
            {
                return BadRequest("Please check owner parameters Project and DateFormat must be selected");
            }
            return Ok(_reportFacade.GetUserEngagementByOwnerParamsBySupervisor(ownerParams,supervisorId));
        }

        [HttpGet]
        [Route("api/[controller]/test")]
        public async Task<IActionResult> Test()
        {
            string id = "fba01237-9967-4f4e-8d18-f9f62b8bdf54";
            IQuery query = new GetTotalProjectWorkingHoursBySupervisorAsync(id);
            return Ok(await _reportBroker.sendMessage(query));
        }




    }



}
