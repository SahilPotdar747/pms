using AutoMapper;
using Kemar.TMS.Domain.RequestModel;
using Kemar.TMS.Domain.ResponseModel;
using Kemar.TMS.Repository.Entities;
using Kemar.TMS.Repository.Interface;
using Kemar.UrgeTruck.Domain.Common;
using Kemar.UrgeTruck.Domain.ResponseModel;
using Kemar.UrgeTruck.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.TMS.Repository.Repositories
{
    public class TaskTypeRegistrationRepository : ITaskTypeRegistration
    {
        private readonly IKUrgeTruckContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public TaskTypeRegistrationRepository(IKUrgeTruckContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }
        public async Task<List<TaskTypeMasterResponse>> GetAllTaskTypeAsync()
        {
            //throw new NotImplementedException();
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var task = await kUrgeTruckContext.TaskTypeMaster.Include(x => x.DepartmentMaster).OrderBy(x => x.TaskId).ToListAsync();
            return _mapper.Map<List<TaskTypeMasterResponse>>(task);
        }

        public async Task<List<TaskTypeMasterResponse>> GetAllActiveTaskTypeAsync()
        {
            //throw new NotImplementedException();
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var task = await kUrgeTruckContext.TaskTypeMaster.Include(x => x.DepartmentMaster).Where(x => x.IsActive).OrderBy(x => x.TaskId).ToListAsync();
            return _mapper.Map<List<TaskTypeMasterResponse>>(task);
        }

        public async Task<List<TaskTypeMasterResponse>> GetTaskTypeAsyncWithPagination(int rowSize, int skipRow, int currentPage, string searchtext, int departmentId)
        
        {
            try
            {
                //var skip = (currentPage - 1) * rowSize;
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                List<TaskTypeMaster> taskType = null;
                var commonData = kUrgeTruckContext.TaskTypeMaster;
                if (string.IsNullOrEmpty(searchtext) == false && departmentId == 0)
                    taskType = await commonData.Include(x => x.DepartmentMaster).Where(x => x.TaskName.Contains(searchtext)).OrderBy(x=>x.TaskName).Skip(skipRow).Take(rowSize).ToListAsync();
                if (departmentId> 0 && string.IsNullOrEmpty(searchtext))
                    taskType = await commonData.Include(x => x.DepartmentMaster).Where(x => x.DepartmentMaster.DepartmentId == departmentId).OrderBy(x => x.TaskName).Skip(skipRow).Take(rowSize).ToListAsync();
                if (departmentId > 0 && !string.IsNullOrEmpty(searchtext))
                    taskType = await commonData.Include(x => x.DepartmentMaster).Where(x => x.DepartmentMaster.DepartmentId == departmentId && x.TaskName.Contains(searchtext)).OrderBy(x => x.TaskName).Skip(skipRow).Take(rowSize).ToListAsync();
                if (string.IsNullOrEmpty(searchtext) && departmentId == 0)
                    taskType = await commonData.Include(x => x.DepartmentMaster).OrderBy(x => x.TaskName).Skip(skipRow).Take(rowSize).ToListAsync();
                //roles = await kUrgeTruckContext.RoleMaster.Where(x => x.IsActive == true).ToListAsync();
                var response = _mapper.Map<List<TaskTypeMasterResponse>>(taskType);
                if (response.Count > 0 && !string.IsNullOrEmpty(searchtext) && departmentId == 0)
                    response[0].TotalRecord = await commonData.Include(x => x.DepartmentMaster).Where(x => x.TaskName.Contains(searchtext)).CountAsync();
                else if(response.Count >0 && departmentId > 0 && string.IsNullOrEmpty(searchtext))
                    response[0].TotalRecord = await commonData.Include(x => x.DepartmentMaster).Where(x => x.DepartmentMaster.DepartmentId == departmentId).CountAsync();
                else if (response.Count > 0 && !string.IsNullOrEmpty(searchtext) && departmentId > 0)
                    response[0].TotalRecord = await commonData.Include(x => x.DepartmentMaster).Where(x => x.TaskName.Contains(searchtext)&& x.DepartmentMaster.DepartmentId == departmentId).CountAsync();
                else if (response.Count > 0)
                    response[0].TotalRecord = await commonData.CountAsync();
                return response;
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }

        public async Task<ResultModel> RegisterTaskTypeAsync(TaskTypeMasterRequest taskTypeRequest)
        {
            var resMessage = "Action Type ";
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                var departmentExists = kUrgeTruckContext.TaskTypeMaster
                    .Any(x => x.TaskName == taskTypeRequest.TaskName && x.TaskId != taskTypeRequest.TaskId && x.DepartmentId == taskTypeRequest.DepartmentId);
                if (departmentExists == true)
                {
                    return ResultModelFactory.CreateFailure(ResultCode.DuplicateRecord, UrgeTruckMessages.taskType_Exist);
                }
                var task = await kUrgeTruckContext.TaskTypeMaster
                    .FirstOrDefaultAsync(x => x.TaskId == taskTypeRequest.TaskId || (x.TaskName == taskTypeRequest.TaskName && x.DepartmentId == taskTypeRequest.DepartmentId));
                if (task == null)
                {
                    var tasktype = _mapper.Map<TaskTypeMaster>(taskTypeRequest);
                    kUrgeTruckContext.Add(tasktype);
                    resMessage = resMessage + UrgeTruckMessages.added_successfully;
                    await kUrgeTruckContext.SaveChangesAsync();
                    taskTypeRequest.TaskId = tasktype.TaskId;
                    return ResultModelFactory.CreateSucess(resMessage);
                }
                else
                {
                    task.TaskName = taskTypeRequest.TaskName;
                    task.IsActive = taskTypeRequest.IsActive;
                    task.NextTaskId = taskTypeRequest.NextTaskId;
                    task.NextTaskname = taskTypeRequest.NextTaskname;
                    task.DepartmentId = taskTypeRequest.DepartmentId;
                    task.ModifiedBy = taskTypeRequest.ModifiedBy;
                    task.ModifiedDate = taskTypeRequest.ModifiedDate;
                    resMessage = resMessage + UrgeTruckMessages.updated_successfully;
                    kUrgeTruckContext.TaskTypeMaster.Update(task);
                    //taskTypeRequest.TaskId = task.TaskId;
                    await kUrgeTruckContext.SaveChangesAsync();
                    return ResultModelFactory.UpdateSucess(resMessage);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while register department " + ex);
                return ResultModelFactory.CreateFailure(ResultCode.ExceptionThrown, UrgeTruckMessages.Error_while_addupdate_taskType, ex);
            }
        }

        public async Task<List<TaskTypeMasterResponse>> GetAllTaskTypeDepartmentWiseAsync(int departmentId)
        {
            //throw new NotImplementedException();
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var task = await kUrgeTruckContext.TaskTypeMaster.OrderBy(x => x.TaskName).Where(x =>( x.DepartmentId == departmentId || x.DepartmentId == 0 || x.DepartmentId == null ) && x.IsActive).ToListAsync();
            return _mapper.Map<List<TaskTypeMasterResponse>>(task);
        }
    }
}