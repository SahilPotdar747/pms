using Kemar.UrgeTruck.Api.Core.Auth;
using Kemar.UrgeTruck.Domain.Common;
using Kemar.UrgeTruck.Domain.RequestModel;
using Kemar.UrgeTruck.Domain.ResponseModel;
using Kemar.UrgeTruck.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kemar.UrgeTruck.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class RoleManagerController : ControllerBase
    {
        private readonly IRoleRegistration _roleRegistration;

        public RoleManagerController(IRoleRegistration roleRegistration)
        {
            _roleRegistration = roleRegistration;
        }

        [HttpPost]
        [Route("registerrole")]
        public async Task<IActionResult> RegisterRoleAsync([FromBody] RoleMasterRequest roleRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data model");
           
            var resultModel = await _roleRegistration.RegisterRoleAsync(roleRequest);

            return ReturnResposneType(resultModel);
        }

        [HttpGet]
        [Route("getallroles")]
        public async Task<List<RoleMasterResponse>> GetAllRolesAsync()
        {
            return await _roleRegistration.GetAllRolesAsync();
        }

        [HttpGet]
        [Route("getallactiveroles")]
        public async Task<List<RoleMasterResponse>> GetAllActiveRolesAsync()
        {
            return await _roleRegistration.GetAllActiveRolesAsync();
        }

        [HttpGet]
        [Route("getrole")]
        public async Task<RoleMasterResponse> GetRoleAsync(int roleId)
        {
            return await _roleRegistration.GetRoleAsync(roleId);
        }

        [HttpGet]
        [Route("getRoleWithPagination")]
        public async Task<List<RoleMasterResponse>> GetRoleAsyncWithPagination([FromQuery] int rowSize, int currentPage, string searchtext)
        {
            return await _roleRegistration.GetRoleAsyncWithPagination(rowSize, currentPage, searchtext);
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
