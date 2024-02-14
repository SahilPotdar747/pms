using Kemar.TMS.Domain.DTOs;
using Kemar.TMS.Domain.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.TMS.Repository.Interface
{
    public interface IReport
    {
        Task<List<ProjectWiseTaskCountReportResponse>> GetProjectWiseReport(DateTime fromdate, DateTime todate, int skipRow, int pageSize);
        Task<List<UserWiseTaskDataCountReportResponse>> GetUserWiseReport(int userId, DateTime fromdate, DateTime todate, int skipRow, int pageSize);
        Task<List<TaskTransactionResponse>> GetProjectWiseTask(DateTime fromdate, DateTime todate, int skipRow, int pageSize, string searchtext, int projectId, int assignedById, int taskTypeId, int assignedTo);
        Task<List<TaskTransactionResponse>> GetUserWiseTask(int userId, DateTime fromdate, DateTime todate, int skipRow, int pageSize, string searchtext, int projectId, int assignedById, int taskTypeId, int assignedTo);
        Task<List<UserWiseTaskDataCountReportResponse>> GetAllUserWiseReport(DateTime fromdate, DateTime todate, int skipRow, int pageSize);
        Task<List<TaskTransactionResponse>> GetAllUserWiseTask(DateTime fromdate, DateTime todate, int skipRow, int pageSize, string searchtext, int projectId, int assignedById, int taskTypeId, int assignedTo);
        Task<List<ProjectWiseTaskCountReportResponseForDownload>> GetProjectWiseTaskCountReportTODownload( DateTime fromdate, DateTime todate);
        Task<List<UserWiseTaskDataCountReportResponseToDownload>> GetUserWiseTaskDataCountReportToDownload(int userId, DateTime fromdate, DateTime todate);
        Task<List<TaskTransactionResponseForDownload>> GetUserWiseTaskToDownload(int userId, DateTime fromdate, DateTime todate, string searchtext, int projectId, int assignedById, int taskTypeId, int assignedTo);
        Task<List<TaskTransactionResponseForDownload>> GetProjectWiseTaskToDownload(DateTime fromdate, DateTime todate, string searchtext, int projectId, int assignedById, int taskTypeId, int assignedTo);
        Task<List<UserWiseTaskDataCountReportResponseToDownload>> GetAllUserWiseCountReportToDownload(DateTime fromdate, DateTime todate);
        Task<List<TaskTransactionResponseForDownload>> GetAllUserWiseReportToDownload(DateTime fromdate, DateTime todate, string searchtext, int projectId, int assignedById, int taskTypeId, int assignedTo);
    }
}
