using AutoMapper;
using Kemar.UrgeTruck.Business.Interfaces;
using Kemar.UrgeTruck.Domain.Common;
using Kemar.UrgeTruck.Domain.DTOs;
using Kemar.UrgeTruck.Domain.RequestModel;
using Kemar.UrgeTruck.Domain.ResponseModel;
using Kemar.UrgeTruck.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.UrgeTruck.Business.BusinessLogic
{
    public class UserRoleAccessManager : IUserRoleAccessManager
    {
        private readonly IUserAccessManager _userAccessManager;
        private readonly IUserScreen _userScreen;
        private readonly IMapper _mapper;
        private readonly IRoleRegistration _roleRegistration;
        public UserRoleAccessManager(IUserScreen userScreen,
            IUserAccessManager userAccessManager,
            IMapper mapper,
            IRoleRegistration roleRegistration)
        {
            _userAccessManager = userAccessManager;
            _userScreen = userScreen;
            _mapper = mapper;
            _roleRegistration = roleRegistration;
        }

        public async Task<UserRoleAccessDto> GetUserAccessOnRoleAsync(int roleId)
        {
            return await ExtractUserAccess(roleId);
        }
        public async Task<ResultModel> AssingUserRoleAccessAsync(UserRoleAccessDto userRoleAccessDto)
        {
            List<UserAccessManagerRequest> request = new List<UserAccessManagerRequest>();

            var isRoleAddUpdated = await RegisterAndUpdateRole(userRoleAccessDto);

            foreach (var roleAccess in userRoleAccessDto.UserAccessManagerResponse)
            {    // Deactive all access
                if (isRoleAddUpdated && userRoleAccessDto.IsActive == false && roleAccess.UserAccessManagerId > 0)
                {
                    roleAccess.IsActive = false;
                    request.Add(_mapper.Map<UserAccessManagerRequest>(roleAccess));
                } // Active back all access
                else if (isRoleAddUpdated && userRoleAccessDto.IsActive == true && (roleAccess.UserAccessManagerId > 0 || roleAccess.CanCreate == true || roleAccess.CanUpdate == true || roleAccess.CanDeactivate == true))
                {
                    roleAccess.IsActive = true;
                    roleAccess.RoleId = userRoleAccessDto.RoleId;
                    request.Add(_mapper.Map<UserAccessManagerRequest>(roleAccess));
                } // Add / update option
                else if (userRoleAccessDto.IsActive != false && (roleAccess.UserAccessManagerId > 0 || roleAccess.CanCreate == true || roleAccess.CanUpdate == true || roleAccess.CanDeactivate == true || roleAccess.IsActive == true))
                {
                    roleAccess.RoleId = userRoleAccessDto.RoleId;
                    request.Add(_mapper.Map<UserAccessManagerRequest>(roleAccess));
                }
            }
            if (request.Count > 0)
            {
                var respone = await _userAccessManager.RegisterUserAccessAsync(request);
                return respone;
            }
            else if (userRoleAccessDto.IsActive == false)
                return ResultModelFactory.CreateFailure(ResultCode.ExceptionThrown, "Role is InActive can't update");
            return null;
        }

        private async Task<UserRoleAccessDto> ExtractUserAccess(int roleId)
        {
            UserRoleAccessDto userAcessManager = new UserRoleAccessDto();
            UserAccessManagerResponse userAccessResponse;
            var allUserScreens = await _userScreen.GetAllUserScreensAsync();

            if (roleId > 0)
            {
                userAcessManager = await GetRoles(roleId, userAcessManager);
                if (userAcessManager == null)
                    return null; // This means this role does not exist in DB.
            }

            var activeScreens = await _userScreen.GetAllActiveUserScreensAsync();
            var currentUserAcess = await _userAccessManager.GetUserAccessBasedOnRoleIdAsync(roleId);
            UserScreenMasterResponse parentmenu = new UserScreenMasterResponse();


            foreach (var screen1 in currentUserAcess)
            {
                parentmenu = allUserScreens.Where(x => x.UserScreenId == screen1.ParentId).FirstOrDefault();
                screen1.ParentName = parentmenu != null ? parentmenu.MenuName : "";
            }


            foreach (var screen in activeScreens)
            {
                parentmenu = null;
                var isAccess = currentUserAcess.Any(x => x.UserScreenId == screen.UserScreenId);
                if (!isAccess)
                {
                    if (screen.ParentId > 0)
                    {
                        parentmenu = allUserScreens.Where(x => x.UserScreenId == screen.ParentId).FirstOrDefault();
                    }

                    userAccessResponse = new UserAccessManagerResponse();
                    userAccessResponse.UserScreenId = screen.UserScreenId;
                    userAccessResponse.ScreenCode = screen.ScreenCode;
                    userAccessResponse.ParentName = parentmenu != null ? parentmenu.MenuName : "";
                    userAccessResponse.ScreenName = screen.MenuName;
                    userAccessResponse.CanCreate = false;
                    userAccessResponse.CanUpdate = false;
                    userAccessResponse.CanDeactivate = false;
                    userAccessResponse.RoleId = roleId;
                    currentUserAcess.Add(userAccessResponse);
                }
            }
            userAcessManager.UserAccessManagerResponse = currentUserAcess;
            return userAcessManager;
        }

        private async Task<UserRoleAccessDto> GetRoles(int roleId, UserRoleAccessDto userAcessManager)
        {
            var role = await _roleRegistration.GetRoleAsync(roleId);
            if (role == null)
                return null; // mention there is no role 
            userAcessManager.RoleId = role.RoleId;
            userAcessManager.RoleName = role.RoleName;
            userAcessManager.IsActive = role.IsActive;
            return userAcessManager;
        }

        private async Task<bool> RegisterAndUpdateRole(UserRoleAccessDto userAcessManager, bool isActiveUpdate = false)
        {
            bool result = false;
            var role = await _roleRegistration.GetRoleAsync(userAcessManager.RoleId);
            if (role == null)
                role = await _roleRegistration.GetRoleAsync(0, userAcessManager.RoleName);

            if (role != null)
            {
                userAcessManager.RoleId = role.RoleId;
                userAcessManager.RoleName = role.RoleName;
            }

            if (role != null && role.IsActive != userAcessManager.IsActive)
            {
                var updateRole = _mapper.Map<RoleMasterRequest>(role);
                if (userAcessManager.RoleName.ToUpper() != role.RoleName.ToUpper())
                    role.RoleName = userAcessManager.RoleName;

                updateRole.IsActive = userAcessManager.IsActive;
                var response = await _roleRegistration.RegisterRoleAsync(updateRole);
                if (response.StatusCode == ResultCode.SuccessfullyCreated || response.StatusCode == ResultCode.SuccessfullyUpdated)
                    result = true;
                else
                    return false;
            }
            else if (role != null && userAcessManager.RoleName.ToUpper() != role.RoleName.ToUpper())
            {
                var currentRole = new RoleMasterRequest();
                currentRole.RoleName = userAcessManager.RoleName;
                currentRole.RoleId = role.RoleId;
                var response = await _roleRegistration.RegisterRoleAsync(currentRole);
                if (response.StatusCode == ResultCode.SuccessfullyCreated || response.StatusCode == ResultCode.SuccessfullyUpdated)
                    result = true;
            }
            else if (role == null)
            {
                var currentRole = new RoleMasterRequest();
                currentRole.RoleName = userAcessManager.RoleName;
                currentRole.IsActive = true;
                var response = await _roleRegistration.RegisterRoleAsync(currentRole);
                if (response.StatusCode == ResultCode.SuccessfullyCreated || response.StatusCode == ResultCode.SuccessfullyUpdated)
                {
                    userAcessManager.RoleId = currentRole.RoleId;
                    userAcessManager.IsActive = true;
                    result = true;
                }
            }
            return result;
        }

        public async Task<List<AllUserRoleAccessDto>> GetAllUserAccessMappingAsync()
        {
            List<AllUserRoleAccessDto> allUserRoleAccessLists = new List<AllUserRoleAccessDto>();
            AllUserRoleAccessDto allUserRoleAccess;
            StringBuilder roleAccess;

            var accessRights = await _userAccessManager.GetAllUserAccessAsync();
            var getallroles = await _roleRegistration.GetAllActiveRolesAsync();

            try
            {
                if (accessRights != null && accessRights.Count > 0)
                {
                    var distinctRoles = accessRights.Select(x => x.RoleId).Distinct().ToList();
                    foreach (var roleId in distinctRoles)
                    {
                        allUserRoleAccess = new AllUserRoleAccessDto();
                        var userAccess = accessRights.Where(x => x.RoleId == roleId).ToList();
                        //Get role from rolemaster
                        var role = await _roleRegistration.GetRoleAsync(roleId);

                        allUserRoleAccess.RoleId = role.RoleId;
                        allUserRoleAccess.RoleName = role.RoleName;
                        allUserRoleAccess.Status = role.IsActive;
                        roleAccess = new StringBuilder();
                        foreach (var access in userAccess)
                        {
                            if (access.IsActive || access.CanCreate || access.CanUpdate || access.CanDeactivate)
                            {
                                roleAccess.Append(access.ScreenName);
                                roleAccess.Append(" (");
                                if (access.IsActive)
                                    roleAccess.Append("R,");
                                if (access.CanCreate)
                                    roleAccess.Append("C,");
                                if (access.CanUpdate)
                                    roleAccess.Append("U,");
                                if (access.CanDeactivate)
                                    roleAccess.Append("D");
                                char lastChar = roleAccess.ToString()[roleAccess.ToString().Length - 1];
                                if (lastChar == ',')
                                    roleAccess.Remove(roleAccess.Length - 1, 1);

                                roleAccess.Append("), ");
                            }
                        }
                        int count = userAccess.Where(x => x.IsActive || x.CanCreate || x.CanUpdate || x.CanDeactivate).Count();

                        allUserRoleAccess.Count = count;
                        
                        if (!string.IsNullOrEmpty(roleAccess.ToString()))
                        {
                            char closeChar = roleAccess.ToString()[roleAccess.ToString().Length - 1];
                            if (closeChar == ',')
                                roleAccess.Remove(roleAccess.Length - 1, 1);
                        }

                        allUserRoleAccess.UserAccess = roleAccess.ToString();
                        allUserRoleAccessLists.Add(allUserRoleAccess);
                    }
                }

                foreach (var item in getallroles)
                {
                    var user = allUserRoleAccessLists.Where(x => x.RoleId == item.RoleId).FirstOrDefault();
                    if (user == null)
                    {
                        allUserRoleAccessLists.Add(new AllUserRoleAccessDto
                        {
                            RoleId = item.RoleId,
                            RoleName = item.RoleName,
                            Status = item.IsActive
                        });
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }

            return allUserRoleAccessLists.OrderBy(x => x.RoleName).ToList();
        }
    }
}
