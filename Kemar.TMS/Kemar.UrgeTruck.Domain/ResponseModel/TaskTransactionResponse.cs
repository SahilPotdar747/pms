using System;
using System.Collections;
using System.Collections.Generic;

namespace Kemar.TMS.Domain.ResponseModel
{
    public class TaskTransactionResponse
    {
        public TaskTransactionResponse()
        {
            delegateHistory = new List<DelegateHistoryResponse>();
        }

        public int TaskId { get; set; }
        public string Title { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int TaskTypeId { get; set; }
        public string Description { get; set;}
        public int Priority { get; set; }
        public int AssignedTo { get; set; }
        public int AssignedById { get; set; }
        public string AssignedBy { get; set; }
        public DateTime? AssignedDate { get; set; }
        public DateTime? ExceptedStartDate { get; set; }
        public DateTime? ExceptedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public string Status { get; set; }
        public string remarks { get; set; }
        public bool IsActive { get; set; }
        public int DepartmentId { get; set; }
        public int TotalRecord { get; set; }
        public virtual UserResponse UserManager { get; set; }
        public virtual TaskTypeMasterResponse TaskTypeMaster { get; set; }
        public virtual ProjectMasterResponse ProjectMaster { get; set; }
        public virtual DepartmentMasterResponse DepartmentMaster { get; set; }

        public ICollection<DelegateHistoryResponse> delegateHistory { get; set; }
    }

    public class TaskTransactionResponseForDownload
    {
        public string Title { get; set; }
        public string ProjectName { get; set; }
        public string TaskTypeName { get; set; }
        public string Department { get; set; }
        public string Description { get; set; }
        public string AssignedTo{ get; set; }
        public string AssignedBy { get; set; }
        public DateTime? AssignedDate { get; set; }
        public DateTime? ExceptedStartDate { get; set; }
        public DateTime? ExceptedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public string Status { get; set; }
    }
}
