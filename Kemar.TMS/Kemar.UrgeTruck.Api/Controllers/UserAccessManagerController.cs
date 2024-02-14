using Kemar.TMS.Domain.ResponseModel;
using Kemar.UrgeTruck.Business.Interfaces;
using Kemar.UrgeTruck.Domain.Common;
using Kemar.UrgeTruck.Domain.DTOs;
using Kemar.UrgeTruck.Domain.ResponseModel;
using Kemar.UrgeTruck.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kemar.UrgeTruck.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccessManagerController : ControllerBase
    {
        private readonly IUserAccessManager _userAccessManager;
        private readonly IUserRoleAccessManager _userRoleAccessManager;

        public UserAccessManagerController(IUserAccessManager userAccessManager,
           IUserRoleAccessManager userRoleAccessManager)
        {
            _userAccessManager = userAccessManager;
            _userRoleAccessManager = userRoleAccessManager;
        }

        [HttpGet]
        [Route("getUserAccessbyRole")]
        public async Task<UserRoleAccessDto> GetUserAccessBasedOnRoleIdAsync(int roleId)
        {
            return await _userRoleAccessManager.GetUserAccessOnRoleAsync(roleId);
        }

        [HttpGet]
        [Route("getAllUserRoleAccess")]
        public async Task<List<AllUserRoleAccessDto>> GetAllUserRoleAccessAsync()
        {
            return await _userRoleAccessManager.GetAllUserAccessMappingAsync();
        }

        [HttpGet]
        [Route("getAllUserbyDepartment")]
        public async Task<List<UserResponse>> GetAllUserbyDepartmentAsync([FromQuery] int departmentId)
        {
            return await _userAccessManager.GetAllUserbyDepartmentAsync(departmentId);
        }

        [HttpPost]
        [Route("assignUserAccessRole")]
        public async Task<IActionResult> RegisterUserAccessAsync([FromBody] UserRoleAccessDto userRoleAccessDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data model");

            var resultModel = await _userRoleAccessManager.AssingUserRoleAccessAsync(userRoleAccessDto);

            return ReturnResposneType(resultModel);
        }

        [HttpGet]
        [Route("getalluseraccess")]
        public async Task<List<UserAccessManagerResponse>> GetAllUserAccessAsync()
        {
            return await _userAccessManager.GetAllUserAccessAsync();
        }

        [HttpGet]
        [Route("getallactiveuseraccess")]
        public async Task<List<UserAccessManagerResponse>> GetAllActiveUserAccessAsync()
        {
            return await _userAccessManager.GetAllActiveUserAccessAsync();
        }

        [HttpGet]
        [Route("getuseraccess")]
        public async Task<UserAccessManagerResponse> GetUserAccessAsync(int userAccessManagerId)
        {
            return await _userAccessManager.GetUserAccessAsync(userAccessManagerId);
        }

        private IActionResult ReturnResposneType(ResultModel result)
        {
            if (result == null)
                return NotFound("Invalid data in request");
            else if (result.StatusCode == ResultCode.SuccessfullyCreated)
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
