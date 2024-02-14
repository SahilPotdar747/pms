using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Kemar.TMS.Repository.Interface;
using Kemar.UrgeTruck.Repository.Interface;
using System.Threading.Tasks;
using System.Collections.Generic;
using Kemar.TMS.Repository.Entities;
using Kemar.TMS.Domain.ResponseModel;
using Kemar.UrgeTruck.Api.Core.Auth;
using Kemar.UrgeTruck.Domain.ResponseModel;
using Kemar.TMS.Domain.RequestModel;
using Kemar.UrgeTruck.Domain.Common;
using Kemar.UrgeTruck.Api.Core.Helper;

namespace Kemar.TMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRegistration _departmentRegistration;

        public DepartmentController(IDepartmentRegistration departmentRegistration) // default constructor of class
        {
            _departmentRegistration = departmentRegistration;
        }

        [HttpGet]
        [Route("getAllDept")]
        public async Task<List<DepartmentMasterResponse>> GetAllDepartmentAsync()
        {
            return await _departmentRegistration.GetAllDepartmentAsync();
        }

        [HttpGet]
        [Route("getDeptWithPagination")]
        public async Task<List<DepartmentMasterResponse>> GetDeptAsyncWithPagination([FromQuery] int skipRow, int rowSize, int currentPage, string searchtext)
        {
            return await _departmentRegistration.GetDeptAsyncWithPagination(skipRow, rowSize, currentPage, searchtext);
        }

        [HttpGet]
        [Route("getActiveDept")]
        public async Task<List<DepartmentMasterResponse>> GetActiveDept()
        {
            return await _departmentRegistration.GetActiveDept();
        }

        [HttpPost]
        [Route("registerDepartment")]
        public async Task<IActionResult> RegisterDeptAsync([FromBody] DepartmentMasterRequest deptRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Data Model");
            CommonHelper.SetUserInformation(ref deptRequest, deptRequest.DepartmentId, HttpContext);
            var resultModel = await _departmentRegistration.RegisterDeptAsync(deptRequest);

            return ReturnResposneType(resultModel);
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
