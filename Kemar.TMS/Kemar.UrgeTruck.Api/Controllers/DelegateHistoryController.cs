using Kemar.TMS.Domain.RequestModel;
using Kemar.TMS.Domain.ResponseModel;
using Kemar.TMS.Repository.Entities;
using Kemar.TMS.Repository.Interface;
using Kemar.UrgeTruck.Api.Core.Auth;
using Kemar.UrgeTruck.Api.Core.Helper;
using Kemar.UrgeTruck.Domain.Common;
using Kemar.UrgeTruck.Domain.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Kemar.TMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class DelegateHistoryController : ControllerBase
    {
        private readonly IDelegateHistory _delegateHistory;
        public DelegateHistoryController(IDelegateHistory delegateHistory)
        {
            _delegateHistory = delegateHistory;
        }

        [HttpPost]
        [Route("RegisterDelegateAsync")]
        public async Task<IActionResult> RegisterDelegateAsync([FromBody] DelegateRequest delegateRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data model");

            CommonHelper.SetUserInformation(ref delegateRequest, delegateRequest.delegateHistoryId, HttpContext);
            delegateRequest.RaisedBy = CommonHelper.ReturnUserName(HttpContext);
            delegateRequest.RaisedById = CommonHelper.GetCurrentUserId(HttpContext);
            var resultModel = await _delegateHistory.DelegateRequest(delegateRequest);
            return ReturnResposneType(resultModel);
        }

        [HttpPost]
        [Route("DelegateActionAsync")]
        public async Task<IActionResult> DelegateActionAsync([FromBody] DelegateActionRequest delegateRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data model");

            var resultModel = await _delegateHistory.DelegateAction(delegateRequest);
            return ReturnResposneType(resultModel);
        }

        [HttpGet]
        [Route("GetAllMyDelegatedTask")] //delegated for me
        public async Task<List<DelegateHistoryResponse>> GetAllMyDelegatedTask([FromQuery] int skipRow, int rowSize, int currentPage, string taskStatus, string fromDate, string toDate)
        {
            try
            {
                int userId = CommonHelper.GetCurrentUserId(HttpContext);
                var from = fromDate.Remove(16);
                var to = toDate.Remove(16);
                var from1 = DateTime.Parse(from);
                var to1 = DateTime.Parse(to);
                return await _delegateHistory.GetAllMyDelegatedTask(userId,skipRow, rowSize, currentPage, taskStatus, from1, to1.AddDays(+1));
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpGet]
        [Route("CheckIHaveDelegateTask")]
        public async Task<bool> CheckIHaveDelegateTask()
        {
            int userId = CommonHelper.GetCurrentUserId(HttpContext);
            return await _delegateHistory.CheckIHaveDelegateTask(userId);
        }

        [HttpGet]
        [Route("GetMyRaisedDelegateTask")] //delegated by me
        public async Task<List<DelegateHistoryResponse>> GetMyRaisedDelegateTask([FromQuery] int skipRow, int rowSize, int currentPage, string taskStatus, string fromDate, string toDate)
        {
            string UserName = CommonHelper.ReturnUserName(HttpContext);
            var from = fromDate.Remove(16);
            var to = toDate.Remove(16);
            var from1 = DateTime.Parse(from);
            var to1 = DateTime.Parse(to);
            return await _delegateHistory.GetMyRaisedDelegatedTask(UserName, skipRow, rowSize, currentPage, taskStatus,from1,to1.AddDays(+1));
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
