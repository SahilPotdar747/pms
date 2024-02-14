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
    public class LocationController : BaseController
    {
        private readonly ILocationRegistration _locationRegistration;
        public LocationController(ILocationRegistration locationRegistration)
        {
            _locationRegistration = locationRegistration;
        }

        [HttpPost]
        [Route("RegisterLocation")]
        public async Task<IActionResult> RegisterLocationAsync([FromBody] LocationMasterRequest locationMasterRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data model");

            CommonHelper.SetUserInformation(ref locationMasterRequest, locationMasterRequest.LocationId, HttpContext);

            var resultModel = await _locationRegistration.RegisterLocationAsync(locationMasterRequest);
            return ReturnResposneType(resultModel);
        }

        [HttpGet]
        [Route("GetAllLocations")]
        public async Task<List<LocationMasterResponse>> GetAllLocationsAsync()
        {
            return await _locationRegistration.GetAllLocationsAsync();
        }

        [HttpGet]
        [Route("GetLocation")]
        public async Task<LocationMasterResponse> GetLocationAsync(int locationId)
        {
            return await _locationRegistration.GetLocationAsync(locationId);
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
