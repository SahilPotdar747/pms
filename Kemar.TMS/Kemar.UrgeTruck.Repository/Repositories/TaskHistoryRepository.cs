using AutoMapper;
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
    public class TaskHistoryRepository : ITaskHistory
    {
        private readonly IKUrgeTruckContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly ITaskHistory _taskHistory;
        public TaskHistoryRepository(IKUrgeTruckContextFactory contextFactory,
            IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<ResultModel> AddTaskHistory(TaskTransaction task)
        {
            var resMessage = "Task HIstory ";
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                TaskHistory history = new TaskHistory();
                history.TaskId = task.TaskId;
                history.TaskType = task.TaskTypeMaster?.TaskName;
                history.Description = task.Description;
                history.Priority = task.Priority;
                history.AssignedTo = task.UserManager?.UserName;
                history.ExceptedStartDate = task.ExceptedStartDate;
                history.ExceptedEndDate = task.ExceptedEndDate;
                history.Status = task.Status;
                history.IsActive = task.IsActive;

                kUrgeTruckContext.Add(history);
                await kUrgeTruckContext.SaveChangesAsync();

                return ResultModelFactory.UpdateSucess(resMessage);
                

            }
            catch (Exception ex)
            {
                Logger.Error("Error while register Task History " + ex);
                return ResultModelFactory.CreateFailure(ResultCode.ExceptionThrown, UrgeTruckMessages.Error_while_addupdate_Task, ex);
            }
        }

        public async Task<List<TaskHistory>> GetTaskHistory(int taskId)
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var taskHistory = await kUrgeTruckContext.TaskHistory.Where(x => x.TaskId == taskId).OrderByDescending(x=>x.TaskId).ToListAsync();
            return taskHistory;
        }
    }
}
