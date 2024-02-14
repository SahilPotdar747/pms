using Kemar.TMS.Domain.ResponseModel;
using Kemar.UrgeTruck.Domain.RequestModel;
using Kemar.UrgeTruck.Domain.ResponseModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kemar.UrgeTruck.Repository.Interface
{
    public interface IUserAccessManager
    {
        Task<ResultModel> RegisterUserAccessAsync(List<UserAccessManagerRequest> requestModel);
        Task<List<UserAccessManagerResponse>> GetAllActiveUserAccessAsync();
        Task<List<UserAccessManagerResponse>> GetAllUserAccessAsync();
        Task<UserAccessManagerResponse> GetUserAccessAsync(int accessManagerId);
        Task<List<UserAccessManagerResponse>> GetUserAccessBasedOnRoleIdAsync(int roleId);
        Task<List<UserResponse>> GetAllUserbyDepartmentAsync(int department);
    }
}
