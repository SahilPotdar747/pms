using Kemar.TMS.Domain.ResponseModel;
using Kemar.TMS.Repository.Interface;
using Kemar.UrgeTruck.Api.Core.Helper;
using Kemar.UrgeTruck.Domain.Common;
using Kemar.UrgeTruck.Domain.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kemar.TMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotification _notification;

        public NotificationController(INotification notification)
        {
            _notification = notification;
        }

        [HttpGet]
        [Route("GetMyNotification")]
        public async Task<List<NotificationResponse>> GetMyNotification()
        {
            int userId = CommonHelper.GetCurrentUserId(HttpContext);
            return await _notification.GetMyNotification(userId);
        }

        [HttpGet]
        [Route("GetCurrentNotifications")]
        public async Task<List<NotificationResponse>> GetCurrentNotifications([FromQuery] int count)
        {
            int userId = CommonHelper.GetCurrentUserId(HttpContext);
            return await _notification.GetCurrentNotifications(userId, count);
        }

        [HttpDelete]
        [Route("CloseNotificationByUser/{notificationId}")]
        public async Task<IActionResult> CloseNotificationByUser(int notificationId)
        {
            var resultModel = await _notification.CloseNotificationByUser(notificationId);
            return ReturnResposneType(resultModel);
        }

        [HttpGet]
        [Route("GetMyNotificationCount")]
        public async Task<int> GetMyNotificationCount()
        {
            int userId = CommonHelper.GetCurrentUserId(HttpContext);
            return await _notification.GetMyNotificationCount(userId);
        }

        [HttpGet]
        [Route("GetMyNotificationCountasync")]
        public async Task<NotificationCount> GetMyNotificationCountAsync()
        {
            int userId = CommonHelper.GetCurrentUserId(HttpContext);
            return await _notification.GetMyNotificationCountAsync(userId);
        }

        [HttpGet]
        [Route("GetDesktopNotifications")]
        public async Task<List<NotificationResponse>> GetDesktopNotifications()
        {
            int userId = CommonHelper.GetCurrentUserId(HttpContext);
            return await _notification.GetDesktopNotifications(userId);
        }

        [HttpGet]
        [Route("GetMobileNotifications")]
        public async Task<List<NotificationResponse>> GetMobileNotifications()
        {
            int userId = CommonHelper.GetCurrentUserId(HttpContext);
            return await _notification.GetMobileNotifications(userId);
        }

        [HttpDelete]
        [Route("CloseNotificationByUserForDesktop/{notificationId}")]
        public async Task<IActionResult> CloseNotificationByUserForDesktop(int notificationId)
        {
            var resultModel = await _notification.CloseNotificationByUserForDesktop(notificationId);
            return ReturnResposneType(resultModel);
        }

        [HttpDelete]
        [Route("CloseNotificationByUserForMobile/{notificationId}")]
        public async Task<IActionResult> CloseNotificationByUserForMobile(int notificationId)
        {
            var resultModel = await _notification.CloseNotificationByUserForMobile(notificationId);
            return ReturnResposneType(resultModel);
        }

        [HttpDelete]
        [Route("CloseAllNotificationByUser")]
        public async Task<IActionResult> CloseAllNotificationByUserForMobile()
        {
            int userId = CommonHelper.GetCurrentUserId(HttpContext);
            var resultModel = await _notification.CloseAllNotificationByUser(userId);
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

            return null;
        }
    }
}
