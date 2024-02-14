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
    public interface ITaskTransaction
    {
        Task<List<UserForTaskResponse>> GetAllUsersOfParentUserAsync(int userId);
        Task<ResultModel> RegisterTaskAsync(TaskTransaction request);
        Task<List<TaskTransaction>> GetAllMyTaskAsync(int userId);
        Task<List<TaskTransactionResponse>> GetTaskTransactionAsyncWithPagination(int ParentUserId, int skipRow, int pageSize, int currentPage, string searchtext, int projectId, int assignedById, int taskTypeId, string status);
        Task<List<TaskTransactionResponse>> GetAllCompletedTaskAsyncWithPagination(int ParentUserId, int skipRow, int pageSize, int currentPage, string searchtext, int projectId, int assignedById, int taskTypeId, string status, DateTime fromDate, DateTime toDate);
        Task<List<TaskTransaction>> GetAllMyTeamTaskAsync(int userId);
        Task<List<TaskTransactionResponse>> GetTeamTaskTransactionAsyncWithPagination(int ParentUserId, int skipRow1, int pageSize1, int currentPage1, string searchtext1, int projectId, int assignedById, int assignedTo, int taskTypeId, string status);
        Task<List<TaskTransactionResponse>> GetCompletedTeamTaskAsyncWithPagination(int ParentUserId, int skipRow1, int pageSize1, int currentPage1, string searchtext1, int projectId, int assignedById, int assignedTo, int taskTypeId, string status, DateTime fromDate, DateTime toDate);
        Task<bool> CheckIHaveTeam(int userId);
        Task<List<TaskTransactionResponse>> GetAllMyraisedTeamTask(int raisedBy);
        Task<List<TaskTransactionResponse>> getAllRaisedTaskAsyncWithPagination(int ParentUserId, int skipRow, int pageSize, string searchtext, int projectId, int assignedToId, int taskTypeId, string status, string raisedBy);
        Task<List<TaskTransactionResponse>> getCompletedRaisedTaskAsyncWithPagination(int ParentUserId, int skipRow, int pageSize, string searchtext, int projectId, int assignedToId, int taskTypeId, string status, string raisedBy);
        Task<List<TaskTransactionResponse>> GetAllUnAssignTask(int departmentId, int skiprow, int pagesize, int currentPage, string searchtext, int projectId, int taskTypeId);
        Task<List<TaskTransaction>> GetNotStartedTask();
        Task updateTaskStatus(TaskTransaction taskTransaction);
        Task<List<TaskTransaction>> GetUpcommingTaskforNotification();
        Task<List<TaskTransactionResponse>> CoordinatingTeamTaskAsyn(int ParentUserId, int skipRow1, int pageSize1, int currentPage1, string searchtext1, int projectId, int assignedById, int assignedTo, int taskTypeId, string status, DateTime fromDate, DateTime toDate);
        Task<List<TaskTransactionResponseForDownload>> CoordinatingTeamTaskToDownloadAsyn(int ParentUserId, string searchtext1, int projectId, int assignedById, int assignedTo, int taskTypeId, string status, DateTime fromDate, DateTime toDate);
        Task<List<TaskTransactionResponseForDownload>> GetCompletedTeamTaskToDownloadAsync(int ParentUserId, string searchtext1, int projectId, int assignedById, int assignedTo, int taskTypeId, string status, DateTime fromDate, DateTime toDate);
        Task<List<TaskTransactionResponseForDownload>> GetAllCompletedTaskToDownloadAsync(int ParentUserId, string searchtext, int projectId, int assignedById, int taskTypeId, string status, DateTime fromDate, DateTime toDate);
        Task<ResultModel> ReopenTask(RepoenedRequest repoenedRequest, int raisedById);
    }
}
