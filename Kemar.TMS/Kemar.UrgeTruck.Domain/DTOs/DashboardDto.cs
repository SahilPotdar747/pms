using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.TMS.Domain.DTOs
{
    public class DashboardDto
    {
        public int ActiveProject { get; set; }
        public int NewTask { get; set; }
        public int WIPTask { get; set; }
        public int TotalEmployee { get; set; }
        public int SelfNewTask { get; set; }
        public int PendingTask { get; set; }
        public int OverdueTask { get; set; }
        public int UnAssigned { get; set; }
        public List<DepartmentTaskStatusDto> departmentTaskStatuses { get; set; }
    }

    public class DepartmentTaskStatusDto
    {
        public string DepartmentName { get; set; }
        public int NewTask { get; set; }
        public int WIPTask { get; set; }
        public int UnAssigned { get; set; }
        public int Pending { get; set; }
        public int Overdue { get; set; }
    }

    public class DepartmentTaskStatusForPieChartDto
    {
        public int NewTask { get; set; }
        public int WIPTask { get; set; }
        public int UnAssigned { get; set; }
        public int Pending { get; set; }
        public int Overdue { get; set; }
    }
}
