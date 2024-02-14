using System.Collections.Generic;

namespace Kemar.UrgeTruck.Domain.Common
{
    public static class ResponseStatus
    {
        public const string Success = "Success";
        public const string Failed = "Failed";
        public const string Cancelled = "Cancelled";
        public const string Invalid = "Invalid";
        public const string PartialSuccess = "Succeeded Partially";
    }

    public static class RoleConstant
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
    }

    public static class RoleGroupConstant
    {
        public const string Admin= "Admin";
        public const string Operation = "Operation";
        public const string ControlTower = "ControlTower";
        public const string Security = "Security";
        public const string Management = "Management";
    }

    public static class AccessRightsConst
    {
        public const string Read = "R";
        public const string Create = "C";
        public const string Update = "U";
        public const string Deactivate = "D";
    }

    public static class ScreenCodeConst
    {
        public const string UserManagement = "USERMGMT";
        public const string Rolemanagement = "ROLEMGMT";
        public const string UserRoleMapping = "URMAP";
    }

    public static class TaskTransactionStatus
    {
        public const string All = "";
        public const string NewTask = "New Task";
        public const string WIP = "WIP";
        public const string WIPShort = "Work In Progress";
        public const string Completed = "Completed";
        public const string Closed = "Closed";
        public const string Canceled = "Canceled";
        public const string OnHold = "On Hold";
        public const string Invalid = "Invalid";
        public const string Delegated = "Delegated";
        public const string Pending = "Pending";
        public const string Overdue = "Overdue";
        public const string UnAssigned = "UnAssigned";
        public const string Reopen = "Reopen";
        public const string Ongoing = "Ongoing";
    }

    public static class DelegateStatus
    {
        public const string Requested = "Requested";
        public const string Rejected = "Rejected";
        public const string Accepted = "Accepted";
        public const string Reassigned = "Reassigned";
    }

    public static class NotificationTitle
    {
        public const string NewTask = "New Task";
        public const string Delegate = "Task Reassign";
        public const string TaskCompleted = "Task Completed";
        public const string TaskNearToDeadline = "Task To Near deadline";
        public const string DelegatedTaskReject = "Delegated Task Rejected";
        public const string DelegatedTaskAccepted = "Delegated Task Accepted";
        public const string DelegatedTaskTransferReuqest = "New Task Transfer Requested";
        public const string DelegatedTaskReassign= "New Task Reassign";
        public const string UpcommingTask = "Uncomming Task";
        public const string Reopened = "Task Reopened";
    }
}

