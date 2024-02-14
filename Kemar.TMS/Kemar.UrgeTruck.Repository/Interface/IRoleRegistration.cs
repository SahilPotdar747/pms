using Kemar.UrgeTruck.Domain.RequestModel;
using Kemar.UrgeTruck.Domain.ResponseModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kemar.UrgeTruck.Repository.Interface
{
    public interface IRoleRegistration
    {
        Task<ResultModel> RegisterRoleAsync(RoleMasterRequest request);
        Task<List<RoleMasterResponse>> GetAllActiveRolesAsync();
        Task<List<RoleMasterResponse>> GetAllRolesAsync();
        Task<RoleMasterResponse> GetRoleAsync(int roleId, string roleName = null);
        Task<List<RoleMasterResponse>> GetRoleAsyncWithPagination( int rowSize, int currentPage, string searchtext);
    }
}
