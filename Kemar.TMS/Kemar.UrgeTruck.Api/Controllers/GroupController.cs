using Kemar.UrgeTruck.Api.Core.Auth;
using Kemar.UrgeTruck.Api.Core.Helper;
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
    [Authorize]
    public class GroupController : ControllerBase
    {

        private readonly IGroupRegistration _groupRegistration;
        public GroupController(IGroupRegistration groupRegistration)
        {
            _groupRegistration = groupRegistration;
        }

        [HttpPost]
        [Route("Registergroup")]
        public async Task<IActionResult> RegisterGroupAsync([FromBody] LocationGroupRequest locationGroupRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data model");

            CommonHelper.SetUserInformation(ref locationGroupRequest, locationGroupRequest.LocationGroupId, HttpContext);
            var resultModel = await _groupRegistration.RegisterGroupAsync(locationGroupRequest);

            return ReturnResposneType(resultModel);
        }

        [HttpGet]
        [Route("GetAllGroups")]
        public async Task<List<LocationGroupResponse>> GetAllGroupsAsync()
        {
            return await _groupRegistration.GetAllGroupsAsync();
        }

        [HttpGet]
        [Route("GetGroup")]
        public async Task<LocationGroupResponse> GetGroupAsync(int groupId)
        {
            return await _groupRegistration.GetGroupAsync(groupId);
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

            return null;
        }
    }
}


