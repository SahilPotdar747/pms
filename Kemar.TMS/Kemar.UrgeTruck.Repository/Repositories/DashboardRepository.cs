using AutoMapper;
using Kemar.TMS.Domain.DTOs;
using Kemar.TMS.Repository.Interface;
using Kemar.UrgeTruck.Repository.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.TMS.Repository.Repositories
{
    public class DashboardRepository : IDashboard
    {
        private readonly IKUrgeTruckContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly ITaskTransaction _transaction;
        public DashboardRepository(IKUrgeTruckContextFactory contextFactory, IMapper mapper, ITaskTransaction transaction)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _transaction = transaction;
        }

        public async Task<List<DashboardDto>> GetDashboardData(int userId)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                List<DashboardDto> dashData = new List<DashboardDto>();
                var parameters = new List<SqlParameter>();
                //parameters.Add(new SqlParameter("@date", fromDate ?? (object)DBNull.Value));
                parameters.Add(new SqlParameter("@userId", userId));
                dashData = await kUrgeTruckContext.DashboardDto.FromSqlRaw("dashboardData @userId", parameters.ToArray()).ToListAsync();

                var departmentStatusData = await kUrgeTruckContext.DepartmentStatus.FromSqlRaw("DepartmentStatus").ToListAsync();
                if (dashData.Count > 0 && departmentStatusData.Count > 0)
                    dashData[0].departmentTaskStatuses = departmentStatusData;
                return dashData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<DepartmentTaskStatusForPieChartDto>> GetDashboardPiaData(int departmentId, int userId)
        {
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@departmentId", departmentId));
                parameters.Add(new SqlParameter("@userId", userId));
                var departmentStatusData = await kUrgeTruckContext.DepartmentTaskStatusForPieChart.FromSqlRaw("DashboardPieChartData @departmentId, @userId",parameters.ToArray()).ToListAsync();
                return departmentStatusData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



    }
}
