using Kemar.TMS.Domain.RequestModel;
using Kemar.TMS.Domain.ResponseModel;
using Kemar.TMS.Repository.Interface;
using Kemar.UrgeTruck.Api.Core.Auth;
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
    //[Authorize]
    public class DesignationController : ControllerBase
    {
        private readonly IDesignationRegistration _designationRegistration;

        public DesignationController(IDesignationRegistration designationRegistration) // default constructor of class
        {
            _designationRegistration = designationRegistration;
        }

        [HttpGet]
        [Route("getAllDesignation")]
        public async Task<List<DesignationMasterResponse>> GetAllDeisgnationAsync()
        {
            return await _designationRegistration.GetAllDeisgnationAsync();
        }

        [HttpGet]
        [Route("getDesignationWithPagination")]
        public async Task<List<DesignationMasterResponse>> GetDeptAsyncWithPagination([FromQuery] int skipRow, int rowSize, int currentPage, string searchtext)
        {
            return await _designationRegistration.GetDesignationAsyncWithPagination(skipRow, rowSize, currentPage, searchtext);
        }

        [HttpGet]
        [Route("getActiveDesignation")]
        public async Task<List<DesignationMasterResponse>> GetActiveDesignation()
        {
            return await _designationRegistration.GetActiveDesignation();
        }

        [HttpPost]
        [Route("registerDesignation")]
        public async Task<IActionResult> RegisterDeptAsync([FromBody] DesignationMasterRequest designationRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Data Model");
            CommonHelper.SetUserInformation(ref designationRequest, designationRequest.DesignationId, HttpContext);
            var resultModel = await _designationRegistration.RegisterDesignationAsync(designationRequest);

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
