using AutoMapper;
using Kemar.UrgeTruck.Domain.Common;
using Kemar.UrgeTruck.Domain.RequestModel;
using Kemar.UrgeTruck.Domain.ResponseModel;
using Kemar.UrgeTruck.Repository.Context;
using Kemar.UrgeTruck.Repository.Entities;
using Kemar.UrgeTruck.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.UrgeTruck.Repository.Repositories
{
    public class UserScreenRepository : IUserScreen
    {
        private readonly IKUrgeTruckContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public UserScreenRepository(IKUrgeTruckContextFactory contextFactory,
            IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<List<UserScreenMasterResponse>> GetAllActiveUserScreensAsync()
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var userScreens = await kUrgeTruckContext.UserScreenMaster.Where(x => x.IsActive == true && !string.IsNullOrEmpty(x.ScreenName)).ToListAsync();
            return _mapper.Map<List<UserScreenMasterResponse>>(userScreens);
        }

        public async Task<List<UserScreenMasterResponse>> GetAllUserScreensAsync()
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var userScreens = await kUrgeTruckContext.UserScreenMaster.ToListAsync();
            return _mapper.Map<List<UserScreenMasterResponse>>(userScreens);
        }

        public async Task<UserScreenMasterResponse> GetUserScreenAsync(int screenId)
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var userScreen = await kUrgeTruckContext.UserScreenMaster.FirstOrDefaultAsync(x => x.UserScreenId == screenId);
            return _mapper.Map<UserScreenMasterResponse>(userScreen);
        }

        public async Task<List<UserRoleScreenMappingResponse>> GetUserdetailAsync()
        {
            List<UserRoleScreenMappingResponse> userRoleScreenMappingResponse = new List<UserRoleScreenMappingResponse>();

            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var userScreen = await kUrgeTruckContext.RoleMaster.ToListAsync();

            var list = await GetAllActiveUserAccessAsync();

            foreach (var userscreen in userScreen)
            {
                var roles = list.Where(x => x.RoleId == userscreen.RoleId).ToList();
                var pagedescription = string.Empty;

                StringBuilder sb = new StringBuilder();
                foreach (var role in roles)
                {
                    var canfunction = string.Empty;

                    if (role.CanCreate == true)
                        canfunction = "create";
                    else if (role.CanDeactivate == true && role.CanCreate == false)
                        canfunction = "delete";
                    else
                        canfunction = "Read";

                    sb.Append(role.ScreenName + "(" + canfunction + "),");
                }
                if (!string.IsNullOrEmpty(sb.ToString()))
                {
                    pagedescription = sb.ToString().Remove(sb.Length - 1);
                }

                userRoleScreenMappingResponse.Add(new UserRoleScreenMappingResponse
                {
                    RoleName = userscreen.RoleName,
                    PageAcessDescription = pagedescription,
                    IsActive = userscreen.IsActive,
                    RoleId = userscreen.RoleId,
                    PageCount = roles.Count()

                });
            }
            return userRoleScreenMappingResponse;
        }

        public async Task<List<UserAccessManagerResponse>> GetAllActiveUserAccessAsync()
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var userAccessList = await kUrgeTruckContext.UserAccessManager.Include(x => x.UserScreenMaster).Where(x => x.IsActive == true).ToListAsync();
            return _mapper.Map<List<UserAccessManagerResponse>>(userAccessList);
        }

        public async Task<ResultModel> RegisterUserScreenAsync(UserScreenMasterRequest request)
        {
            var resMessage = UrgeTruckMessages.User_screen;
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                var userScreenMaster = await kUrgeTruckContext.UserScreenMaster
                                                  .FirstOrDefaultAsync(x => x.UserScreenId == request.UserScreenId || x.ScreenCode == request.ScreenCode);
                if (userScreenMaster == null)
                {
                    kUrgeTruckContext.Add(_mapper.Map<UserScreenMaster>(request));
                    resMessage = resMessage + UrgeTruckMessages.added_successfully;
                }
                else
                {
                    userScreenMaster.ScreenName = request.ScreenName;
                    userScreenMaster.ScreenCode = request.ScreenCode;
                    userScreenMaster.IsActive = request.IsActive;
                    resMessage = resMessage + UrgeTruckMessages.updated_successfully;

                    kUrgeTruckContext.UserScreenMaster.Update(userScreenMaster);
                }

                await kUrgeTruckContext.SaveChangesAsync();
                return ResultModelFactory.CreateSucess(resMessage);
            }
            catch (Exception ex)
            {
                Logger.Error("Error while register user screen " + ex);
                return ResultModelFactory.CreateFailure(ResultCode.ExceptionThrown, UrgeTruckMessages.Error_while_addupdate_user_screen, ex);
            }
        }
    }
}
