using Kemar.TMS.Domain.ResponseModel;
using Kemar.UrgeTruck.Api.Core.Helper;
using Kemar.UrgeTruck.Domain.Common;
using Kemar.UrgeTruck.Domain.DTOs;
using Kemar.UrgeTruck.Domain.RequestModel;
using Kemar.UrgeTruck.Domain.ResponseModel;
using Kemar.UrgeTruck.Repository.Interface;
using Kemar.UrgeTruck.Repository.Repositories.Interface;
using Kemar.UrgeTruck.ServiceIntegration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kemar.UrgeTruck.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : BaseController
    {
        private readonly IUserManager _userManager;
        private readonly IEmailManager _emailManager;

        public UserManagementController(IUserManager userManagement, IEmailManager emailManager)
        {
            _userManager = userManagement;
            _emailManager = emailManager;
        }

        [HttpGet]
        [Core.Auth.Authorize]
        [Route("UsersDetail")]
        public async Task<ActionResult<RegisterUserRequest>> GetData()
        {
            var user = await _userManager.GetRegisteredUserAsync();
            return Ok(user);
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<List<AuthenticateResponse>> GetAllUsersAsync()
        {
            return await _userManager.GetAllUsersAsync();
        }

        [HttpGet]
        [Route("GetActiveUsers")]
        public async Task<List<UserResponse>> GetActiveUsersAsync()
        {
            return await _userManager.GetActiveUsersAsync();
        }

        [HttpGet]
        [Route("getUsersWithPagination")]
        public async Task<List<UserResponse>> GetUsersAsyncWithPagination([FromQuery]  int skipRow, int rowSize, int currentPage, string searchtext)
        {
            return await _userManager.GetUsersAsyncWithPagination( skipRow, rowSize, currentPage, searchtext);
        }

        [HttpPost]
        [Route("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest registerUser)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");
           
            var resultModel = await _userManager.RegisterUserAsync(registerUser);

            return resultModel.StatusCode == ResultCode.SuccessfullyCreated
                ? Ok(new { message = resultModel.ResponseMessage })
                : (IActionResult)BadRequest(resultModel.ErrorMessage);
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthenticateResponse>> Authenticate(AuthenticateRequest model)
        {
            var response = await _userManager.AuthenticateAsync(model, GetIPAddress());
            SetTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult<ForgotPasswordResponse>> ForgotPasswordAsync(ForgotPasswordRequest model)
        {
            var resultModel = await _userManager.ForgotPasswordAsync(model, Request.Headers["origin"]);

            if (string.IsNullOrEmpty(resultModel.ErrorMessage))
            {
                MailMessageDto mailMessageDto = new MailMessageDto();
                mailMessageDto.MailTo = resultModel.EmailId;
                mailMessageDto.Subject = "OTP to reset your password";
                mailMessageDto.Body = "The OTP to reset your password is " + resultModel.ForgetPasswordOTP;
                _emailManager.SendMessage(mailMessageDto);
                return Ok(resultModel);
            }
            else
                return (ActionResult)BadRequest(resultModel);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            var resultModel = await _userManager.ResetPasswordAsync(model);
            return resultModel.StatusCode == ResultCode.SuccessfullyCreated
               ? Ok(new { message = "Your password has been reset" })
               : (IActionResult)BadRequest(resultModel.ErrorMessage);
        }

        [HttpPost("change-password")]
        [Core.Auth.Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest model)
        {
            var resultModel = await _userManager.ChangePasswordAsync(model, null);
            return resultModel.StatusCode == ResultCode.SuccessfullyCreated
               ? Ok(new { message = resultModel.ResponseMessage })
               : (IActionResult)BadRequest(resultModel.ErrorMessage);
        }

        [HttpPost("reset-password-by-admin")]
        [Core.Auth.Authorize]
        public async Task<IActionResult> ResetUserPasswordByAdminAsync(ChangePasswordRequest model)
        {
            var resultModel = await _userManager.ResetUserPasswordByAdminAsync(model, null);
            return resultModel.StatusCode == ResultCode.SuccessfullyUpdated
               ? Ok(new { message = resultModel.ResponseMessage })
               : (IActionResult)BadRequest(resultModel.ErrorMessage);
        }

        [HttpPost]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] updateUserRequest registerUser)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            CommonHelper.SetUserInformation(ref registerUser, registerUser.Id, HttpContext);
            var resultModel = await _userManager.UpdateUserDeatilAsync(registerUser);

            return resultModel.StatusCode == ResultCode.SuccessfullyCreated
                ? Ok()
                : (IActionResult)BadRequest(resultModel.ErrorMessage);
        }

        [AllowAnonymous]
        [HttpPost("refreshtoken")]
        public async Task<IActionResult> RefreshToken([FromBody] RevokeTokenRequest model)
        {
            //var refreshToken = Request.Cookies["refreshToken"];
            var response = await _userManager.RefreshTokenAsync(model.Token, GetIPAddress());

            if (response == null)
                return Unauthorized(new { message = "Invalid token" });

            SetTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model)
        {
            // accept token from request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            var response = await _userManager.RevokeTokenAsync(token, GetIPAddress());

            if (!response)
                return NotFound(new { message = "Token not found" });

            return Ok(new { message = "Token revoked" });
        }

        // helper methods
        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string GetIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        [HttpGet]
        [Route("GetAllUsersByRoll")]
        public async Task<List<AuthenticateResponse>> GetAllUsersByRollAsync()
        {
            var RoleGroup = "";
            if (Request.Query.ContainsKey("RoleGroup"))
            {
                RoleGroup = Request.Query["RoleGroup"].ToString();
            }
            return await _userManager.GetAllUsersByRollAsync(RoleGroup);
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("UpdateUsersProfileImageAsync")]
        public async Task UpdateUsersProfileImageAsync()
        {
            var file = Request.Form.Files[0];
            var userId = CommonHelper.GetCurrentUserId(HttpContext);
            var result = await _userManager.UploadUserImage(file, userId);
            //return result.StatusCode == ResultCode.SuccessfullyCreated ? Ok()
            //    : (IActionResult)BadRequest(result.ErrorMessage); ;
        }

        [HttpGet]
        [Route("GetAllUsersAsyncNew")]
        public async Task<List<UserResponseNew>> GetAllUsersAsyncNew([FromQuery] int skipRow, int rowSize, int currentPage, string searchtext)
        {
            return await _userManager.GetAllUsersAsyncNew(skipRow, rowSize, currentPage, searchtext);
        }
    }
}
