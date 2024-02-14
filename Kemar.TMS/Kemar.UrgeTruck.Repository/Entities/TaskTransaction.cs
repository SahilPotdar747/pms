using Kemar.UrgeTruck.Domain.Common;
using Kemar.UrgeTruck.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.TMS.Repository.Entities
{
    public class TaskTransaction : CommonEntityModel
    {
        public TaskTransaction()
        {
            DelegateHistory = new List<DelegateHistory>();
        }
        public int TaskId { get; set; }
        public int? taskNumber { get; set; }
        public string Title { get; set; }
        public int ProjectId { get; set; }
        public int TaskTypeId { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public int? AssignedTo { get; set; }
        public int AssignedById { get; set; }
        public string AssignedBy { get; set; }
        public DateTime? AssignedDate { get; set; }
        public DateTime? ExceptedStartDate { get; set; }
        public DateTime? ExceptedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public string Remarks { get; set; }
        public int? DepartmentId { get; set; }
        public virtual UserManager UserManager { get; set; }
        public virtual TaskTypeMaster TaskTypeMaster { get; set; }
        public virtual ProjectMaster ProjectMaster { get; set; }
        public virtual DepartmentMaster DepartmentMaster { get; set; }
        public virtual ICollection<DelegateHistory> DelegateHistory { get; set; }
    }
}
