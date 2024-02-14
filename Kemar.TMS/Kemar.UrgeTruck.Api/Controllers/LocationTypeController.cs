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
    public class LocationTypeController : BaseController
    {
        private readonly ILocationTypeRegistration _locationTypeRegistration;
        public LocationTypeController(ILocationTypeRegistration locationTypeRegistration)
        {
            _locationTypeRegistration = locationTypeRegistration;
        }

        [HttpPost]
        [Route("RegisterLocationType")]
        public async Task<IActionResult> RegisterLocationTypeAsync([FromBody] LocationTypeRequest locationTypeRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data model");

            CommonHelper.SetUserInformation(ref locationTypeRequest, locationTypeRequest.LocationTypeId, HttpContext);

            var resultModel = await _locationTypeRegistration.RegisterLocationTypeAsync(locationTypeRequest);

            return ReturnResposneType(resultModel);
               
        }

        [HttpGet]
        [Route("GetAllLocationTypes")]
        public async Task<List<LocationTypeResponse>> GetAllLocationTypesAsync()
        {
            return await _locationTypeRegistration.GetAllLocationTypesAsync(); 
        }

        [HttpGet]
        [Route("GetLocationType")]
        public async Task<LocationTypeResponse> GetLocationTypeAsync(int locationTypeId)
        {
            return await _locationTypeRegistration.GetLocationTypeAsync(locationTypeId);
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
