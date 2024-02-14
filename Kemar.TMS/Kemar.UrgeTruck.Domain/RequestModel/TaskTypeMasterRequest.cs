using Kemar.UrgeTruck.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.TMS.Domain.RequestModel
{
    public class TaskTypeMasterRequest:CommonEntityModel
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public bool IsActive { get; set; }
        public string NextTaskname { get; set; }
        public int? NextTaskId { get; set; }
        public int? DepartmentId { get; set; }
    }
}
