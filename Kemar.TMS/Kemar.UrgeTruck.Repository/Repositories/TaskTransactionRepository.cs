using AutoMapper;
using Kemar.TMS.Domain.RequestModel;
using Kemar.TMS.Domain.ResponseModel;
using Kemar.TMS.Repository.Entities;
using Kemar.TMS.Repository.Interface;
using Kemar.UrgeTruck.Domain.Common;
using Kemar.UrgeTruck.Domain.ResponseModel;
using Kemar.UrgeTruck.Domain.UserManagement;
using Kemar.UrgeTruck.Repository.Context;
using Kemar.UrgeTruck.Repository.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.TMS.Repository.Repositories
{
    public class TaskTransactionRepository : ITaskTransaction
    {
        private readonly IKUrgeTruckContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly INotification _notification;
        private readonly ITaskHistory _taskHistory;

        public TaskTransactionRepository(IKUrgeTruckContextFactory contextFactory,
            IMapper mapper, INotification notification, ITaskHistory taskHistory)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _notification = notification;
            _taskHistory = taskHistory;
        }

        #region GetAllUsersOfParentUserAsync
        public async Task<List<UserForTaskResponse>> GetAllUsersOfParentUserAsync(int userId)
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();

            List<UserForTaskResponse> users = new List<UserForTaskResponse>();
            var user = await kUrgeTruckContext.UserManager.Where(x => x.Id == userId && x.IsActive).ToListAsync();
            if (user != null && user.Count > 0)
                users.Add(_mapper.Map<UserForTaskResponse>(user.First()));

            GetReportingUsers(kUrgeTruckContext, userId, users);

            return users;
        }
        #endregion

        #region GetReportingUsers
        private void GetReportingUsers(KUrgeTruckContext kUrgeTruckContext, int userId, List<UserForTaskResponse> userList)
        {
            List<UserManager> repUsers = kUrgeTruckContext.UserManager.Where(x => x.ReportingUser == userId).ToList();
            if (repUsers != null && repUsers.Count > 0)
            {
                foreach (var user in repUsers)
                {
                    userList.Add(_mapper.Map<UserForTaskResponse>(user));
                    GetReportingUsers(kUrgeTruckContext, user.Id, userList);
                }
            }
        }
        #endregion


        #region RegisterTaskAsync
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResultModel> RegisterTaskAsync(TaskTransaction request)
        {
            var resMessage = "Task ";
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                //var userRoleExists = kUrgeTruckContext.TaskTransaction.Any(x => x.Title == request.Title && x.TaskId != request.TaskId);
                //if (userRoleExists == true)
                //    return ResultModelFactory.CreateFailure(ResultCode.DuplicateRecord, UrgeTruckMessages.Task_Title_Exist);
                if (request.TaskId == 0)
                {
                    request.AssignedDate = DateTime.Now;
                    if ((request.DepartmentId == null || request.DepartmentId == 0) && (request.AssignedTo > 0 || request.AssignedTo != null))
                    {
                        var userInfo = await kUrgeTruckContext.UserManager.FirstOrDefaultAsync(x => x.Id == request.AssignedTo);
                        if (userInfo != null)
                            request.DepartmentId = userInfo.DepartmentId;
                    }
                    if (request.AssignedTo > 0 || request.AssignedTo != null)
                        request.Status = request.ExceptedStartDate <= DateTime.Now ? TaskTransactionStatus.Pending : TaskTransactionStatus.NewTask;
                    if (request.taskNumber == null || request.taskNumber == 0)
                        request.taskNumber = await GenerateTaskNumber();
                    kUrgeTruckContext.TaskTransaction.Add(request);
                    if (request.AssignedTo > 0 && (request.AssignedTo != request.AssignedById))
                    {
                        NotificationRequest notification = new NotificationRequest();
                        notification.Title = NotificationTitle.NewTask;
                        notification.Message = request.Title;
                        notification.UserId = (int)request.AssignedTo;
                        notification.CreatedBy = request.CreatedBy;
                        notification.CreatedDate = DateTime.Now;

                        await _notification.AddNewNotification(notification);
                    }
                    resMessage = resMessage + UrgeTruckMessages.added_successfully;
                }
                else
                {
                    var Task = await kUrgeTruckContext.TaskTransaction.Include(x => x.TaskTypeMaster).Include(x => x.UserManager).FirstOrDefaultAsync(x => x.TaskId == request.TaskId);
                    await _taskHistory.AddTaskHistory(Task);
                    if (Task != null)
                    {
                        Task.Title = request.Title;
                        Task.ProjectId = request.ProjectId;
                        Task.AssignedTo = request.AssignedTo;
                        Task.Description = request.Description;
                        Task.TaskTypeId = request.TaskTypeId;
                        Task.Priority = request.Priority;
                        Task.IsActive = request.IsActive;
                        Task.ExceptedEndDate = request.ExceptedEndDate;
                        Task.ExceptedStartDate = request.ExceptedStartDate;

                        if (request.Status == TaskTransactionStatus.Completed && Task.ActualStartDate == null)
                            Task.ActualStartDate = DateTime.Now;

                        switch (request.Status)
                        {
                            case TaskTransactionStatus.Completed:
                                Task.ActualEndDate = DateTime.Now;
                                if (Task.AssignedTo != Task.AssignedById)
                                {
                                    NotificationRequest notification = new NotificationRequest();
                                    notification.Title = NotificationTitle.TaskCompleted;
                                    notification.Message = request.Title;
                                    notification.UserId = (int)request.AssignedById;
                                    notification.CreatedBy = request.CreatedBy;
                                    notification.CreatedDate = DateTime.Now;
                                    await _notification.AddNewNotification(notification);
                                }
                                if (Task.TaskTypeMaster.NextTaskId != 0 && Task.TaskTypeMaster.NextTaskId != null &&
                                    (Task.Status != TaskTransactionStatus.Completed || Task.Status != TaskTransactionStatus.Closed
                                    || Task.Status != TaskTransactionStatus.Canceled || Task.Status != TaskTransactionStatus.Invalid))
                                    await AutoGeneratedTask(Task);
                                Task.Status = request.Status;
                                break;
                            case TaskTransactionStatus.WIP:
                                Task.ActualStartDate = DateTime.Now;
                                Task.Status = request.Status;
                                break;
                            case TaskTransactionStatus.WIPShort:
                                Task.ActualStartDate = DateTime.Now;
                                Task.Status = request.Status;
                                break;
                            case TaskTransactionStatus.NewTask:
                                request.Status = request.Status;
                                break;
                            case TaskTransactionStatus.UnAssigned:
                                request.Status = request.ExceptedStartDate >= DateTime.Now ? TaskTransactionStatus.Pending : TaskTransactionStatus.NewTask;
                                break;
                            default:
                                Task.Status = request.Status;
                                break;
                        }
                        if (Task.Status == request.Status && request.AssignedTo > 0)
                            Task.Status = request.Status;
                        else if (TaskTransactionStatus.UnAssigned == "UnAssigned" && request.Status == request.Status && Task.Status == Task.Status && request.AssignedTo > 0)
                            Task.Status = request.ExceptedStartDate >= DateTime.Now ? TaskTransactionStatus.Pending : TaskTransactionStatus.NewTask;
                            //else if (TaskTransactionStatus.UnAssigned == "Unassigned"&& request.AssignedTo > 0)
                            //    Task.Status = request.ExceptedStartDate >= DateTime.Now ? TaskTransactionStatus.Pending : TaskTransactionStatus.NewTask;
                        else if (request.AssignedTo == 0 && Task.AssignedTo == 0)
                            Task.AssignedTo = null;
                            //request.AssignedTo = null;
                        kUrgeTruckContext.TaskTransaction.Update(Task);
                        resMessage = resMessage + UrgeTruckMessages.updated_successfully;
                        
                    }
                }
                await kUrgeTruckContext.SaveChangesAsync();
                return ResultModelFactory.UpdateSucess(resMessage);

            }
            catch (Exception ex)
            {
                Logger.Error("Error while register Task " + ex);
                return ResultModelFactory.CreateFailure(ResultCode.ExceptionThrown, UrgeTruckMessages.Error_while_addupdate_Task, ex);
            }
        }
        #endregion

        #region GetAllMyTeamTaskAsync
        public async Task<List<TaskTransaction>> GetAllMyTeamTaskAsync(int userId)
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();

            var myTeam = await GetAllUsersOfParentUserAsync(userId);
            //var myQuery = from task in kUrgeTruckContext.TaskTransaction
            //              join team in myTeam on task.AssignedTo equals team.Id
            //              join pm  in kUrgeTruckContext.ProjectMaster on  task.ProjectId equals pm.ProjectId

            //              select  task;
            var myUserList = myTeam.Where(x => x.Id != userId);

            var myTask = kUrgeTruckContext.TaskTransaction
                .Include(x => x.ProjectMaster)
                .Include(x => x.TaskTypeMaster)
                .Include(x => x.UserManager);
            //.Where(x => x.AssignedTo == userId);
            //.ToListAsync();

            var mytask2 = from task in myTask.ToList()
                          join team in myUserList on task.AssignedTo equals team.Id
                          select task;
            return mytask2.ToList();
        }
        #endregion

        #region GetAllMyTaskAsync
        public async Task<List<TaskTransaction>> GetAllMyTaskAsync(int userId)
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var myTask = await kUrgeTruckContext.TaskTransaction.Include(x => x.ProjectMaster).Include(x => x.TaskTypeMaster).Include(x => x.UserManager).Where(x => x.AssignedTo == userId).ToListAsync();
            return myTask;
        }
        #endregion

        #region CheckIHaveTeam
        public async Task<bool> CheckIHaveTeam(int userId)
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var resultVal = false;
            var myTask = await kUrgeTruckContext.UserManager.Where(x => x.ReportingUser == userId).CountAsync();
            if (myTask > 0)
                resultVal = true;
            return resultVal;
        }
        #endregion

        #region GetTaskTransactionAsyncWithPagination
        public async Task<List<TaskTransactionResponse>> GetTaskTransactionAsyncWithPagination(int ParentUserId, int skipRow, int pageSize, int currentPage, string searchtext, int projectId, int assignedById, int taskTypeId, string status)

        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                List<TaskTransaction> tasktransaction = null;
                var task = kUrgeTruckContext.TaskTransaction.Include(x => x.ProjectMaster).Include(x => x.TaskTypeMaster).Include(x => x.UserManager).Where(x => x.AssignedTo == ParentUserId);
                if (string.IsNullOrEmpty(searchtext) == false)
                    task = task.Where(x => x.Title.Contains(searchtext));
                if (projectId > 0)
                    task = task.Where(x => x.ProjectId == projectId);
                if (assignedById > 0)
                    task = task.Where(x => x.AssignedById == assignedById);
                if (taskTypeId > 0)
                    task = task.Where(x => x.TaskTypeId == taskTypeId);
                if (status != null)
                    task = task.Where(x => x.Status == status);
                else
                    task = task.Where(x => x.Status != TaskTransactionStatus.Completed && x.Status != TaskTransactionStatus.Canceled);
                tasktransaction = await task.OrderByDescending(x => x.AssignedDate).Skip(skipRow).Take(pageSize).ToListAsync();
                var response = _mapper.Map<List<TaskTransactionResponse>>(tasktransaction);
                //if (response.Count > 0 && string.IsNullOrEmpty(searchtext))
                //    response[0].TotalRecord = await kUrgeTruckContext.TaskTransaction.Where(x => x.AssignedTo == ParentUserId).CountAsync();
                //else if (response.Count > 0)
                if (response.Count > 0)
                    response[0].TotalRecord = await task.CountAsync();
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region GetTeamTaskTransactionAsyncWithPagination
        public async Task<List<TaskTransactionResponse>> GetTeamTaskTransactionAsyncWithPagination(int ParentUserId, int skipRow1, int pageSize1, int currentPage1, string searchtext1, int projectId, int assignedById, int assignedTo, int taskTypeId, string status)
        {
            try
            {
                //var skipRow = (currentPage - 1) * pageSize;
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                var myTeam = await GetAllUsersOfParentUserAsync(ParentUserId);
                var myUserList = myTeam.Where(x => x.Id != ParentUserId);
                List<TaskTransaction> myTask = null;
                if (string.IsNullOrEmpty(searchtext1) == false)
                    myTask = await kUrgeTruckContext.TaskTransaction.Include(x => x.ProjectMaster).Include(x => x.TaskTypeMaster).Include(x => x.UserManager).Where(x => x.Title.Contains(searchtext1)).ToListAsync();
                else
                    myTask = await kUrgeTruckContext.TaskTransaction.Include(x => x.ProjectMaster).Include(x => x.TaskTypeMaster).Include(x => x.UserManager).ToListAsync();
                var mytask2 = from task in myTask.ToList()
                              join team in myUserList on task.AssignedTo equals team.Id
                              select task;
                if (projectId > 0)
                    mytask2 = mytask2.Where(x => x.ProjectId == projectId);
                if (assignedById > 0)
                    mytask2 = mytask2.Where(x => x.AssignedById == assignedById);
                if (assignedTo > 0)
                    mytask2 = mytask2.Where(x => x.AssignedTo == assignedTo);
                if (taskTypeId > 0)
                    mytask2 = mytask2.Where(x => x.TaskTypeId == taskTypeId);
                //if (status != null)
                //    mytask2 = mytask2.Where(x => x.Status == status);
                //else
                //    mytask2 = mytask2.Where(x => x.Status != TaskTransactionStatus.Completed);
                if (status != null)
                    mytask2 = mytask2.Where(x => x.Status == status);
                else
                    mytask2 = mytask2.Where(x => x.Status != TaskTransactionStatus.Completed && x.Status != TaskTransactionStatus.Canceled);
                var mytask3 = mytask2.OrderByDescending(x => x.AssignedDate).Skip(skipRow1).Take(pageSize1).ToList();
                var response = _mapper.Map<List<TaskTransactionResponse>>(mytask3);
                if (response.Count > 0)
                    response[0].TotalRecord = mytask2.Count(); ;
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region GetAllMyraisedTeamTask
        public async Task<List<TaskTransactionResponse>> GetAllMyraisedTeamTask(int raisedBy)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                List<TaskTransaction> tasktransaction = new List<TaskTransaction>();
                tasktransaction = await kUrgeTruckContext.TaskTransaction.Include(x => x.ProjectMaster).Include(x => x.TaskTypeMaster).Include(x => x.UserManager).Where(x => x.AssignedTo == raisedBy).OrderByDescending(x => x.TaskId).ToListAsync();

                var response = _mapper.Map<List<TaskTransactionResponse>>(tasktransaction);
                if (response.Count > 0)
                    response[0].TotalRecord = tasktransaction.Count();
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region GetAllUnAssignTask
        public async Task<List<TaskTransactionResponse>> GetAllUnAssignTask(int departmentId, int skiprow, int pagesize, int currentPage, string searchtext, int projectId, int taskTypeId)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                List<TaskTransaction> task = null;
                //List<TaskTransaction> task1 = new List<TaskTransaction>();
                IQueryable<TaskTransaction> task1 = kUrgeTruckContext.TaskTransaction.Include(x => x.ProjectMaster).Include(x => x.TaskTypeMaster)
                    .Include(x => x.DepartmentMaster).Include(x => x.UserManager).OrderByDescending(x => x.AssignedDate)
                    .Where(x => (x.AssignedTo == null || x.AssignedTo == 0) && x.DepartmentId == departmentId);
                if (string.IsNullOrEmpty(searchtext) == false)
                    task1 = task1.Where(x => x.Title.Contains(searchtext));
                if (projectId > 0)
                    task1 = task1.Where(x => x.ProjectId == projectId);
                if (taskTypeId > 0)
                    task1 = task1.Where(x => x.TaskTypeId == taskTypeId);

                task = await task1.Skip(skiprow).Take(pagesize).OrderByDescending(x => x.TaskId).ToListAsync();

                var response = _mapper.Map<List<TaskTransactionResponse>>(task);
                if (response.Count > 0)

                    response[0].TotalRecord = await task1.CountAsync();
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region getAllRaisedTaskAsyncWithPagination
        public async Task<List<TaskTransactionResponse>> getAllRaisedTaskAsyncWithPagination(int ParentUserId, int skipRow, int pageSize, string searchtext, int projectId, int assignedToId, int taskTypeId, string status, string raisedBy)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                List<TaskTransaction> tasktransaction = null;
                var task = kUrgeTruckContext.TaskTransaction.Include(x => x.ProjectMaster).Include(x => x.TaskTypeMaster).Include(x => x.UserManager).OrderByDescending(x => x.AssignedDate).Where(x => x.AssignedById == ParentUserId);
                if (string.IsNullOrEmpty(searchtext) == false)
                    task = task.Where(x => x.Title.Contains(searchtext));
                if (projectId > 0)
                    task = task.Where(x => x.ProjectId == projectId);
                if (assignedToId > 0)
                    task = task.Where(x => x.AssignedTo == assignedToId);
                if (taskTypeId > 0)
                    task = task.Where(x => x.TaskTypeId == taskTypeId);
                if (status != null)
                    task = task.Where(x => x.Status == status);
                else
                    task = task.Where(x => x.Status != TaskTransactionStatus.Completed && x.Status != TaskTransactionStatus.Canceled);
                //else
                //    task = task.Where(x => x.Status != TaskTransactionStatus.Completed && x.Status != TaskTransactionStatus.Canceled);
                tasktransaction = await task.OrderByDescending(x => x.AssignedDate).Skip(skipRow).Take(pageSize).ToListAsync();
                var response = _mapper.Map<List<TaskTransactionResponse>>(tasktransaction);
                if (response.Count > 0)
                    response[0].TotalRecord = await task.CountAsync();
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region GetNotStartedTask
        public async Task<List<TaskTransaction>> GetNotStartedTask()
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                var taskList = await kUrgeTruckContext.TaskTransaction.OrderByDescending(x => x.AssignedDate).Where(x => x.Status == TaskTransactionStatus.NewTask || x.Status == TaskTransactionStatus.Pending).ToListAsync();
                return taskList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region updateTaskStatus
        public async Task updateTaskStatus(TaskTransaction taskTransaction)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                kUrgeTruckContext.TaskTransaction.Update(taskTransaction);
                await kUrgeTruckContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetUpcommingTaskforNotification
        public async Task<List<TaskTransaction>> GetUpcommingTaskforNotification()
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                var time = DateTime.Now;
                var lastTime = time.AddHours(+2);
                var tasklist = await kUrgeTruckContext.TaskTransaction.Where(x => x.Status == TaskTransactionStatus.NewTask || x.Status == TaskTransactionStatus.Pending && x.ExceptedStartDate >= time && x.ExceptedStartDate <= lastTime).ToListAsync();
                return tasklist;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region AutoGeneratedTask
        public async Task AutoGeneratedTask(TaskTransaction task)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                var nextAction = await kUrgeTruckContext.TaskTypeMaster.FirstOrDefaultAsync(x => x.TaskId == task.TaskTypeMaster.NextTaskId);
                TaskTransaction task1 = new TaskTransaction();
                if (nextAction == null)
                    return;
                task1.Title = task.Title;
                task1.ProjectId = task.ProjectId;
                task1.TaskTypeId = nextAction.TaskId;
                task1.Description = task.Description;
                task1.Priority = task.Priority;
                task1.AssignedBy = "Auto Generated";
                task1.AssignedDate = DateTime.Now;
                task1.Status = TaskTransactionStatus.NewTask;
                task1.IsActive = true;
                task1.CreatedBy = "Auto Generated";
                task1.CreatedDate = DateTime.Now;
                task1.DepartmentId = nextAction.DepartmentId;
                task1.taskNumber = task.taskNumber;

                await RegisterTaskAsync(task1);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Completed Task
        public async Task<List<TaskTransactionResponse>> GetAllCompletedTaskAsyncWithPagination(int ParentUserId, int skipRow, int pageSize, int currentPage, string searchtext, int projectId, int assignedById, int taskTypeId, string status, DateTime fromDate, DateTime toDate)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                List<TaskTransaction> tasktransaction = null;
                var task = kUrgeTruckContext.TaskTransaction.Include(x => x.ProjectMaster).Include(x => x.TaskTypeMaster).Include(x => x.UserManager).OrderByDescending(x => x.AssignedDate).Where(x => x.AssignedTo == ParentUserId);
                if (string.IsNullOrEmpty(searchtext) == false)
                    task = task.Where(x => x.Title.Contains(searchtext));
                if (projectId > 0)
                    task = task.Where(x => x.ProjectId == projectId);
                if (assignedById > 0)
                    task = task.Where(x => x.AssignedById == assignedById);
                if (taskTypeId > 0)
                    task = task.Where(x => x.TaskTypeId == taskTypeId);
                else
                    task = task.Where(x => x.Status == TaskTransactionStatus.Completed && x.CreatedDate > fromDate && x.CreatedDate < toDate);
                tasktransaction = await task.Skip(skipRow).Take(pageSize).OrderByDescending(x => x.TaskId).ToListAsync();
                var response = _mapper.Map<List<TaskTransactionResponse>>(tasktransaction);
                if (response.Count > 0)
                    response[0].TotalRecord = await task.CountAsync();
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<TaskTransactionResponse>> GetCompletedTeamTaskAsyncWithPagination(int ParentUserId, int skipRow1, int pageSize1, int currentPage1, string searchtext1, int projectId, int assignedById, int assignedTo, int taskTypeId, string status, DateTime fromDate, DateTime toDate)
        {
            try
            {
                //var skipRow = (currentPage - 1) * pageSize;
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                var myTeam = await GetAllUsersOfParentUserAsync(ParentUserId);
                var myUserList = myTeam.Where(x => x.Id != ParentUserId);
                List<TaskTransaction> myTask = null;
                if (string.IsNullOrEmpty(searchtext1) == false)
                    myTask = await kUrgeTruckContext.TaskTransaction.Include(x => x.ProjectMaster).Include(x => x.TaskTypeMaster).Include(x => x.UserManager).OrderByDescending(x => x.AssignedDate).Where(x => x.Title.Contains(searchtext1)).ToListAsync();
                else
                    myTask = await kUrgeTruckContext.TaskTransaction.Include(x => x.ProjectMaster).Include(x => x.TaskTypeMaster).Include(x => x.UserManager).OrderByDescending(x => x.AssignedDate).ToListAsync();
                var mytask2 = from task in myTask.ToList()
                              join team in myUserList on task.AssignedTo equals team.Id
                              select task;
                if (projectId > 0)
                    mytask2 = mytask2.Where(x => x.ProjectId == projectId);
                if (assignedById > 0)
                    mytask2 = mytask2.Where(x => x.AssignedById == assignedById);
                if (assignedTo > 0)
                    mytask2 = mytask2.Where(x => x.AssignedTo == assignedTo);
                if (taskTypeId > 0)
                    mytask2 = mytask2.Where(x => x.TaskTypeId == taskTypeId);
                else
                    mytask2 = mytask2.Where(x => x.Status == TaskTransactionStatus.Completed && x.CreatedDate > fromDate && x.CreatedDate < toDate);
                //return mytask2.ToList();
                var mytask3 = mytask2.Skip(skipRow1).Take(pageSize1).OrderByDescending(x => x.TaskId);
                var response = _mapper.Map<List<TaskTransactionResponse>>(mytask3);
                if (response.Count > 0)
                    response[0].TotalRecord = mytask2.Count();
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<TaskTransactionResponse>> getCompletedRaisedTaskAsyncWithPagination(int ParentUserId, int skipRow, int pageSize, string searchtext, int projectId, int assignedToId, int taskTypeId, string status, string raisedBy)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                List<TaskTransaction> tasktransaction = null;
                var task = kUrgeTruckContext.TaskTransaction.Include(x => x.ProjectMaster).Include(x => x.TaskTypeMaster).Include(x => x.UserManager).OrderByDescending(x => x.AssignedDate).Where(x => x.AssignedById == ParentUserId);
                if (string.IsNullOrEmpty(searchtext) == false)
                    task = task.Where(x => x.Title.Contains(searchtext));
                if (projectId > 0)
                    task = task.Where(x => x.ProjectId == projectId);
                if (assignedToId > 0)
                    task = task.Where(x => x.AssignedTo == assignedToId);
                if (taskTypeId > 0)
                    task = task.Where(x => x.TaskTypeId == taskTypeId);
                if (status != null)
                    task = task.Where(x => x.Status == status);
                else
                    task = task.Where(x => x.Status == TaskTransactionStatus.Completed);
                //else
                //    task = task.Where(x => x.Status != TaskTransactionStatus.Completed && x.Status != TaskTransactionStatus.Canceled);
                tasktransaction = await task.Skip(skipRow).Take(pageSize).OrderByDescending(x => x.TaskId).ToListAsync();
                var response = _mapper.Map<List<TaskTransactionResponse>>(tasktransaction);
                if (response.Count > 0)
                    response[0].TotalRecord = await task.CountAsync();
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<TaskTransactionResponseForDownload>> GetAllCompletedTaskToDownloadAsync(int ParentUserId, string searchtext, int projectId, int assignedById, int taskTypeId, string status, DateTime fromDate, DateTime toDate)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                List<TaskTransaction> tasktransaction = null;
                var task = kUrgeTruckContext.TaskTransaction.Include(x => x.ProjectMaster).Include(x => x.TaskTypeMaster).Include(x => x.DepartmentMaster).Include(x => x.UserManager).OrderByDescending(x => x.AssignedDate).Where(x => x.AssignedTo == ParentUserId);
                if (string.IsNullOrEmpty(searchtext) == false)
                    task = task.Where(x => x.Title.Contains(searchtext));
                if (projectId > 0)
                    task = task.Where(x => x.ProjectId == projectId);
                if (assignedById > 0)
                    task = task.Where(x => x.AssignedById == assignedById);
                if (taskTypeId > 0)
                    task = task.Where(x => x.TaskTypeId == taskTypeId);
                else
                    task = task.Where(x => x.Status == TaskTransactionStatus.Completed && x.CreatedDate > fromDate && x.CreatedDate < toDate);
                tasktransaction = await task.OrderByDescending(x => x.TaskId).ToListAsync();
                var response = _mapper.Map<List<TaskTransactionResponseForDownload>>(tasktransaction);
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<TaskTransactionResponseForDownload>> GetCompletedTeamTaskToDownloadAsync(int ParentUserId, string searchtext1, int projectId, int assignedById, int assignedTo, int taskTypeId, string status, DateTime fromDate, DateTime toDate)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                var myTeam = await GetAllUsersOfParentUserAsync(ParentUserId);
                var myUserList = myTeam.Where(x => x.Id != ParentUserId);
                List<TaskTransaction> myTask = null;
                if (string.IsNullOrEmpty(searchtext1) == false)
                    myTask = await kUrgeTruckContext.TaskTransaction.Include(x => x.ProjectMaster).Include(x => x.DepartmentMaster).Include(x => x.TaskTypeMaster).Include(x => x.UserManager).OrderByDescending(x => x.AssignedDate).Where(x => x.Title.Contains(searchtext1)).ToListAsync();
                else
                    myTask = await kUrgeTruckContext.TaskTransaction.Include(x => x.ProjectMaster).Include(x => x.DepartmentMaster).Include(x => x.TaskTypeMaster).Include(x => x.UserManager).OrderByDescending(x => x.AssignedDate).ToListAsync();
                var mytask2 = from task in myTask.ToList()
                              join team in myUserList on task.AssignedTo equals team.Id
                              select task;
                if (projectId > 0)
                    mytask2 = mytask2.Where(x => x.ProjectId == projectId);
                if (assignedById > 0)
                    mytask2 = mytask2.Where(x => x.AssignedById == assignedById);
                if (assignedTo > 0)
                    mytask2 = mytask2.Where(x => x.AssignedTo == assignedTo);
                if (taskTypeId > 0)
                    mytask2 = mytask2.Where(x => x.TaskTypeId == taskTypeId);
                else
                    mytask2 = mytask2.Where(x => x.Status == TaskTransactionStatus.Completed && x.CreatedDate > fromDate && x.CreatedDate < toDate);
                //return mytask2.ToList();
                var mytask3 = mytask2.OrderByDescending(x => x.TaskId);
                var response = _mapper.Map<List<TaskTransactionResponseForDownload>>(mytask3);
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Coordinate Team Task
        public async Task<List<TaskTransactionResponse>> CoordinatingTeamTaskAsyn(int ParentUserId, int skipRow1, int pageSize1, int currentPage1, string searchtext1, int projectId, int assignedById, int assignedTo, int taskTypeId, string status, DateTime fromDate, DateTime toDate)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                List<TaskTransaction> myTask = null;
                List<TaskTransactionResponse> response = null;
                var dept = await kUrgeTruckContext.DepartmentMaster.Where(x => x.coordinatingIncharge == ParentUserId).ToListAsync();
                if (dept.Count == 0)
                    return response;
                if (string.IsNullOrEmpty(searchtext1) == false)
                    myTask = await kUrgeTruckContext.TaskTransaction.Include(x => x.ProjectMaster).Include(x => x.TaskTypeMaster).Include(x => x.UserManager).OrderByDescending(x => x.AssignedDate).Where(x => x.Title.Contains(searchtext1)).ToListAsync();
                else
                    myTask = await kUrgeTruckContext.TaskTransaction.Include(x => x.ProjectMaster).Include(x => x.TaskTypeMaster).Include(x => x.UserManager).OrderByDescending(x => x.AssignedDate).ToListAsync();
                var mytask2 = from task in myTask.ToList()
                              join deptment in dept on task.DepartmentId equals deptment.DepartmentId
                              select task;
                if (projectId > 0)
                    mytask2 = mytask2.Where(x => x.ProjectId == projectId);
                if (assignedById > 0)
                    mytask2 = mytask2.Where(x => x.AssignedById == assignedById);
                if (assignedTo > 0)
                    mytask2 = mytask2.Where(x => x.AssignedTo == assignedTo);
                if (taskTypeId > 0)
                    mytask2 = mytask2.Where(x => x.TaskTypeId == taskTypeId);
                mytask2 = mytask2.Where(x => x.AssignedDate >= fromDate && x.AssignedDate <= toDate);
                //return mytask2.ToList();
                var mytask3 = mytask2.Skip(skipRow1).Take(pageSize1).OrderByDescending(x => x.TaskId);
                response = _mapper.Map<List<TaskTransactionResponse>>(mytask3);
                if (response.Count > 0)
                    response[0].TotalRecord = mytask2.Count();

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region generateTaskNumber
        public async Task<int> GenerateTaskNumber()
        {
            try
            {
                int taskNumber = 1;
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                var task = await kUrgeTruckContext.TaskTransaction.MaxAsync(x => x.taskNumber);
                if (task != null || task > 0)
                    taskNumber = (int)task + 1;
                return taskNumber;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Coordinate Team Task To Download
        public async Task<List<TaskTransactionResponseForDownload>> CoordinatingTeamTaskToDownloadAsyn(int ParentUserId, string searchtext1, int projectId, int assignedById, int assignedTo, int taskTypeId, string status, DateTime fromDate, DateTime toDate)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                List<TaskTransaction> myTask = null;
                List<TaskTransactionResponseForDownload> response = null;
                var dept = await kUrgeTruckContext.DepartmentMaster.Where(x => x.coordinatingIncharge == ParentUserId).ToListAsync();
                if (dept.Count == 0)
                    return response;
                if (string.IsNullOrEmpty(searchtext1) == false)
                    myTask = await kUrgeTruckContext.TaskTransaction.Include(x => x.ProjectMaster).Include(x => x.TaskTypeMaster).Include(x => x.UserManager).OrderByDescending(x => x.AssignedDate).Where(x => x.Title.Contains(searchtext1)).ToListAsync();
                else
                    myTask = await kUrgeTruckContext.TaskTransaction.Include(x => x.ProjectMaster).Include(x => x.TaskTypeMaster).Include(x => x.UserManager).OrderByDescending(x => x.AssignedDate).ToListAsync();
                var mytask2 = from task in myTask.ToList()
                              join deptment in dept on task.DepartmentId equals deptment.DepartmentId
                              select task;
                if (projectId > 0)
                    mytask2 = mytask2.Where(x => x.ProjectId == projectId);
                if (assignedById > 0)
                    mytask2 = mytask2.Where(x => x.AssignedById == assignedById);
                if (assignedTo > 0)
                    mytask2 = mytask2.Where(x => x.AssignedTo == assignedTo);
                if (taskTypeId > 0)
                    mytask2 = mytask2.Where(x => x.TaskTypeId == taskTypeId);
                mytask2 = mytask2.Where(x => x.AssignedDate >= fromDate && x.AssignedDate <= toDate);
                //return mytask2.ToList();
                var mytask3 = mytask2.OrderByDescending(x => x.TaskId);
                response = _mapper.Map<List<TaskTransactionResponseForDownload>>(mytask3);

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region ReopenTask
        public async Task<ResultModel> ReopenTask(RepoenedRequest repoenedRequest, int raisedById)
        {
            try
            {
                var resMessage = "Task ";
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();

                var task = await kUrgeTruckContext.TaskTransaction.FirstOrDefaultAsync(x => x.TaskId == repoenedRequest.TaskId);
                if (task != null)
                {
                    task.Status = TaskTransactionStatus.Reopen;
                    task.Remarks = task.Remarks + " " + repoenedRequest.Remarks;
                    task.ModifiedBy = repoenedRequest.ReopedBy;
                    task.ModifiedDate = DateTime.Now;
                    task.ActualEndDate = null;

                    kUrgeTruckContext.TaskTransaction.Update(task);
                    await kUrgeTruckContext.SaveChangesAsync();

                    var autogeneratedTask = await kUrgeTruckContext.TaskTransaction.Where(x => x.taskNumber == task.taskNumber).ToListAsync();
                    if (autogeneratedTask.Count > 1)
                    {
                        var unassign = autogeneratedTask.Where(x => x.TaskId != task.TaskId && x.TaskId > task.TaskId).ToList();
                        foreach (var item in unassign)
                        {
                            item.Status = TaskTransactionStatus.Canceled;
                            item.ModifiedBy = repoenedRequest.ReopedBy;
                            item.ModifiedDate = DateTime.Now;
                            kUrgeTruckContext.TaskTransaction.Update(item);
                        }
                        await kUrgeTruckContext.SaveChangesAsync();
                    }

                    if (raisedById != task.AssignedTo)
                    {
                        NotificationRequest notification = new NotificationRequest();
                        notification.Title = NotificationTitle.Reopened;
                        notification.Message = task.Title + NotificationTitle.Reopened + " by " + repoenedRequest.ReopedBy;
                        notification.UserId = (int)task.AssignedTo;
                        notification.CreatedBy = repoenedRequest.ReopedBy;
                        notification.CreatedDate = DateTime.Now;
                        await _notification.AddNewNotification(notification);
                    }
                    if (raisedById != task.AssignedById)
                    {
                        NotificationRequest notification1 = new NotificationRequest();
                        notification1.Title = NotificationTitle.Reopened;
                        notification1.Message = task.Title + NotificationTitle.Reopened + " by " + repoenedRequest.ReopedBy;
                        notification1.UserId = (int)task.AssignedById;
                        notification1.CreatedBy = repoenedRequest.ReopedBy;
                        notification1.CreatedDate = DateTime.Now;
                        await _notification.AddNewNotification(notification1);
                    }
                }

                resMessage = resMessage + UrgeTruckMessages.Task_reopen_successfully;
                return ResultModelFactory.UpdateSucess(resMessage);
            }
            catch (Exception ex)
            {
                Logger.Error("Error while Reopening Task " + ex);
                return ResultModelFactory.CreateFailure(ResultCode.ExceptionThrown, UrgeTruckMessages.Error_while_reopen_Task, ex);
            }
        }
        #endregion
    }
}
