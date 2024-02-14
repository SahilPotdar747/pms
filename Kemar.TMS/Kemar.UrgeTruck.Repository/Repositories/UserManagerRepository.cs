using AutoMapper;
using Kemar.TMS.Domain.ResponseModel;
using Kemar.TMS.Repository.Entities;
using Kemar.UrgeTruck.Domain;
using Kemar.UrgeTruck.Domain.Common;
using Kemar.UrgeTruck.Domain.DTOs;
using Kemar.UrgeTruck.Domain.RequestModel;
using Kemar.UrgeTruck.Domain.ResponseModel;
using Kemar.UrgeTruck.Repository.Context;
using Kemar.UrgeTruck.Repository.Entities;
using Kemar.UrgeTruck.Repository.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace Kemar.UrgeTruck.Repository.Repositories
{
    public class UserManagerRepository : IUserManager
    {
        private readonly IKUrgeTruckContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserManagerRepository(IKUrgeTruckContextFactory contextFactory,
            IMapper mapper,
            IConfiguration configuration)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<UserTokenDto> AuthenticateAsync(AuthenticateRequest model, string ipAddress)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();

                var userAccount = await kUrgeTruckContext.UserManager
                    .Include(x => x.DepartmentMaster).Include(x => x.DesignationMaster)
                              .Include(x => x.RoleMaster)
                              .ThenInclude(x => x.UserAccessManager.Where(x => x.IsActive == true))
                              .ThenInclude(x => x.UserScreenMaster)
                              .FirstOrDefaultAsync(x => x.UserName == model.UserName && x.IsActive == true);

                if (userAccount == null || !BC.Verify(model.Password, userAccount.PasswordHash))
                    throw new AppException(UrgeTruckMessages.User_name_or_password_is_incorrect);

                // authentication successful so generate jwt and refresh tokens
                var jwtToken = GenerateJwtToken(userAccount);
                var refreshToken = GenerateRefreshToken(ipAddress);
                userAccount.RefreshTokens.Add(refreshToken);

                // remove old refresh tokens from account
                RemoveOldRefreshTokens(userAccount);

                // save changes to db
                kUrgeTruckContext.Update(userAccount);
                await kUrgeTruckContext.SaveChangesAsync();

                var response = _mapper.Map<UserTokenDto>(userAccount);
                var allScreens = await kUrgeTruckContext.UserScreenMaster.ToListAsync();
                var userSpecificMenu = GetUserSpecificMenuAccess(userAccount.RoleMaster.UserAccessManager, allScreens);

                var menus = BuildChildMenuRecursively(userSpecificMenu, 0);

                response.MenuAccess = menus;
                response.JwtToken = jwtToken;
                response.RefreshToken = refreshToken.Token;
                response.DepartmentName = userAccount.DepartmentMaster?.DepartmentName;
                response.DesignationName = userAccount.DesignationMaster?.DesignationName;
                return response;
            }
            catch (Exception ex)
            {
                Logger.Error("Error while authenticate " + ex);
                throw;
            }
        }

        private List<UserScreenMaster> GetUserSpecificMenuAccess(ICollection<UserAccessManager> userAccessManager, List<UserScreenMaster> allUserScreens)
        {
            List<UserScreenMaster> menuDtoList = new List<UserScreenMaster>();
            List<UserScreenMaster> userScreenAccessList = new List<UserScreenMaster>();
            UserScreenMaster userScreen;
            UserScreenMaster menuDto;

            foreach (var access in userAccessManager)
            {
                userScreen = new UserScreenMaster();
                userScreen.UserScreenId = access.UserScreenMaster.UserScreenId;
                userScreen.ScreenName = access.UserScreenMaster.ScreenName;
                userScreen.ScreenCode = access.UserScreenMaster.ScreenCode;
                userScreen.MenuName = access.UserScreenMaster.MenuName;
                userScreen.ParentId = access.UserScreenMaster.ParentId;
                userScreen.RoutingURL = access.UserScreenMaster.RoutingURL;
                userScreen.IsActive = access.UserScreenMaster.IsActive;

                userScreenAccessList.Add(userScreen);
            }

            foreach (var item in allUserScreens)
            {
                var currentItem = userScreenAccessList.Where(x => x.UserScreenId == item.UserScreenId && !string.IsNullOrEmpty(item.RoutingURL) || x.ParentId == item.UserScreenId).FirstOrDefault();
                if (currentItem != null)
                {

                    menuDto = new UserScreenMaster();
                    menuDto.UserScreenId = item.UserScreenId;
                    menuDto.ScreenName = item.ScreenName;
                    menuDto.ScreenCode = item.ScreenCode;
                    menuDto.MenuName = item.MenuName;
                    menuDto.ParentId = item.ParentId;
                    menuDto.RoutingURL = item.RoutingURL;
                    menuDto.IsActive = item.IsActive;
                    menuDto.MenuIcon = item.MenuIcon;
                    menuDtoList.Add(menuDto);
                }
            }
            return menuDtoList;
        }

        private static List<MenuAccessDto> BuildChildMenuRecursively(List<UserScreenMaster> flatObjects, int? parentId = 0)
        {
            return flatObjects.Where(x => x.ParentId.Equals(parentId) && x.IsActive == true).Select(item => new MenuAccessDto
            {
                DisplayName = item.MenuName,
                MenuIcon = item.MenuIcon,
                RoutingURL = item.RoutingURL,
                Children = BuildChildMenuRecursively(flatObjects, item.UserScreenId)
            }).ToList();
        }

        public async Task<RegisterUserRequest> GetRegisteredUserAsync()
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var userResponse = await kUrgeTruckContext.UserManager.FirstOrDefaultAsync();

            return new RegisterUserRequest
            {
                FirstName = userResponse.FirstName,
                LastName = userResponse.LastName
            };
        }

        public async Task<AuthenticateResponse> GetUserTokenAsync(int userId)
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();

            var userResponse = await kUrgeTruckContext.UserManager
                .Include(x => x.RoleMaster).ThenInclude(x => x.UserAccessManager)
                .FirstOrDefaultAsync(x => x.Id == userId);

            return new AuthenticateResponse
            {
                FirstName = userResponse.UserName,
                LastName = userResponse.LastName,
                Role = userResponse.RoleMaster.RoleName,
                Email = userResponse.Email,
                Id = userResponse.Id,
                IsVerified = userResponse.IsVerified,
            };
        }

        public async Task<UserTokenDto> RefreshTokenAsync(string token, string ipAddress)
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var user = await kUrgeTruckContext.UserManager
                              .Include(x => x.RoleMaster)
                              .ThenInclude(x => x.UserAccessManager).ThenInclude(x => x.UserScreenMaster)
                              .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            // return null if no user found with token
            if (user == null) return null;

            var refreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == token);
            var dt = DateTime.Now;
            var Expires = refreshToken.Expires;
            // return null if token is no longer active
            if (!refreshToken.IsActive) return null;

            // replace old refresh token with a new one and save
            var newRefreshToken = GenerateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            user.RefreshTokens.Add(newRefreshToken);
            kUrgeTruckContext.Update(user);
            await kUrgeTruckContext.SaveChangesAsync();

            var response = _mapper.Map<UserTokenDto>(user);
            // generate new jwt
            response.JwtToken = GenerateJwtToken(user);
            response.RefreshToken = newRefreshToken.Token;

            return response;
        }

        public async Task<ResultModel> RegisterUserAsync(RegisterUserRequest registerUser)
        {
            try
            {

                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                if(registerUser.DateOfBirth == null) 
                {
                    //registerUser.DateOfBirth = DateTime.Now.AddYears(-20);
                    registerUser.DateOfBirth = Convert.ToDateTime("1900-01-01");
                }

                var userExists = kUrgeTruckContext.UserManager.Any(x => x.UserName == registerUser.UserName);
                var userPhoneExists = kUrgeTruckContext.UserManager.Any(x => x.MobileNumber == registerUser.MobileNumber);
                if (userExists==true)
                {
                    return ResultModelFactory.CreateFailure(ResultCode.DuplicateRecord, UrgeTruckMessages.UserNameExist);
                }
                if (userPhoneExists == true)
                {
                    return ResultModelFactory.CreateFailure(ResultCode.DuplicateRecord, UrgeTruckMessages.UserPhoneExist);
                }

                var userAaccount = _mapper.Map<UserManager>(registerUser);
                var isFirstAccount = kUrgeTruckContext.UserManager.Count() == 0;

                if (isFirstAccount)
                {
                    var role = await kUrgeTruckContext.RoleMaster.FirstOrDefaultAsync(x => x.RoleName == RoleConstant.SuperAdmin);
                    if (role == null)
                        return ResultModelFactory.CreateFailure(ResultCode.RecordNotFound, UrgeTruckMessages.No_super_admin_role_exists_to_create_first_user);
                    else
                        userAaccount.RoleId = role.RoleId;
                }
                else
                    userAaccount.RoleId = registerUser.RoleId;

                userAaccount.CreatedDate = DateTimeFormatter.GetISDTime(DateTime.Now);
                userAaccount.VerificationToken = RandomTokenString();

                // hash password
                userAaccount.PasswordHash = BC.HashPassword(registerUser.Password);
                userAaccount.IsActive = true;
                kUrgeTruckContext.UserManager.Add(userAaccount);
                await kUrgeTruckContext.SaveChangesAsync();
                return ResultModelFactory.CreateSucess(UrgeTruckMessages.User_registered_succesfully);
            }
            catch (Exception ex)
            {
                Logger.Error(UrgeTruckMessages.RegisterUserAsync_error + ex.Message);
                return ResultModelFactory.CreateFailure(ResultCode.ExceptionThrown, ex.Message);
            }
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest model, string origin)
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();

            UserManager userAccount = null;

            if (!string.IsNullOrEmpty(model.UserName))
            {
                userAccount = await kUrgeTruckContext.UserManager.SingleOrDefaultAsync(x =>
                     x.UserName == model.UserName);
            }

            if (userAccount == null)
                return new ForgotPasswordResponse { ErrorMessage = UrgeTruckMessages.User_Name_is_invalid };

            userAccount.ResetToken = GenerateForgotPasswordOTP();
            userAccount.ResetTokenExpires = DateTimeFormatter.GetISDTime(DateTime.Now).AddSeconds(900);

            kUrgeTruckContext.UserManager.Update(userAccount);
            await kUrgeTruckContext.SaveChangesAsync();

            return new ForgotPasswordResponse
            {
                Id = userAccount.Id,
                ForgetPasswordOTP = userAccount.ResetToken,
                ResetToken = userAccount.ResetToken,
                ResetTokenExpires = userAccount.ResetTokenExpires,
                EmailId = userAccount.Email,
                ErrorMessage = null
            };
            //ToDo: Needs to integrate email notifiation and send token in email
        }

        public async Task<ResultModel> ResetPasswordAsync(ResetPasswordRequest model)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();

                var currentTime = DateTimeFormatter.GetISDTime(DateTime.Now);

                var userAccount = await kUrgeTruckContext.UserManager.Where(x =>
                     x.ResetToken == model.Token &&
                     (x.ResetTokenExpires != null && x.ResetTokenExpires > currentTime)
                     ).FirstOrDefaultAsync();

                if (userAccount == null)
                    return ResultModelFactory.CreateFailure(ResultCode.RecordNotFound, UrgeTruckMessages.no_record_found);

                userAccount.PasswordHash = BC.HashPassword(model.Password);
                userAccount.PasswordReset = DateTimeFormatter.GetISDTime(DateTime.Now);
                userAccount.ResetToken = null;
                userAccount.ResetTokenExpires = null;

                kUrgeTruckContext.UserManager.Update(userAccount);
                await kUrgeTruckContext.SaveChangesAsync();
                return ResultModelFactory.UpdateSucess();
            }
            catch (Exception ex)
            {
                Logger.Error(UrgeTruckMessages.ResetPasswordAsync_error + ex.Message);
                return ResultModelFactory.CreateFailure(ResultCode.ExceptionThrown, ex.Message);
            }
        }

        private string GenerateJwtToken(UserManager account)
        {
            string _appSettings_Secret = KUrgeTruckContextFactory.TokenSecrete;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings_Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", account.Id.ToString()),
                    new Claim("Role", account.RoleMaster.RoleName.ToString())
                }),
                Expires = DateTimeFormatter.GetISDTime(DateTime.Now).AddMinutes(KUrgeTruckContextFactory.TokenTTL),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTimeFormatter.GetISDTime(DateTime.Now).AddMinutes(KUrgeTruckContextFactory.RefreshTokenTTL),
                Created = DateTimeFormatter.GetISDTime(DateTime.Now),
                CreatedByIp = ipAddress
            };
        }

        private void RemoveOldRefreshTokens(UserManager account)
        {
            int _appSettings_RefreshTokenTTL = 1;
            account.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.Created.AddDays(_appSettings_RefreshTokenTTL) <= DateTime.Now);
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private string GenerateForgotPasswordOTP()
        {
            int otpLength = 6;
            char[] allowedCharSet = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
            int charSetLength = allowedCharSet.Length;

            Random rand = new Random();

            StringBuilder otpBuilder = new StringBuilder();
            for (int i = 0; i < otpLength; i++)
            {
                int p = rand.Next(0, charSetLength);
                otpBuilder.Append(allowedCharSet[rand.Next(0, charSetLength)]);
            }

            return otpBuilder.ToString();
        }

        public Task<ResultModel> VerifyEmailAsync(string token)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultModel> ChangePasswordAsync(ChangePasswordRequest changePasswordRequest, string token)
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();

            var str = BC.HashPassword(changePasswordRequest.OldPassword);

            var userAccount = kUrgeTruckContext.UserManager.SingleOrDefault(x =>
                 x.UserName == changePasswordRequest.UserName);

            if (userAccount == null && !BC.Verify(changePasswordRequest.OldPassword, userAccount.PasswordHash))
                return ResultModelFactory.CreateFailure(ResultCode.RecordNotFound, UrgeTruckMessages.Invalid_user_name_or_password);

            // Check if the token is valid to verify user authenticity
            if (BC.Verify(changePasswordRequest.OldPassword, userAccount.PasswordHash))
            {
                userAccount.PasswordHash = BC.HashPassword(changePasswordRequest.Password);
                userAccount.PasswordReset = DateTimeFormatter.GetISDTime(DateTime.Now);// DateTime.UtcNow;
                userAccount.ResetToken = null;
                userAccount.ResetTokenExpires = null;

                kUrgeTruckContext.UserManager.Update(userAccount);
                await kUrgeTruckContext.SaveChangesAsync();
                return ResultModelFactory.UpdateSucess(UrgeTruckMessages.Your_password_has_been_changed_successfully);
            }
            return ResultModelFactory.CreateFailure(ResultCode.Invalid, UrgeTruckMessages.Invalid_user_name_or_password);
        }

        public async Task<ResultModel> ResetUserPasswordByAdminAsync(ChangePasswordRequest changePasswordRequest, string token)
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var userAccount = kUrgeTruckContext.UserManager.SingleOrDefault(x =>
                 x.UserName == changePasswordRequest.UserName);

            if (userAccount == null)
                return ResultModelFactory.CreateFailure(ResultCode.RecordNotFound, UrgeTruckMessages.Invalid_user_name_or_password);

            userAccount.PasswordHash = BC.HashPassword(changePasswordRequest.Password);
            userAccount.PasswordReset = DateTimeFormatter.GetISDTime(DateTime.Now);// DateTime.UtcNow;
            userAccount.ResetToken = null;
            userAccount.ResetTokenExpires = null;

            kUrgeTruckContext.UserManager.Update(userAccount);
            await kUrgeTruckContext.SaveChangesAsync();
            return ResultModelFactory.UpdateSucess(UrgeTruckMessages.Your_password_has_been_changed_successfully);
        }

        public async Task<List<AuthenticateResponse>> GetAllUsersAsync()
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var userList = await kUrgeTruckContext.UserManager.Include(x => x.RoleMaster).ToListAsync();
            return _mapper.Map<List<AuthenticateResponse>>(userList);
        }

        public async Task<List<UserResponse>> GetActiveUsersAsync()
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            //var userList = await kUrgeTruckContext.UserManager.ToListAsync();
            var userList = await kUrgeTruckContext.UserManager.OrderBy(x => x.FirstName).Where(x => x.IsActive).ToListAsync();
            return _mapper.Map<List<UserResponse>>(userList);
        }

        public async Task<List<UserResponse>> GetUsersAsyncWithPagination( int skipRow, int rowSize, int currentPage, string searchtext)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                List<UserManager> users = null;
                if (string.IsNullOrEmpty(searchtext) == false)
                    users = await kUrgeTruckContext.UserManager.Include(x => x.RoleMaster).Include(x => x.DepartmentMaster).Include(x => x.DesignationMaster).Where(x => x.UserName.Contains(searchtext)).OrderBy(x=>x.UserName).Skip(skipRow).Take(rowSize).ToListAsync();
                else
                    users = await kUrgeTruckContext.UserManager.Include(x => x.RoleMaster).Include(x => x.DepartmentMaster).Include(x => x.DesignationMaster).OrderBy(x => x.UserName).Skip(skipRow).Take(rowSize).ToListAsync();
                var response = _mapper.Map<List<UserResponse>>(users);
                if(response.Count > 0 && !string.IsNullOrEmpty(searchtext))
                    response[0].TotalRecord = await kUrgeTruckContext.UserManager.Where(x => x.UserName.Contains(searchtext)).CountAsync();
                else if (response.Count > 0)
                    response[0].TotalRecord = await kUrgeTruckContext.UserManager.CountAsync();
                return response;
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }
        public async Task<ResultModel> UpdateUserDeatilAsync(updateUserRequest model)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                var userMaster = await kUrgeTruckContext.UserManager.FirstOrDefaultAsync(x => x.Id == model.Id);

                var userPhoneExists = kUrgeTruckContext.UserManager.Any(x => x.MobileNumber == model.MobileNumber && x.Id !=model.Id);
               
                if (userPhoneExists == true)
                {
                    return ResultModelFactory.CreateFailure(ResultCode.DuplicateRecord, UrgeTruckMessages.UserPhoneExist);
                }
                userMaster.FirstName = model.FirstName;
                userMaster.LastName = model.LastName;
                userMaster.Email = model.Email;
                userMaster.RoleId = model.RoleId;
                userMaster.ReportingUser = model.ReportingUser;
                userMaster.DepartmentId = model.DepartmentId;
                userMaster.DesignationId = model.DesignationId;
                userMaster.Email = model.Email;
                userMaster.MobileNumber = model.MobileNumber;
                userMaster.ModifiedBy = model.ModifiedBy;
                userMaster.IsActive = model.IsActive;
                userMaster.empCode = model.empCode;
                userMaster.DateOfBirth = model.DateOfBirth;
                userMaster.ModifiedDate = DateTimeFormatter.GetISDTime(DateTime.Now);
                kUrgeTruckContext.UserManager.Update(userMaster);

                await kUrgeTruckContext.SaveChangesAsync();
                return ResultModelFactory.CreateSucess(UrgeTruckMessages.User_information_udpated_successfully);
            }
            catch (Exception ex)
            {
                Logger.Error(UrgeTruckMessages.UpdateUserDeatilAsync_error + ex.Message);
                return ResultModelFactory.CreateFailure(ResultCode.ExceptionThrown, UrgeTruckMessages.Error_while_addupdate_User_details, ex);
            }
        }

        public async Task<bool> RevokeTokenAsync(string token, string ipAddress)
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var user = await kUrgeTruckContext.UserManager.FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
            // return false if no user found with token
            if (user == null) return false;

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);
            // return false if token is not active
            if (!refreshToken.IsActive) return false;
            // revoke token and save
            refreshToken.Revoked = DateTimeFormatter.GetISDTime(DateTime.Now);// DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            kUrgeTruckContext.UserManager.Update(user);
            await kUrgeTruckContext.SaveChangesAsync();
            return true;
        }

        public async Task<UserTokenDto> GetUserRoleAndAccessAsync(int userId)
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();

            var userAccount = await kUrgeTruckContext.UserManager
                              .Include(x => x.RoleMaster)
                              .ThenInclude(x => x.UserAccessManager).ThenInclude(x => x.UserScreenMaster)
                              .SingleOrDefaultAsync(x => x.Id == userId);

            var response = _mapper.Map<UserTokenDto>(userAccount);
            return response;
        }

        public async Task<List<AuthenticateResponse>> GetAllUsersByRollAsync(string rollType)
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var userList = await kUrgeTruckContext.UserManager.Where(x => x.RoleMaster.RoleGroup == rollType).ToListAsync();
            return _mapper.Map<List<AuthenticateResponse>>(userList);
        }

        public async Task<ResultModel> UploadUserImage(IFormFile file, int id)
        {
            try
            {
                await GetUserDetailonIdForUpdateProfileImage(id);
                var FolderName = Path.Combine("Resource", "Image");
                var MainFolderName = _configuration.GetSection("AppSettings").GetSection("EmpProfileImageLocation").Value;
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), FolderName);
                if (file.Length > 0)
                {
                    var fileExtension = file.ContentType.Replace("image/",".") == ".jpeg" ? ".jpg" : file.ContentType.Replace("image/", ".");
                    if (fileExtension == null)
                        fileExtension = ".jpg";
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(MainFolderName, fileName);
                    //if (!string.IsNullOrEmpty(UserDetail.ProfileImagePath))
                    //{
                    //    FileInfo oldFile = new FileInfo(UserDetail.ProfileImagePath);
                    //    if (oldFile.Exists)
                    //        oldFile.Delete();
                    //}
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        await UpdateUserProfileDetail(fileName, id);
                    }
                    //UserDetail.ProfileImagePath = fullPath;
                    //kUrgeTruckContext.UserManager.Update(UserDetail);
                    //await kUrgeTruckContext.SaveChangesAsync();

                }
                else
                {
                    return ResultModelFactory.CreateFailure(ResultCode.ExceptionThrown, UrgeTruckMessages.Error_While_UploadImage);
                }

                return ResultModelFactory.CreateSucess(UrgeTruckMessages.UpdatedProfileImage);
            }
            catch (Exception ex)
            {
                return ResultModelFactory.CreateFailure(ResultCode.ExceptionThrown, UrgeTruckMessages.Error_While_UploadImage, ex);
            }
        }

        public async Task<UserManager> GetUserDetailonIdForUpdateProfileImage(int UserId)
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var userDetail = await kUrgeTruckContext.UserManager.FirstOrDefaultAsync(x => x.Id == UserId && x.IsActive);
            if (!string.IsNullOrEmpty(userDetail.ProfileImagePath))
            {
                var MainFolderName = _configuration.GetSection("AppSettings").GetSection("EmpProfileImageLocation").Value;
                var folder = MainFolderName+ "\\" +userDetail.ProfileImagePath;
                FileInfo oldFile = new FileInfo(folder);
                if (oldFile.Exists)
                    oldFile.Delete();
            }
            return userDetail;
        }

        private async Task UpdateUserProfileDetail(string path, int id)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                var userDetail = await kUrgeTruckContext.UserManager.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
                userDetail.ProfileImagePath = path;
                kUrgeTruckContext.UserManager.Update(userDetail);
                await kUrgeTruckContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ResultModelFactory.CreateFailure(ResultCode.ExceptionThrown, UrgeTruckMessages.Error_While_UploadImage, ex);
            }
        }

        public async Task<List<UserResponseNew>> GetAllUsersAsyncNew(int skipRow, int rowSize, int currentPage, string searchtext)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                List<UserManager> users = null;
                if (string.IsNullOrEmpty(searchtext) == false)
                    users = await kUrgeTruckContext.UserManager.Include(x => x.RoleMaster).Include(x => x.DepartmentMaster).Include(x => x.DesignationMaster).Where(x => x.UserName.Contains(searchtext)).OrderBy(x => x.UserName).Skip(skipRow).Take(rowSize).ToListAsync();
                else
                    users = await kUrgeTruckContext.UserManager.Include(x => x.RoleMaster).Include(x => x.DepartmentMaster).Include(x => x.DesignationMaster).OrderBy(x => x.UserName).Skip(skipRow).Take(rowSize).ToListAsync();
                var response = _mapper.Map<List<UserResponseNew>>(users);
                if (response.Count > 0 && !string.IsNullOrEmpty(searchtext))
                    response[0].TotalRecord = await kUrgeTruckContext.UserManager.Where(x => x.UserName.Contains(searchtext)).CountAsync();
                else if (response.Count > 0)
                    response[0].TotalRecord = await kUrgeTruckContext.UserManager.CountAsync();
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
