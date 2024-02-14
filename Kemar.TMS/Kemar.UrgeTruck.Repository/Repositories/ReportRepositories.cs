using AutoMapper;
using Kemar.TMS.Domain.DTOs;
using Kemar.TMS.Domain.ResponseModel;
using Kemar.TMS.Repository.Entities;
using Kemar.TMS.Repository.Interface;
using Kemar.UrgeTruck.Domain.Common;
using Kemar.UrgeTruck.Repository.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.TMS.Repository.Repositories
{
    public class ReportRepositories : IReport
    {
        private readonly IKUrgeTruckContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly ITaskTransaction _taskTransaction;

        public ReportRepositories(IKUrgeTruckContextFactory contextFactory, IMapper mapper, ITaskTransaction taskTransaction)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _taskTransaction = taskTransaction;
        }

        #region GetProjectWiseReport
        public async Task<List<ProjectWiseTaskCountReportResponse>> GetProjectWiseReport(DateTime fromdate, DateTime todate, int skipRow, int pageSize)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@FromDate", fromdate));
                parameters.Add(new SqlParameter("@ToDate", todate));
                var projectWiseTaskCountReport =await kUrgeTruckContext.ProjectWiseTaskCountReport.FromSqlRaw("ProjectTaskCountData @FromDate, @ToDate", parameters.ToArray()).ToListAsync();
                var countReport = projectWiseTaskCountReport.Skip(skipRow).Take(pageSize).OrderBy(x=>x.ProjectName);
                var response = _mapper.Map<List<ProjectWiseTaskCountReportResponse>>(countReport);
                if (response.Count > 0)
                    response[0].TotalRecord = projectWiseTaskCountReport.Count(); // temporary used Completed instead of TotalRecord
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region GetUserWiseReport
        public async Task<List<UserWiseTaskDataCountReportResponse>> GetUserWiseReport(int userId, DateTime fromdate, DateTime todate, int skipRow, int pageSize)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@userId", userId));
                parameters.Add(new SqlParameter("@FromDate", fromdate));
                parameters.Add(new SqlParameter("@ToDate", todate));
                var userWiseTaskDataCountReport = await kUrgeTruckContext.UserWiseTaskDataCountReport.FromSqlRaw("UserWiseTaskDataCount @userId, @FromDate, @ToDate", parameters.ToArray()).ToListAsync();
                var countReport = userWiseTaskDataCountReport.Skip(skipRow).Take(pageSize).OrderBy(x=>x.FirstName);
                var response = _mapper.Map<List<UserWiseTaskDataCountReportResponse>>(countReport);
                if (response.Count > 0)
                    response[0].TotalRecord = userWiseTaskDataCountReport.Count(); // temporary used Completed instead of TotalRecord
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region GetUserWiseTask
        public async Task<List<TaskTransactionResponse>> GetUserWiseTask(int userId, DateTime fromdate, DateTime todate, int skipRow, int pageSize, string searchtext, int projectId, int assignedById, int taskTypeId, int assignedTo)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                var myTeam = await _taskTransaction.GetAllUsersOfParentUserAsync(userId);
                List<TaskTransaction> myTask = null;
                if (string.IsNullOrEmpty(searchtext) == false)
                    //&& x.Status == TaskTransactionStatus.Completed
                    myTask = await kUrgeTruckContext.TaskTransaction.Include(x => x.ProjectMaster).Include(x => x.TaskTypeMaster).Include(x => x.UserManager).Include(x => x.DepartmentMaster).Where(x => x.Title.Contains(searchtext)  && x.AssignedDate >= fromdate && x.AssignedDate <= todate).ToListAsync();
                else
                    myTask = await kUrgeTruckContext.TaskTransaction.Include(x => x.ProjectMaster).Include(x => x.TaskTypeMaster).Include(x => x.UserManager).Include(x => x.DepartmentMaster).Where(x => x.AssignedDate >= fromdate && x.AssignedDate <= todate).ToListAsync();
                var mytask2 = from task in myTask.ToList()
                              join team in myTeam on task.AssignedTo equals team.Id
                              select task;
                if (projectId > 0)
                    mytask2 = mytask2.Where(x => x.ProjectId == projectId);
                if (assignedById > 0)
                    mytask2 = mytask2.Where(x => x.AssignedById == assignedById);
                if (assignedTo > 0)
                    mytask2 = mytask2.Where(x => x.AssignedTo == assignedTo);
                if (taskTypeId > 0)
                    mytask2 = mytask2.Where(x => x.TaskTypeId == taskTypeId);
                //return mytask2.ToList();
                var mytask3 = mytask2.Skip(skipRow).Take(pageSize).OrderByDescending(x => x.TaskId);
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
        #endregion

        #region GetProjectWiseTask
        public async Task<List<TaskTransactionResponse>> GetProjectWiseTask(DateTime fromdate, DateTime todate, int skipRow, int pageSize, string searchtext, int projectId, int assignedById, int taskTypeId, int assignedTo)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                List<TaskTransaction> myTask = null;
                //x.Status == TaskTransactionStatus.Completed &&
                 var Task =  kUrgeTruckContext.TaskTransaction.Include(x => x.ProjectMaster).Include(x => x.TaskTypeMaster).Include(x => x.UserManager).Include(x => x.DepartmentMaster).Where(x => x.AssignedDate >= fromdate && x.AssignedDate <= todate);
                if (string.IsNullOrEmpty(searchtext) == false)
                    Task = Task.Where(x => x.Title.Contains(searchtext));
                if (projectId > 0)
                    Task = Task.Where(x => x.ProjectId == projectId);
                if (assignedById > 0)
                    Task = Task.Where(x => x.AssignedById == assignedById);
                if (assignedTo > 0)
                    Task = Task.Where(x => x.AssignedTo == assignedTo);
                if (taskTypeId > 0)
                    Task = Task.Where(x => x.TaskTypeId == taskTypeId);
                //return mytask2.ToList();
                var mytask3 = await Task.Skip(skipRow).Take(pageSize).OrderByDescending(x => x.AssignedDate).ToListAsync();
                var response = _mapper.Map<List<TaskTransactionResponse>>(mytask3);
                if (response.Count > 0)
                    response[0].TotalRecord = await Task.CountAsync(); ;
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region GetAllUserWiseTask
        public async Task<List<TaskTransactionResponse>> GetAllUserWiseTask(DateTime fromdate, DateTime todate, int skipRow, int pageSize, string searchtext, int projectId, int assignedById, int taskTypeId, int assignedTo)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                var myTeam = await kUrgeTruckContext.UserManager.ToListAsync();
                List<TaskTransaction> myTask = null;
                if (string.IsNullOrEmpty(searchtext) == false)
                    //&& x.Status == TaskTransactionStatus.Completed
                    myTask = await kUrgeTruckContext.TaskTransaction.Include(x => x.ProjectMaster).Include(x => x.TaskTypeMaster).Include(x => x.UserManager).Include(x => x.DepartmentMaster).Where(x => x.Title.Contains(searchtext) && x.AssignedDate >= fromdate && x.AssignedDate <= todate).ToListAsync();
                else
                    myTask = await kUrgeTruckContext.TaskTransaction.Include(x => x.ProjectMaster).Include(x => x.TaskTypeMaster).Include(x => x.UserManager).Include(x => x.DepartmentMaster).Where(x => x.AssignedDate >= fromdate && x.AssignedDate <= todate).ToListAsync();
                var mytask2 = from task in myTask.ToList()
                              join team in myTeam on task.AssignedTo equals team.Id
                              select task;
                if (projectId > 0)
                    mytask2 = mytask2.Where(x => x.ProjectId == projectId);
                if (assignedById > 0)
                    mytask2 = mytask2.Where(x => x.AssignedById == assignedById);
                if (assignedTo > 0)
                    mytask2 = mytask2.Where(x => x.AssignedTo == assignedTo);
                if (taskTypeId > 0)
                    mytask2 = mytask2.Where(x => x.TaskTypeId == taskTypeId);
                //return mytask2.ToList();
                var mytask3 = mytask2.Skip(skipRow).Take(pageSize).OrderByDescending(x => x.TaskId);
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

        #region GetAllUserWiseReport
        public async Task<List<UserWiseTaskDataCountReportResponse>> GetAllUserWiseReport(DateTime fromdate, DateTime todate, int skipRow, int pageSize)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@FromDate", fromdate));
                parameters.Add(new SqlParameter("@ToDate", todate));
                var userWiseTaskDataCountReport = await kUrgeTruckContext.UserWiseTaskDataCountReport.FromSqlRaw("AllUserWiseTaskDataCount @FromDate, @ToDate", parameters.ToArray()).ToListAsync();
                var countReport = userWiseTaskDataCountReport.Skip(skipRow).Take(pageSize).OrderBy(x => x.FirstName);
                var response = _mapper.Map<List<UserWiseTaskDataCountReportResponse>>(countReport);
                if (response.Count > 0)
                    response[0].TotalRecord = userWiseTaskDataCountReport.Count(); // temporary used Completed instead of TotalRecord
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region GetProjectWiseTaskCountReportToDownload
        public async Task<List<ProjectWiseTaskCountReportResponseForDownload>> GetProjectWiseTaskCountReportTODownload(DateTime fromdate, DateTime todate)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@FromDate", fromdate));
                parameters.Add(new SqlParameter("@ToDate", todate));
                var projectWiseTaskCountReport = await kUrgeTruckContext.ProjectWiseTaskCountReport.FromSqlRaw("ProjectTaskCountData @FromDate, @ToDate", parameters.ToArray()).ToListAsync();
                var countReport = projectWiseTaskCountReport.OrderBy(x => x.ProjectName);
                var response = _mapper.Map<List<ProjectWiseTaskCountReportResponseForDownload>>(countReport);
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region GetUserWiseTaskDataCountReportToDownload
        public async Task<List<UserWiseTaskDataCountReportResponseToDownload>> GetUserWiseTaskDataCountReportToDownload(int userId, DateTime fromdate, DateTime todate)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@userId", userId));
                parameters.Add(new SqlParameter("@FromDate", fromdate));
                parameters.Add(new SqlParameter("@ToDate", todate));
                var userWiseTaskDataCountReport = await kUrgeTruckContext.UserWiseTaskDataCountReport.FromSqlRaw("UserWiseTaskDataCount @userId, @FromDate, @ToDate", parameters.ToArray()).ToListAsync();
                var countReport = userWiseTaskDataCountReport.OrderBy(x => x.FirstName);
                var response = _mapper.Map<List<UserWiseTaskDataCountReportResponseToDownload>>(countReport);
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region GetAllUserWiseReportToDownload
        public async Task<List<TaskTransactionResponseForDownload>> GetAllUserWiseReportToDownload(DateTime fromdate, DateTime todate, string searchtext, int projectId, int assignedById, int taskTypeId, int assignedTo)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                var myTeam = await kUrgeTruckContext.UserManager.ToListAsync();
                List<TaskTransaction> myTask = null;
                if (string.IsNullOrEmpty(searchtext) == false)
                    //&& x.Status == TaskTransactionStatus.Completed
                    myTask = await kUrgeTruckContext.TaskTransaction.Include(x => x.ProjectMaster).Include(x => x.TaskTypeMaster).Include(x => x.UserManager).Include(x => x.DepartmentMaster).Where(x => x.Title.Contains(searchtext) && x.AssignedDate >= fromdate && x.AssignedDate <= todate).ToListAsync();
                else
                    myTask = await kUrgeTruckContext.TaskTransaction.Include(x => x.ProjectMaster).Include(x => x.TaskTypeMaster).Include(x => x.UserManager).Include(x => x.DepartmentMaster).Where(x => x.AssignedDate >= fromdate && x.AssignedDate <= todate).ToListAsync();
                var mytask2 = from task in myTask.ToList()
                              join team in myTeam on task.AssignedTo equals team.Id
                              select task;
                if (projectId > 0)
                    mytask2 = mytask2.Where(x => x.ProjectId == projectId);
                if (assignedById > 0)
                    mytask2 = mytask2.Where(x => x.AssignedById == assignedById);
                if (assignedTo > 0)
                    mytask2 = mytask2.Where(x => x.AssignedTo == assignedTo);
                if (taskTypeId > 0)
                    mytask2 = mytask2.Where(x => x.TaskTypeId == taskTypeId);
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

        #region GetUserWiseTaskToDownload
        public async Task<List<TaskTransactionResponseForDownload>> GetUserWiseTaskToDownload(int userId, DateTime fromdate, DateTime todate, string searchtext, int projectId, int assignedById, int taskTypeId, int assignedTo)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                var myTeam = await _taskTransaction.GetAllUsersOfParentUserAsync(userId);
                List<TaskTransaction> myTask = null;
                if (string.IsNullOrEmpty(searchtext) == false)
                    //&& x.Status == TaskTransactionStatus.Completed
                    myTask = await kUrgeTruckContext.TaskTransaction.Include(x => x.DepartmentMaster).Include(x => x.ProjectMaster).Include(x => x.TaskTypeMaster).Include(x => x.UserManager).Where(x => x.Title.Contains(searchtext) && x.AssignedDate >= fromdate && x.AssignedDate <= todate).ToListAsync();
                else
                    myTask = await kUrgeTruckContext.TaskTransaction.Include(x => x.DepartmentMaster).Include(x => x.ProjectMaster).Include(x => x.TaskTypeMaster).Include(x => x.UserManager).Where(x => x.AssignedDate >= fromdate && x.AssignedDate <= todate).ToListAsync();
                var mytask2 = from task in myTask.ToList()
                              join team in myTeam on task.AssignedTo equals team.Id
                              select task;
                if (projectId > 0)
                    mytask2 = mytask2.Where(x => x.ProjectId == projectId);
                if (assignedById > 0)
                    mytask2 = mytask2.Where(x => x.AssignedById == assignedById);
                if (assignedTo > 0)
                    mytask2 = mytask2.Where(x => x.AssignedTo == assignedTo);
                if (taskTypeId > 0)
                    mytask2 = mytask2.Where(x => x.TaskTypeId == taskTypeId);
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

        #region GetProjectWiseTaskToDownload
        public async Task<List<TaskTransactionResponseForDownload>> GetProjectWiseTaskToDownload(DateTime fromdate, DateTime todate, string searchtext, int projectId, int assignedById, int taskTypeId, int assignedTo)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                List<TaskTransaction> myTask = null;
                //x.Status == TaskTransactionStatus.Completed &&
                var Task = kUrgeTruckContext.TaskTransaction.Include(x => x.ProjectMaster).Include(x => x.DepartmentMaster).Include(x => x.TaskTypeMaster).Include(x => x.UserManager).Where(x => x.AssignedDate >= fromdate && x.AssignedDate <= todate);
                if (string.IsNullOrEmpty(searchtext) == false)
                    Task = Task.Where(x => x.Title.Contains(searchtext));
                if (projectId > 0)
                    Task = Task.Where(x => x.ProjectId == projectId);
                if (assignedById > 0)
                    Task = Task.Where(x => x.AssignedById == assignedById);
                if (assignedTo > 0)
                    Task = Task.Where(x => x.AssignedTo == assignedTo);
                if (taskTypeId > 0)
                    Task = Task.Where(x => x.TaskTypeId == taskTypeId);
                //return mytask2.ToList();
                var mytask3 = await Task.OrderByDescending(x => x.TaskId).ToListAsync();
                var response = _mapper.Map<List<TaskTransactionResponseForDownload>>(mytask3);
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region GetAllUserWiseReportToDownload
        public async Task<List<UserWiseTaskDataCountReportResponseToDownload>> GetAllUserWiseCountReportToDownload(DateTime fromdate, DateTime todate)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@FromDate", fromdate));
                parameters.Add(new SqlParameter("@ToDate", todate));
                var userWiseTaskDataCountReport = await kUrgeTruckContext.UserWiseTaskDataCountReport.FromSqlRaw("AllUserWiseTaskDataCount @FromDate, @ToDate", parameters.ToArray()).ToListAsync();
                var countReport = userWiseTaskDataCountReport.OrderBy(x => x.FirstName);
                var response = _mapper.Map<List<UserWiseTaskDataCountReportResponseToDownload>>(countReport);
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}
