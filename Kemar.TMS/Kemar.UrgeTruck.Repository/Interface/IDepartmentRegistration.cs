using Kemar.TMS.Domain.RequestModel;
using Kemar.TMS.Domain.ResponseModel;
using Kemar.TMS.Repository.Entities;
using Kemar.UrgeTruck.Domain.RequestModel;
using Kemar.UrgeTruck.Domain.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.TMS.Repository.Interface
{
    public interface IDepartmentRegistration
    {
        Task<List<DepartmentMasterResponse>> GetAllDepartmentAsync();
        Task<List<DepartmentMasterResponse>> GetDeptAsyncWithPagination(int skipRow, int rowSize, int currentPage, string searchtext);
        Task<List<DepartmentMasterResponse>> GetActiveDept();
        Task<ResultModel> RegisterDeptAsync(DepartmentMasterRequest deptRequest);
    }
}
