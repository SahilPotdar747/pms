using AutoMapper;
using Kemar.TMS.Domain.RequestModel;
using Kemar.TMS.Domain.ResponseModel;
using Kemar.TMS.Repository.Entities;
using Kemar.TMS.Repository.Interface;
using Kemar.UrgeTruck.Domain.Common;
using Kemar.UrgeTruck.Domain.ResponseModel;
using Kemar.UrgeTruck.Repository.Context;
using Kemar.UrgeTruck.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.TMS.Repository.Repositories
{
    public class DelegateHistoryRepository : IDelegateHistory
    {
        private readonly IKUrgeTruckContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly INotification _notification;
        private readonly ITaskTransaction _transaction;
        private readonly ITaskHistory _taskHistory;

        public DelegateHistoryRepository(IKUrgeTruckContextFactory contextFactory,
            IMapper mapper, INotification notification, ITaskTransaction transaction, ITaskHistory taskHistory)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _notification = notification;
            _transaction = transaction;
            _taskHistory = taskHistory;
        }

        public async Task<ResultModel> DelegateRequest(DelegateRequest delegateRequest)
        {
            var resMessage = "Task Transfer ";
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                var task = await kUrgeTruckContext.TaskTransaction.Include(x => x.TaskTypeMaster).Include(x => x.UserManager).FirstOrDefaultAsync(x => x.TaskId == delegateRequest.TaskId);

                var CheckTaskOwner = await kUrgeTruckContext.UserManager.FirstOrDefaultAsync(x => x.Id == delegateRequest.TransferToId);
                if (CheckTaskOwner.Id == task.AssignedById)
                    return ResultModelFactory.CreateFailure(ResultCode.Invalid, "Can't transfer Task to task Owner");
                var delegateCount = await kUrgeTruckContext.DelegateHistory.Where(x => x.TaskId == delegateRequest.TaskId).CountAsync();
                if (delegateCount >= 3)
                    return ResultModelFactory.CreateFailure(ResultCode.Invalid, "Same Task Can not transfer more than 3 times.");

                //var raisedUser = await kUrgeTruckContext.UserManager.FirstOrDefaultAsync(x => x.UserName == delegateRequest.RaisedBy);
                var checkParentdetail = await _transaction.GetAllUsersOfParentUserAsync(delegateRequest.RaisedById);
                var ischildUser = checkParentdetail.FirstOrDefault(x => x.Id == delegateRequest.TransferToId);

                DelegateHistory transfer = new DelegateHistory();
                transfer.RaisedBy = delegateRequest.RaisedBy;
                transfer.TaskId = delegateRequest.TaskId;
                transfer.TransferToId = delegateRequest.TransferToId;
                transfer.TransferTo = CheckTaskOwner.FirstName + " " + CheckTaskOwner.LastName;
                transfer.Remarks = delegateRequest.Remarks;
                transfer.CreatedBy = delegateRequest.CreatedBy;
                transfer.CreatedDate = delegateRequest.CreatedDate;

                if (ischildUser != null)
                {
                    transfer.Status =  DelegateStatus.Reassigned;
                    transfer.isActive = false;

                    task.AssignedTo = delegateRequest.TransferToId;
                    task.ModifiedBy = delegateRequest.RaisedBy;
                    task.ModifiedDate = DateTime.Now;
                    task.Status = task.ExceptedStartDate >= DateTime.Today ? TaskTransactionStatus.Pending : TaskTransactionStatus.NewTask;
                    kUrgeTruckContext.TaskTransaction.Update(task);

                    await _taskHistory.AddTaskHistory(task);
                }
                else
                {
                    transfer.Status = DelegateStatus.Requested;
                    transfer.isActive = true;

                    task.Status = TaskTransactionStatus.Delegated;
                    task.ModifiedBy = delegateRequest.RaisedBy;
                    task.ModifiedDate = DateTime.Now;
                    kUrgeTruckContext.TaskTransaction.Update(task);
                }

                kUrgeTruckContext.DelegateHistory.Add(transfer);
                await kUrgeTruckContext.SaveChangesAsync();

                var message = transfer.RaisedBy + " transfer " + task.Title + " to you.";
                NotificationRequest notification = new NotificationRequest();
                notification.Title = NotificationTitle.Delegate;
                notification.Message = message;
                notification.UserId = delegateRequest.TransferToId;
                notification.CreatedBy = delegateRequest.RaisedBy;
                notification.CreatedDate = DateTime.Now;

                if (transfer.RaisedBy != task.AssignedBy)
                {
                    NotificationRequest notification1 = new NotificationRequest();
                    var userName = await kUrgeTruckContext.UserManager.FirstOrDefaultAsync(x => x.Id == task.AssignedById);
                    if (userName != null)
                    {
                        //var userName1 = await kUrgeTruckContext.UserManager.FirstOrDefaultAsync(x => x.Id == delegateRequest.TransferToId);
                        var notiMessage = transfer.RaisedBy + " " + transfer.Status + " " + task.Title + " task to " + CheckTaskOwner.UserName;

                        notification1.Title = transfer.Status == DelegateStatus.Requested ? NotificationTitle.DelegatedTaskTransferReuqest : NotificationTitle.DelegatedTaskReassign;
                        notification1.Message = notiMessage;
                        notification1.UserId = userName.Id;
                        notification1.CreatedBy = "System";
                        notification1.CreatedDate = DateTime.Now;
                        await _notification.AddNewNotification(notification1);
                    }
                }

                await _notification.AddNewNotification(notification);
                resMessage = resMessage + UrgeTruckMessages.updated_successfully;
                return ResultModelFactory.UpdateSucess(resMessage);
            }
            catch (Exception ex)
            {
                Logger.Error("Error while register Delegate " + ex);
                return ResultModelFactory.CreateFailure(ResultCode.ExceptionThrown, UrgeTruckMessages.Error_while_addupdate_Task, ex);
            }
        }

        public async Task<ResultModel> DelegateAction(DelegateActionRequest delegateAction)
        {
            try
            {
                var resMessage = " ";
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                var delegateList = await kUrgeTruckContext.DelegateHistory.Include(x => x.TaskTransaction).Include(x => x.TaskTransaction.UserManager).FirstOrDefaultAsync(x => x.delegateHistoryId == delegateAction.delegateHistoryId);
                if (delegateList != null)
                {

                    delegateList.Status = delegateAction.Status;
                    delegateList.ModifiedDate = DateTime.Now;
                    delegateList.RejectRemarks = delegateAction.RejectRemarks;
                    delegateList.ModifiedBy = "System";
                    delegateList.isActive = false;

                    var notificationMessage = "Delegated Task " + delegateAction.Status + " by " + delegateList.TransferTo;

                    var task = await kUrgeTruckContext.TaskTransaction.Include(x => x.TaskTypeMaster).Include(x => x.UserManager).FirstOrDefaultAsync(x => x.TaskId == delegateList.TaskId);

                    if (delegateAction.Status == DelegateStatus.Accepted)
                        task.AssignedTo = delegateList.TransferToId;
                    task.Status = task.ExceptedStartDate >= DateTime.Today ? TaskTransactionStatus.Pending : TaskTransactionStatus.NewTask;

                    kUrgeTruckContext.TaskTransaction.Update(task);
                    NotificationRequest notification = new NotificationRequest();
                    var user = delegateList.RaisedBy.Split(' ');
                    UserManager userNameInfo = null;
                    if (user.Length == 2)
                        userNameInfo = await kUrgeTruckContext.UserManager.FirstOrDefaultAsync(x => x.FirstName == user[0] && x.LastName == user[1]);
                    else
                        userNameInfo = await kUrgeTruckContext.UserManager.FirstOrDefaultAsync(x => x.UserName == delegateList.RaisedBy);
                    notification.Title = delegateAction.Status == DelegateStatus.Accepted ? NotificationTitle.DelegatedTaskAccepted : NotificationTitle.DelegatedTaskReject;
                    notification.Message = notificationMessage;
                    notification.UserId = userNameInfo.Id;
                    notification.CreatedBy = "System";
                    notification.CreatedDate = DateTime.Now;

                    

                    kUrgeTruckContext.DelegateHistory.Update(delegateList);
                    await kUrgeTruckContext.SaveChangesAsync();

                    // Notification to Assign Emp
                    if (delegateList.RaisedBy != task.AssignedBy)
                    {
                        NotificationRequest notification1 = new NotificationRequest();
                        var userName = await kUrgeTruckContext.UserManager.FirstOrDefaultAsync(x => x.Id == task.AssignedById);
                        var notiMessage = delegateList.TransferTo + " " + delegateAction.Status + " " + task.Title + " task which was transfered by"+" "+delegateList.RaisedBy;

                        notification1.Title = delegateAction.Status == DelegateStatus.Accepted ? NotificationTitle.DelegatedTaskAccepted : NotificationTitle.DelegatedTaskReject;
                        notification1.Message = notiMessage;
                        notification1.UserId = userName.Id;
                        notification1.CreatedBy = "System";
                        notification1.CreatedDate = DateTime.Now;
                        await _notification.AddNewNotification(notification1);
                    }

                    await _notification.AddNewNotification(notification);                    
                }
                if (delegateAction.Status == DelegateStatus.Accepted)
                    resMessage = UrgeTruckMessages.Task_Added_To_Your_Task;
                else
                    resMessage = UrgeTruckMessages.Task_Rejected;
                return ResultModelFactory.UpdateSucess(resMessage);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<DelegateHistoryResponse>> GetAllMyDelegatedTask(int UserId, int skipRow, int rowSize, int currentPage, string taskStatus, DateTime fromDate, DateTime toDate)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                List<DelegateHistory> delegatedTask = null;
                var delegated = kUrgeTruckContext.DelegateHistory.Include(x => x.TaskTransaction).Include(x => x.TaskTransaction.ProjectMaster).Where(x => x.TransferToId == UserId);
                if (taskStatus == DelegateStatus.Requested)
                    delegated = delegated.Where(x => x.TransferToId == UserId && x.Status == taskStatus);
                else
                  delegated = delegated.Where(x => x.TransferToId == UserId && x.Status == taskStatus && x.CreatedDate > fromDate && x.CreatedDate < toDate);

                delegatedTask = await delegated.Skip(skipRow).Take(rowSize).ToListAsync();
                var response = _mapper.Map<List<DelegateHistoryResponse>>(delegatedTask);
                if (response.Count > 0)
                    response[0].TotalRecord = await delegated.CountAsync();
                return response;
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }

        public async Task<bool> CheckIHaveDelegateTask(int userId)
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var resultVal = false;
            var myTask = await kUrgeTruckContext.DelegateHistory.Where(x => x.TransferToId == userId).CountAsync();
            if (myTask > 0)
                resultVal = true;
            return resultVal;
        }

        public async Task<List<DelegateHistoryResponse>> GetMyRaisedDelegatedTask(string UserName, int skipRow, int rowSize, int currentPage, string taskStatus, DateTime fromDate, DateTime toDate)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();

                List<DelegateHistory> delegatedTask = null;
                var delegated = kUrgeTruckContext.DelegateHistory.Include(x => x.TaskTransaction).Include(x => x.TaskTransaction.ProjectMaster).Where(x => x.RaisedBy == UserName && x.Status == taskStatus && x.CreatedDate > fromDate && x.CreatedDate < toDate);
    
                delegatedTask = await delegated.Skip(skipRow).Take(rowSize).OrderByDescending(x=>x.TaskId) .ToListAsync();
                var response = _mapper.Map<List<DelegateHistoryResponse>>(delegatedTask);
                if (response.Count > 0)
                    response[0].TotalRecord = await delegated.CountAsync();
                return response;
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }

    }
}
