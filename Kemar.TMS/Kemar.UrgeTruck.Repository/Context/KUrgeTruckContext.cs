using Kemar.TMS.Domain.DTOs;
using Kemar.TMS.Repository.Entities;
using Kemar.TMS.Repository.EntityConfiguration;
using Kemar.UrgeTruck.Domain.SPDTOs;
using Kemar.UrgeTruck.Repository.Entities;
using Kemar.UrgeTruck.Repository.EntityConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Kemar.UrgeTruck.Repository.Context
{
    public class KUrgeTruckContext : DbContext
    {
        public DbContextOptions<KUrgeTruckContext> Options { get; }

        public KUrgeTruckContext() { }



        public KUrgeTruckContext(DbContextOptions<KUrgeTruckContext> options)
            : base(options)
        {
            Options = options;
        }

        public virtual DbSet<RoleMaster> RoleMaster { get; set; }
        public virtual DbSet<UserScreenMaster> UserScreenMaster { get; set; }
        public virtual DbSet<UserAccessManager> UserAccessManager { get; set; }
        public virtual DbSet<UserManager> UserManager { get; set; }
        public virtual DbSet<ApplicationConfigMaster> ApplicationConfigMaster { get; set; }
        public virtual DbSet<TransGenerator> TransGenerator { get; set; }
        public virtual DbSet<DesignationMaster> DesignationMaster { get; set; }
        public virtual DbSet<DepartmentMaster> DepartmentMaster { get; set; }
        public virtual DbSet<TaskTypeMaster> TaskTypeMaster { get; set; }
        public virtual DbSet<ProjectMaster> ProjectMaster { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<TaskTransaction> TaskTransaction { get; set; }
        public virtual DbSet<DelegateHistory> DelegateHistory { get; set; }
        public virtual DbSet<TaskHistory> TaskHistory { get; set; }
        // Store Procedure
        public virtual DbSet<DashboardDto> DashboardDto { get; set; }
        public virtual DbSet<DepartmentTaskStatusDto> DepartmentStatus { get; set; }
        public virtual DbSet<DepartmentTaskStatusForPieChartDto> DepartmentTaskStatusForPieChart { get; set; }
        public virtual DbSet<UserWiseTaskDataCountReport> UserWiseTaskDataCountReport { get; set; }
        public virtual DbSet<ProjectWiseTaskCountReport> ProjectWiseTaskCountReport { get; set; }


        // Transaction tables

        //Stored Procedure Model
        //public virtual DbSet<CommonMasterData> CommonMasterData { get; set; }

        [System.Obsolete]
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleMasterConfiguration());
            modelBuilder.ApplyConfiguration(new UserScreenMasterConfiguration());
            modelBuilder.ApplyConfiguration(new UserAccessManagerConfiguration());
            modelBuilder.ApplyConfiguration(new UserManagerConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationConfigMasterConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentMasterConfiguration());
            modelBuilder.ApplyConfiguration(new DesignationMasterConfiguration());
            modelBuilder.ApplyConfiguration(new TaskTypeMasterConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectMasterConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new TaskTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new DelegateHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new TaskHistoryConfiguration());

            // Transaction



            modelBuilder.ApplyConfiguration(new EmailConfigConfiguration());


            modelBuilder.Entity<DashboardDto>().HasNoKey()
                .ToTable("DashboardDto", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<DepartmentTaskStatusForPieChartDto>().HasNoKey()
                .ToTable("DashboardPieChartData", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<DepartmentTaskStatusDto>().HasNoKey()
                .ToTable("DepartmentStatus", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<UserWiseTaskDataCountReport>().HasNoKey()
                .ToTable("UserWiseTaskDataCount", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<ProjectWiseTaskCountReport>().HasNoKey()
                .ToTable("ProjectTaskCountData", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<UserWiseTaskDataCountReport>().HasNoKey()
                .ToTable("AllUserWiseTaskDataCount", t => t.ExcludeFromMigrations());
        }
    }
}
