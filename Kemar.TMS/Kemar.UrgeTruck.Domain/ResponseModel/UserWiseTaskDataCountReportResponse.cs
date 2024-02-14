using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.TMS.Domain.ResponseModel
{
    public class UserWiseTaskDataCountReportResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DepartmentName { get; set; }
        public int WIP { get; set; }
        public int UnAssigned { get; set; }
        public int Pending { get; set; }
        public int Overdue { get; set; }
        public int Closed { get; set; }
        public int Invalid { get; set; }
        public int Delegated { get; set; }
        public int Completed { get; set; }
        public int Reopen { get; set; }
        public int NewTask { get; set; }
        public int TotalRecord {get;set;}
    }

    public class UserWiseTaskDataCountReportResponseToDownload
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DepartmentName { get; set; }
        public int WIP { get; set; }
        public int UnAssigned { get; set; }
        public int Pending { get; set; }
        public int Overdue { get; set; }
        public int Closed { get; set; }
        public int Invalid { get; set; }
        public int Delegated { get; set; }
        public int Completed { get; set; }
        public int NewTask { get; set; }
    }
}
