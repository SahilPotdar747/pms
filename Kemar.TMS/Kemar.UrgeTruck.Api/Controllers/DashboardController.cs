using Microsoft.AspNetCore.Mvc;
using Kemar.TMS.Repository.Interface;
using Kemar.UrgeTruck.Domain.ResponseModel;
using Kemar.UrgeTruck.Domain.Common;
using Kemar.TMS.Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kemar.UrgeTruck.Api.Core.Helper;

namespace Kemar.TMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboard _dashboard;
        public DashboardController(IDashboard dashboard)
        {
            _dashboard = dashboard;
        }

        [HttpGet]
        [Route("GetDashboardData")]
        public async Task<List<DashboardDto>> GetDashboardData()
        {
            //string UserName = CommonHelper.ReturnUserName(HttpContext);
            //var from = fromDate.Remove(16);
            //var to = toDate.Remove(16);
            //var from1 = DateTime.Parse(from);
            //var to1 = DateTime.Parse(to);
            int userId = CommonHelper.GetCurrentUserId(HttpContext);
            return await _dashboard.GetDashboardData(userId);
        }

        [HttpGet]
        [Route("GetDashboardPiaData")]
        public async Task<List<DepartmentTaskStatusForPieChartDto>> GetDashboardPiaData([FromQuery] int departmentId, int userId)
        {
            return await _dashboard.GetDashboardPiaData(departmentId, userId);
        }

        private IActionResult ReturnResposneType(ResultModel result)
        {
            if (result.StatusCode == ResultCode.SuccessfullyCreated)
                return Created("", result);
            else if (result.StatusCode == ResultCode.SuccessfullyUpdated)
                return Ok(result);
            else if (result.StatusCode == ResultCode.Unauthorized)
                return Unauthorized(result);
            else if (result.StatusCode == ResultCode.DuplicateRecord)
                return Conflict(result);
            else if (result.StatusCode == ResultCode.RecordNotFound)
                return NotFound(result);
            else if (result.StatusCode == ResultCode.NotAllowed)
                return NotFound(result);
            else if (result.StatusCode == ResultCode.ExceptionThrown)
                return NotFound(result);
            else if (result.StatusCode == ResultCode.Invalid)
                return NotFound(result);

            return null;
        }
    }
}
