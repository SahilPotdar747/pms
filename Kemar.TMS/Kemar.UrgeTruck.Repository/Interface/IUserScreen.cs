using Kemar.UrgeTruck.Domain.RequestModel;
using Kemar.UrgeTruck.Domain.ResponseModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kemar.UrgeTruck.Repository.Interface
{
    public interface IUserScreen
    {
        Task<ResultModel> RegisterUserScreenAsync(UserScreenMasterRequest request);
        Task<List<UserScreenMasterResponse>> GetAllActiveUserScreensAsync();
        Task<List<UserScreenMasterResponse>> GetAllUserScreensAsync();
        Task<UserScreenMasterResponse> GetUserScreenAsync(int screenId);
        Task<List<UserRoleScreenMappingResponse>> GetUserdetailAsync();
    }
}
