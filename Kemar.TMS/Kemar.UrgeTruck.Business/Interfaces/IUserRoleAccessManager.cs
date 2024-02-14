using Kemar.UrgeTruck.Domain.DTOs;
using Kemar.UrgeTruck.Domain.ResponseModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kemar.UrgeTruck.Business.Interfaces
{
    public interface IUserRoleAccessManager
    {
        Task<UserRoleAccessDto> GetUserAccessOnRoleAsync(int roleId);
        Task<List<AllUserRoleAccessDto>> GetAllUserAccessMappingAsync();
        Task<ResultModel> AssingUserRoleAccessAsync(UserRoleAccessDto userRoleAccessDto);
    }
}
