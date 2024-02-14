using Kemar.TMS.Domain.RequestModel;
using Kemar.TMS.Domain.ResponseModel;
using Kemar.TMS.Repository.Entities;
using Kemar.UrgeTruck.Domain.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.TMS.Repository.Interface
{
    public interface IDelegateHistory
    {
        Task<ResultModel> DelegateRequest(DelegateRequest delegateRequest);
        Task<ResultModel> DelegateAction(DelegateActionRequest delegateAction);
        Task<List<DelegateHistoryResponse>> GetAllMyDelegatedTask(int UserId, int skipRow, int rowSize, int currentPage, string taskStatus, DateTime fromDate, DateTime toDate);
        Task<bool> CheckIHaveDelegateTask(int userId);
        Task<List<DelegateHistoryResponse>> GetMyRaisedDelegatedTask(string UserName, int skipRow, int rowSize, int currentPage, string taskStatus, DateTime fromDate, DateTime toDate);
    }
}
