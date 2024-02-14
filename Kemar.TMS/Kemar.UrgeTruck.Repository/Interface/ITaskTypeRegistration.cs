using Kemar.TMS.Domain.RequestModel;
using Kemar.TMS.Domain.ResponseModel;
using Kemar.UrgeTruck.Domain.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.TMS.Repository.Interface
{
    public interface ITaskTypeRegistration
    {
        Task<List<TaskTypeMasterResponse>> GetAllTaskTypeAsync();
        Task<List<TaskTypeMasterResponse>> GetAllActiveTaskTypeAsync();
        Task<List<TaskTypeMasterResponse>> GetTaskTypeAsyncWithPagination(int rowSize, int skipRow, int currentPage, string searchText, int departmentId);
        Task<ResultModel> RegisterTaskTypeAsync(TaskTypeMasterRequest taskTypeRequest);
        Task<List<TaskTypeMasterResponse>> GetAllTaskTypeDepartmentWiseAsync(int departmentId);
    }
}
