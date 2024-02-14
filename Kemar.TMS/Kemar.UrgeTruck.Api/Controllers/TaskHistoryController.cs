using Kemar.TMS.Repository.Entities;
using Kemar.TMS.Repository.Interface;
using Kemar.UrgeTruck.Api.Core.Helper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kemar.TMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskHistoryController : ControllerBase
    {
        private readonly ITaskHistory _taskHistory;

        public TaskHistoryController(ITaskHistory taskHistory)
        {
            _taskHistory = taskHistory;
        }

        [HttpGet]
        [Route("GetTaskHistory")]
        public async Task<List<TaskHistory>> GetTaskHistory([FromQuery] int taskId)
        {
            return await _taskHistory.GetTaskHistory(taskId);
        }
    }
}
