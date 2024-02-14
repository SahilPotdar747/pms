using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.TMS.Repository.Entities
{
    public class TaskHistory
    {
        public int TaskHistoryId { get; set; }
        public int TaskId { get; set; }
        public string TaskType { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public string AssignedTo { get; set; }
        public DateTime? ExceptedStartDate { get; set; }
        public DateTime? ExceptedEndDate { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }

    }
}
