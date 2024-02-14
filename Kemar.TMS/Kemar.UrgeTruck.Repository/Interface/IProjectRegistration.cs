using Kemar.TMS.Domain.RequestModel;
using Kemar.TMS.Domain.ResponseModel;
using Kemar.UrgeTruck.Domain.ResponseModel;
using Kemar.UrgeTruck.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.TMS.Repository.Interface
{
    public interface IProjectRegistration
    {
        Task<List<ProjectMasterResponse>> GetAllProjectAsync();
        Task<List<ProjectMasterResponse>> GetProjectAsyncWithPagination(int skipRow, int rowSize, int currentPage, string searchtext, string status);
        Task<List<ProjectMasterResponse>> GetActiveProjectAsync();
        Task<List<UserResponse>> GetManagerDDLAsync();
        Task<ResultModel> RegisterProjectAsync(ProjectMasterRequest projectRequest);
    }
}
