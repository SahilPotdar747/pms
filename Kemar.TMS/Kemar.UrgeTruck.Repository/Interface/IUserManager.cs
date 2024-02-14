using Kemar.TMS.Domain.ResponseModel;
using Kemar.UrgeTruck.Domain.DTOs;
using Kemar.UrgeTruck.Domain.RequestModel;
using Kemar.UrgeTruck.Domain.ResponseModel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kemar.UrgeTruck.Repository.Repositories.Interface
{
    public interface IUserManager
    {
        Task<ResultModel> RegisterUserAsync(RegisterUserRequest registerUser);

        Task<AuthenticateResponse> GetUserTokenAsync(int userId);

        Task<RegisterUserRequest> GetRegisteredUserAsync();

        Task<UserTokenDto> AuthenticateAsync(AuthenticateRequest model, string ipAddress);

        Task<UserTokenDto> RefreshTokenAsync(string token, string ipAddress);

        Task<bool> RevokeTokenAsync(string token, string ipAddress);

        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest model, string origin);

        Task<ResultModel> ResetPasswordAsync(ResetPasswordRequest model);

        Task<ResultModel> VerifyEmailAsync(string token);

        Task<ResultModel> ChangePasswordAsync(ChangePasswordRequest changePasswordRequest, string token);
        Task<ResultModel> ResetUserPasswordByAdminAsync(ChangePasswordRequest changePasswordRequest, string token);

        Task<List<AuthenticateResponse>> GetAllUsersAsync();
        Task<List<UserResponse>> GetActiveUsersAsync();
        Task<List<UserResponse>> GetUsersAsyncWithPagination( int skipRow, int rowSize, int currentPage, string searchtext);
        Task<ResultModel> UpdateUserDeatilAsync(updateUserRequest model);
        Task<UserTokenDto> GetUserRoleAndAccessAsync(int userId);
        Task<List<AuthenticateResponse>> GetAllUsersByRollAsync(string rollType);
        Task<ResultModel> UploadUserImage(IFormFile file, int id);
        Task<List<UserResponseNew>> GetAllUsersAsyncNew(int skipRow, int rowSize, int currentPage, string searchtext);
    }
}
