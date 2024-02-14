using Kemar.TMS.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.TMS.Repository.Interface
{
    public interface IDashboard
    {
        Task<List<DashboardDto>> GetDashboardData(int UserID);
        Task<List<DepartmentTaskStatusForPieChartDto>> GetDashboardPiaData(int departmentId, int userId);
    }
}
