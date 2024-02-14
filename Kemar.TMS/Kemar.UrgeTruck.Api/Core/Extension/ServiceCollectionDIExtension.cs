using Kemar.TMS.Business.BusinessLogic;
using Kemar.TMS.Business.Interfaces;
using Kemar.TMS.Business.Services;
using Kemar.TMS.Repository.Interface;
using Kemar.TMS.Repository.Repositories;
using Kemar.UrgeTruck.Business.BusinessLogic;
using Kemar.UrgeTruck.Business.Interfaces;
using Kemar.UrgeTruck.Repository.Context;
using Kemar.UrgeTruck.Repository.Interface;
using Kemar.UrgeTruck.Repository.Repositories;
using Kemar.UrgeTruck.Repository.Repositories.Interface;
using Kemar.UrgeTruck.ServiceIntegration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Kemar.UrgeTruck.Api.Core.Extension
{
    public static class ServiceCollectionDIExtension
    {
        public static void 
            ConfigureServicesDependency(IServiceCollection services)
        {
            RepositoryDependency(services);
            BusinessDepedency(services);
            ServiceDependency(services);
            ControlTowerDependency(services);
            SchedulerDependency(services);

            //Services
            services.AddHostedService<CheckTaskStatus>();

            // Configure retry resilience service
            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            //services.AddHostedService<QuartzHostedService>();
        }

        private static void SchedulerDependency(IServiceCollection services)
        {
            
        }

        private static void RepositoryDependency(IServiceCollection services)
        {
            services.AddSingleton<IKUrgeTruckContextFactory, KUrgeTruckContextFactory>();
            services.AddScoped<IUserManager, UserManagerRepository>();
          
            services.AddSingleton<IApplicationConfiguration, ApplicationConfigurationRepository>();
            services.AddScoped<IRoleRegistration, RoleRegistrationRepository>();
            services.AddScoped<IUserScreen, UserScreenRepository>();
            services.AddScoped<IUserAccessManager, UserAccessManagerRepository>();
            services.AddScoped<IProjectVersioning, ProjectVersionRepository>();
            services.AddScoped<IDepartmentRegistration, DepartmentRegistrationRepository>();
            services.AddScoped<IDesignationRegistration, DesignationRegistrationRepository>();
            services.AddScoped<IProjectRegistration, ProjectRegistrationRepository>();
            services.AddScoped<ITaskTypeRegistration, TaskTypeRegistrationRepository>();
            services.AddScoped<ITaskTransaction, TaskTransactionRepository>();
            services.AddScoped<INotification, NotificationRepository>();
            services.AddScoped<ITaskHistory, TaskHistoryRepository>();
            services.AddScoped<IDelegateHistory, DelegateHistoryRepository>();
            services.AddScoped<IDashboard, DashboardRepository>();
            services.AddScoped<IReport, ReportRepositories>();
            // services.AddScoped<ICommonMasterData, CommonMasterDataRepository>();


        }

        private static void BusinessDepedency(IServiceCollection services)
        {
            services.AddScoped<IUserRoleAccessManager, UserRoleAccessManager>();
            services.AddScoped<ICheckTaskStatus, CheckTaskStatusBusinessLogic>();
        }

        private static void ServiceDependency(IServiceCollection services)
        {
            services.AddScoped<IEmailManager, EmailManager>();
        }

        private static void ControlTowerDependency(IServiceCollection services)
        {
        }
    }
}
