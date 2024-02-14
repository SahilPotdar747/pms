using Kemar.UrgeTruck.Domain.Common;
using Kemar.UrgeTruck.Repository.Entities;
using System.Collections.Generic;

namespace Kemar.TMS.Repository.Entities
{
    public class DepartmentMaster : CommonEntityModel
    {
        public DepartmentMaster()
        {
            UserManager = new List<UserManager>();
            TaskTransaction = new List<TaskTransaction>();
            TaskTypeMaster = new List<TaskTypeMaster>();
        }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public bool IsActive { get; set; }
        public int? coordinatingIncharge { get; set; }
        public string coordinatingInchargeName { get; set; }
        public virtual ICollection<UserManager> UserManager { get; set; }
        public virtual ICollection<TaskTransaction> TaskTransaction { get; set; }
        public virtual ICollection<TaskTypeMaster> TaskTypeMaster { get; set; }
    }
}
