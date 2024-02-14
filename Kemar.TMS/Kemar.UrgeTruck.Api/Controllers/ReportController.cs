using Kemar.TMS.Domain.DTOs;
using Kemar.TMS.Domain.ResponseModel;
using Kemar.TMS.Repository.Interface;
using Kemar.UrgeTruck.Api.Core.Helper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kemar.TMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReport _report;

        public ReportController(IReport report)
        {
            _report = report;
        }

        #region Project Wise Report
        [HttpGet]
        [Route("GetProjectWiseTaskCountReport")]
        public async Task<List<ProjectWiseTaskCountReportResponse>> GetProjectWiseTaskCountReport([FromQuery] string fromDate, string toDate, int skipRow, int pageSize)
        {
            var from = fromDate.Remove(16);
            var to = toDate.Remove(16);
            var from1 = DateTime.Parse(from);
            var to1 = DateTime.Parse(to);
            return await _report.GetProjectWiseReport(from1, to1.AddDays(+1), skipRow, pageSize);
        }

        [HttpGet]
        [Route("GetProjectWiseTaskCountReportToDownload")]
        public async Task<List<ProjectWiseTaskCountReportResponseForDownload>> GetProjectWiseTaskCountReportTODownload([FromQuery] string fromDate, string toDate)
        {
            var from = fromDate.Remove(16);
            var to = toDate.Remove(16);
            var from1 = DateTime.Parse(from);
            var to1 = DateTime.Parse(to);
            return await _report.GetProjectWiseTaskCountReportTODownload(from1, to1.AddDays(+1));
        }

        [HttpGet]
        [Route("GetProjectWiseTask")]
        public async Task<List<TaskTransactionResponse>> GetProjectWiseTask([FromQuery] string fromdate, string todate, int skipRow, int pageSize, string searchtext, int projectId, int assignedById, int taskTypeId, int assignedTo)
        {
            var from = fromdate.Remove(16);
            var to = todate.Remove(16);
            var from1 = DateTime.Parse(from);
            var to1 = DateTime.Parse(to);
            return await _report.GetProjectWiseTask(from1, to1.AddDays(+1), skipRow, pageSize, searchtext, projectId, assignedById, taskTypeId, assignedTo);
        }

        [HttpGet]
        [Route("GetProjectWiseTaskToDownload")]
        public async Task<List<TaskTransactionResponseForDownload>> GetProjectWiseTaskToDownload([FromQuery] string fromdate, string todate, string searchtext, int projectId, int assignedById, int taskTypeId, int assignedTo)
        {
            var from = fromdate.Remove(16);
            var to = todate.Remove(16);
            var from1 = DateTime.Parse(from);
            var to1 = DateTime.Parse(to);
            return await _report.GetProjectWiseTaskToDownload(from1, to1.AddDays(+1), searchtext, projectId, assignedById, taskTypeId, assignedTo);
        }
        #endregion

        #region User Wise Report
        [HttpGet]
        [Route("GetUserWiseTaskDataCountReport")]
        public async Task<List<UserWiseTaskDataCountReportResponse>> GetUserWiseTaskDataCountReport([FromQuery] string fromDate, string toDate, int skipRow, int pageSize)
        {
            int userId = CommonHelper.GetCurrentUserId(HttpContext);
            var from = fromDate.Remove(16);
            var to = toDate.Remove(16);
            var from1 = DateTime.Parse(from);
            var to1 = DateTime.Parse(to);
            return await _report.GetUserWiseReport(userId, from1, to1.AddDays(+1), skipRow, pageSize);
        }

        [HttpGet]
        [Route("GetUserWiseTaskDataCountReportToDownload")]
        public async Task<List<UserWiseTaskDataCountReportResponseToDownload>> GetUserWiseTaskDataCountReportToDownload([FromQuery] string fromDate, string toDate)
        {
            int userId = CommonHelper.GetCurrentUserId(HttpContext);
            var from = fromDate.Remove(16);
            var to = toDate.Remove(16);
            var from1 = DateTime.Parse(from);
            var to1 = DateTime.Parse(to);
            return await _report.GetUserWiseTaskDataCountReportToDownload(userId, from1, to1.AddDays(+1));
        }

        [HttpGet]
        [Route("GetUserWiseTask")]
        public async Task<List<TaskTransactionResponse>> GetUserWiseTask([FromQuery] string fromdate, string todate, int skipRow, int pageSize, string searchtext, int projectId, int assignedById, int taskTypeId, int assignedTo)
        {
            int userId = CommonHelper.GetCurrentUserId(HttpContext);
            var from = fromdate.Remove(16);
            var to = todate.Remove(16);
            var from1 = DateTime.Parse(from);
            var to1 = DateTime.Parse(to);
            return await _report.GetUserWiseTask(userId, from1, to1.AddDays(+1), skipRow, pageSize, searchtext, projectId, assignedById, taskTypeId, assignedTo);
        }

        [HttpGet]
        [Route("GetUserWiseTaskToDownload")]
        public async Task<List<TaskTransactionResponseForDownload>> GetUserWiseTaskToDownload([FromQuery] string fromdate, string todate, string searchtext, int projectId, int assignedById, int taskTypeId, int assignedTo)
        {
            int userId = CommonHelper.GetCurrentUserId(HttpContext);
            var from = fromdate.Remove(16);
            var to = todate.Remove(16);
            var from1 = DateTime.Parse(from);
            var to1 = DateTime.Parse(to);
            return await _report.GetUserWiseTaskToDownload(userId, from1, to1.AddDays(+1), searchtext, projectId, assignedById, taskTypeId, assignedTo);
        }

        #endregion

        #region All User Wise Report
        [HttpGet]
        [Route("GetAllUserWiseTaskDataCountReport")]
        public async Task<List<UserWiseTaskDataCountReportResponse>> GetAllUserWiseTaskDataCountReport([FromQuery] string fromDate, string toDate, int skipRow, int pageSize)
        {
            var from = fromDate.Remove(16);
            var to = toDate.Remove(16);
            var from1 = DateTime.Parse(from);
            var to1 = DateTime.Parse(to);
            return await _report.GetAllUserWiseReport(from1, to1.AddDays(+1), skipRow, pageSize);
        }

        [HttpGet]
        [Route("GetAllUserWiseTaskDataCountReportToDownload")]
        public async Task<List<UserWiseTaskDataCountReportResponseToDownload>> GetAllUserWiseTaskDataCountReportToDownload([FromQuery] string fromDate, string toDate)
        {
            var from = fromDate.Remove(16);
            var to = toDate.Remove(16);
            var from1 = DateTime.Parse(from);
            var to1 = DateTime.Parse(to);
            return await _report.GetAllUserWiseCountReportToDownload(from1, to1.AddDays(+1));
        }

        [HttpGet]
        [Route("GetAllUserWiseTask")]
        public async Task<List<TaskTransactionResponse>> GetAllUserWiseTask([FromQuery] string fromdate, string todate, int skipRow, int pageSize, string searchtext, int projectId, int assignedById, int taskTypeId, int assignedTo)
        {
            var from = fromdate.Remove(16);
            var to = todate.Remove(16);
            var from1 = DateTime.Parse(from);
            var to1 = DateTime.Parse(to);
            return await _report.GetAllUserWiseTask(from1, to1.AddDays(+1), skipRow, pageSize, searchtext, projectId, assignedById, taskTypeId, assignedTo);
        }

        [HttpGet]
        [Route("GetAllUserWiseReportToDownload")]
        public async Task<List<TaskTransactionResponseForDownload>> GetAllUserWiseReportToDownload([FromQuery] string fromdate, string todate, string searchtext, int projectId, int assignedById, int taskTypeId, int assignedTo)
        {
            var from = fromdate.Remove(16);
            var to = todate.Remove(16);
            var from1 = DateTime.Parse(from);
            var to1 = DateTime.Parse(to);
            return await _report.GetAllUserWiseReportToDownload(from1, to1.AddDays(+1), searchtext, projectId, assignedById, taskTypeId, assignedTo);
        }

        #endregion

    }
}
