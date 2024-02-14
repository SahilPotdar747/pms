using BCrypt.Net;
using Kemar.TMS.Domain.RequestModel;
using Kemar.TMS.Domain.ResponseModel;
using Kemar.TMS.Repository.Entities;
using Kemar.TMS.Repository.Interface;
using Kemar.UrgeTruck.Api.Core.Auth;
using Kemar.UrgeTruck.Api.Core.Helper;
using Kemar.UrgeTruck.Domain.Common;
using Kemar.UrgeTruck.Domain.ResponseModel;
using Kemar.UrgeTruck.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kemar.TMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class TaskTransactionController : ControllerBase
    {
        private readonly ITaskTransaction _taskTransaction;
        public TaskTransactionController(ITaskTransaction taskTransaction)
        {
            _taskTransaction = taskTransaction;
        }

        [HttpGet]
        [Route("GetAllUsersOfParentUserAsync")]
        public async Task<List<UserForTaskResponse>> GetAllUsersOfParentUserAsync()
        {
            int ParentUserId = CommonHelper.GetCurrentUserId(HttpContext);
            return await _taskTransaction.GetAllUsersOfParentUserAsync(ParentUserId);
        }

        [HttpPost]
        [Route("RegisterTaskAsync")]
        public async Task<IActionResult> RegisterTaskAsync([FromBody] TaskTransaction taskRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data model");

            CommonHelper.SetUserInformationForNewTask(ref taskRequest, taskRequest.TaskId, HttpContext);
            var resultModel = await _taskTransaction.RegisterTaskAsync(taskRequest);

            return ReturnResposneType(resultModel);
        }


        [HttpGet]
        [Route("GetAllMyTaskAsync")]
        public async Task<List<TaskTransaction>> GetAllMyTaskAsync()
        {
            int ParentUserId = CommonHelper.GetCurrentUserId(HttpContext);
            return await _taskTransaction.GetAllMyTaskAsync(ParentUserId);
        }

        [HttpGet]
        [Route("getTaskTransactionWithPagination")]
        public async Task<List<TaskTransactionResponse>> GetTaskAsyncWithPagination([FromQuery] int skipRow, int pageSize, int currentPage, string searchtext, int projectId, int assignedById, int taskTypeId, string status)
        {
            //,string projectName,string assignedBy,string taskType
            int ParentUserId = CommonHelper.GetCurrentUserId(HttpContext);
            return await _taskTransaction.GetTaskTransactionAsyncWithPagination(ParentUserId, skipRow, pageSize, currentPage, searchtext, projectId, assignedById, taskTypeId, status);
        }


        [HttpGet]
        [Route("GetAllMyTeamTaskAsync")]
        public async Task<List<TaskTransaction>> GetAllMyTeamTask()
        {
            int ParentUserId = CommonHelper.GetCurrentUserId(HttpContext);
            return await _taskTransaction.GetAllMyTeamTaskAsync(ParentUserId);
        }

        [HttpGet]
        [Route("getTeamTaskTransactionWithPagination")]
        public async Task<List<TaskTransactionResponse>> GetTeamTaskAsyncWithPagination([FromQuery] int skipRow1, int pageSize1, int currentPage1, string searchtext1, int projectId, int assignedById, int assignedTo, int taskTypeId, string status)
        {
            int ParentUserId = CommonHelper.GetCurrentUserId(HttpContext);
            return await _taskTransaction.GetTeamTaskTransactionAsyncWithPagination(ParentUserId, skipRow1, pageSize1, currentPage1, searchtext1, projectId, assignedById, assignedTo, taskTypeId, status);
        }


        [HttpGet]
        [Route("CheckIHaveTeam")]
        public async Task<bool> CheckIHaveTeam()
        {
            int ParentUserId = CommonHelper.GetCurrentUserId(HttpContext);
            return await _taskTransaction.CheckIHaveTeam(ParentUserId);
        }

        [HttpGet]
        [Route("GetAllRaisedByMeTask")]
        public async Task<List<TaskTransactionResponse>> GetAllMyraisedTeamTask()
        {
            int ParentUserId = CommonHelper.GetCurrentUserId(HttpContext);
            return await _taskTransaction.GetAllMyraisedTeamTask(ParentUserId);
        }

        [HttpGet]
        [Route("getAllRaisedTaskWithPagination")]
        public async Task<List<TaskTransactionResponse>> getAllRaisedTaskWithPagination([FromQuery] int skipRow, int pageSize, string searchtext, int projectId, int assignedToId, int taskTypeId, string status, string raisedBy)
        {
            int ParentUserId = CommonHelper.GetCurrentUserId(HttpContext);
            return await _taskTransaction.getAllRaisedTaskAsyncWithPagination(ParentUserId,skipRow, pageSize, searchtext, projectId, assignedToId, taskTypeId, status,raisedBy);
        }

        [HttpGet]
        [Route("GetAllUnAssignTask")]
        public async Task<List<TaskTransactionResponse>> GetAllUnAssignTask([FromQuery] int departmentId, int skiprow, int pagesize, int currentPage, string searchtext, int projectId, int taskTypeId)
        {
            return await _taskTransaction.GetAllUnAssignTask(departmentId, skiprow, pagesize, currentPage, searchtext, projectId, taskTypeId);
        }

        #region Completed Task

        [HttpGet]
        [Route("getCompletedTaskWithPagination")]
        public async Task<List<TaskTransactionResponse>> GetAllCompletedTaskAsyncWithPagination([FromQuery] int skipRow, int pageSize, int currentPage, string searchtext, int projectId, int assignedById, int taskTypeId, string status, string fromDate, string toDate)
        {
            //,string projectName,string assignedBy,string taskType
            int ParentUserId = CommonHelper.GetCurrentUserId(HttpContext);
            var from = fromDate.Remove(16);
            var to = toDate.Remove(16);
            var from1 = DateTime.Parse(from);
            var to1 = DateTime.Parse(to);
            return await _taskTransaction.GetAllCompletedTaskAsyncWithPagination(ParentUserId, skipRow, pageSize, currentPage, searchtext, projectId, assignedById, taskTypeId, status, from1, to1.AddDays(+1));
        }

        [HttpGet]
        [Route("getCompletedTeamTaskWithPagination")]
        public async Task<List<TaskTransactionResponse>> GetCompletedTeamTaskAsyncWithPagination([FromQuery] int skipRow1, int pageSize1, int currentPage1, string searchtext1, int projectId, int assignedById, int assignedTo, int taskTypeId, string status, string fromDate, string toDate)
        {
            int ParentUserId = CommonHelper.GetCurrentUserId(HttpContext);
            var from = fromDate.Remove(16);
            var to = toDate.Remove(16);
            var from1 = DateTime.Parse(from);
            var to1 = DateTime.Parse(to);
            return await _taskTransaction.GetCompletedTeamTaskAsyncWithPagination(ParentUserId, skipRow1, pageSize1, currentPage1, searchtext1, projectId, assignedById, assignedTo, taskTypeId, status, from1, to1.AddDays(+1));
        }

        [HttpGet]
        [Route("getCompletedRaisedTaskWithPagination")]
        public async Task<List<TaskTransactionResponse>> getCompletedRaisedTaskWithPagination([FromQuery] int skipRow, int pageSize, string searchtext, int projectId, int assignedToId, int taskTypeId, string status, string raisedBy)
        {
            int ParentUserId = CommonHelper.GetCurrentUserId(HttpContext);
            return await _taskTransaction.getCompletedRaisedTaskAsyncWithPagination(ParentUserId, skipRow, pageSize, searchtext, projectId, assignedToId, taskTypeId, status, raisedBy);
        }

        [HttpGet]
        [Route("getCompletedTaskToDownload")]
        public async Task<List<TaskTransactionResponseForDownload>> getCompletedTaskToDownload([FromQuery] string searchtext, int projectId, int assignedById, int taskTypeId, string status, string fromDate, string toDate)
        {
            //,string projectName,string assignedBy,string taskType
            int ParentUserId = CommonHelper.GetCurrentUserId(HttpContext);
            var from = fromDate.Remove(16);
            var to = toDate.Remove(16);
            var from1 = DateTime.Parse(from);
            var to1 = DateTime.Parse(to);
            return await _taskTransaction.GetAllCompletedTaskToDownloadAsync(ParentUserId, searchtext, projectId, assignedById, taskTypeId, status, from1, to1.AddDays(+1));
        }

        [HttpGet]
        [Route("getCompletedTeamTaskToDownload")]
        public async Task<List<TaskTransactionResponseForDownload>> getCompletedTeamTaskToDownload([FromQuery] string searchtext1, int projectId, int assignedById, int assignedTo, int taskTypeId, string status, string fromDate, string toDate)
        {
            int ParentUserId = CommonHelper.GetCurrentUserId(HttpContext);
            var from = fromDate.Remove(16);
            var to = toDate.Remove(16);
            var from1 = DateTime.Parse(from);
            var to1 = DateTime.Parse(to);
            return await _taskTransaction.GetCompletedTeamTaskToDownloadAsync(ParentUserId, searchtext1, projectId, assignedById, assignedTo, taskTypeId, status, from1, to1.AddDays(+1));
        }

        #endregion

        #region CoordinatingTeamTaskAsyn
        [HttpGet]
        [Route("CoordinatingTeamTaskAsyn")]
        public async Task<List<TaskTransactionResponse>> CoordinatingTeamTaskAsyn([FromQuery] int skipRow1, int pageSize1, int currentPage1, string searchtext1, int projectId, int assignedById, int assignedTo, int taskTypeId, string status, string fromDate, string toDate)
        {
            int ParentUserId = CommonHelper.GetCurrentUserId(HttpContext);
            var from = fromDate.Remove(16);
            var to = toDate.Remove(16);
            var from1 = DateTime.Parse(from);
            var to1 = DateTime.Parse(to);
            return await _taskTransaction.CoordinatingTeamTaskAsyn(ParentUserId, skipRow1, pageSize1, currentPage1, searchtext1, projectId, assignedById, assignedTo, taskTypeId, status, from1, to1.AddDays(+1));
        }

        [HttpGet]
        [Route("CoordinatingTeamTaskToDownloadAsyn")]
        public async Task<List<TaskTransactionResponseForDownload>> CoordinatingTeamTaskToDownloadAsyn([FromQuery] string searchtext1, int projectId, int assignedById, int assignedTo, int taskTypeId, string status, string fromDate, string toDate)
        {
            int ParentUserId = CommonHelper.GetCurrentUserId(HttpContext);
            var from = fromDate.Remove(16);
            var to = toDate.Remove(16);
            var from1 = DateTime.Parse(from);
            var to1 = DateTime.Parse(to);
            return await _taskTransaction.CoordinatingTeamTaskToDownloadAsyn(ParentUserId, searchtext1, projectId, assignedById, assignedTo, taskTypeId, status, from1, to1.AddDays(+1));
        }
        #endregion

        #region Reopened Task
        [HttpPost]
        [Route("ReopenCompletedTask")]
        public async Task<IActionResult> ReopenTaskAsyn([FromBody] RepoenedRequest repoenedRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data model");

            repoenedRequest.ReopedBy = CommonHelper.ReturnUserName(HttpContext);
            int userid = CommonHelper.GetCurrentUserId(HttpContext);
            var resultModel = await _taskTransaction.ReopenTask(repoenedRequest, userid);
            return ReturnResposneType(resultModel);
        }
        #endregion

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
