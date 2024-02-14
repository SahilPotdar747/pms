using Kemar.TMS.Repository.Entities;
using Kemar.UrgeTruck.Domain.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.TMS.Repository.Interface
{
    public interface ITaskHistory
    {
        Task<ResultModel> AddTaskHistory(TaskTransaction task);
        Task<List<TaskHistory>> GetTaskHistory(int taskId);
    }
}
