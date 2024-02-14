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
    public interface IDesignationRegistration
    {
        Task<List<DesignationMasterResponse>> GetAllDeisgnationAsync();
        Task<List<DesignationMasterResponse>> GetDesignationAsyncWithPagination(int skipRow, int rowSize, int currentPage, string searchtext);
        Task<List<DesignationMasterResponse>> GetActiveDesignation();
        Task<ResultModel> RegisterDesignationAsync(DesignationMasterRequest designationRequest);
    }
}
