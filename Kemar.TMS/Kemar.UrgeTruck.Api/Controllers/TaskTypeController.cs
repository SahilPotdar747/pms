using Kemar.TMS.Domain.RequestModel;
using Kemar.TMS.Domain.ResponseModel;
using Kemar.TMS.Repository.Interface;
using Kemar.UrgeTruck.Api.Core.Helper;
using Kemar.UrgeTruck.Domain.Common;
using Kemar.UrgeTruck.Domain.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kemar.TMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTypeController : ControllerBase
    {
        private readonly ITaskTypeRegistration _taskTypeRegistration;

        public TaskTypeController(ITaskTypeRegistration taskTypeRegistration) // default constructor of class
        {
            _taskTypeRegistration = taskTypeRegistration;
        }

        [HttpGet]
        [Route("getAllTask")]
        public async Task<List<TaskTypeMasterResponse>> GetAllTaskTypeAsync()
        {
            return await _taskTypeRegistration.GetAllTaskTypeAsync();
        }

        [HttpGet]
        [Route("getActiveTask")]
        public async Task<List<TaskTypeMasterResponse>> GetAllActiveTaskTypeAsync()
        {
            return await _taskTypeRegistration.GetAllActiveTaskTypeAsync();
        }

        [HttpGet]
        [Route("getTaskWithPagination")]
        public async Task<List<TaskTypeMasterResponse>> GetTaskTypeAsyncWithPagination([FromQuery] int rowSize, int skipRow, int currentPage, string searchtext, int departmentId)
        {
            return await _taskTypeRegistration.GetTaskTypeAsyncWithPagination(rowSize, skipRow, currentPage, searchtext, departmentId);
        }

        [HttpPost]
        [Route("registerTaskType")]
        public async Task<IActionResult> RegisterTaskTypeAsync([FromBody] TaskTypeMasterRequest taskTypeRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Data Model");
            CommonHelper.SetUserInformation(ref taskTypeRequest, taskTypeRequest.TaskId, HttpContext);
            var resultModel = await _taskTypeRegistration.RegisterTaskTypeAsync(taskTypeRequest);

            return ReturnResposneType(resultModel);
        }

        [HttpGet]
        [Route("getAllTaskTypeDepartmentWiseAsync")]
        public async Task<List<TaskTypeMasterResponse>> GetAllTaskTypeDepartmentWiseAsync([FromQuery] int departmentId)
        {
            return await _taskTypeRegistration.GetAllTaskTypeDepartmentWiseAsync(departmentId);
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

            return null;
        }
    }
}
