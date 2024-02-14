using Kemar.UrgeTruck.Business.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;

namespace Kemar.UrgeTruck.Api.Core.Extension
{
    public static class UTRegisterQuartzExtension
    {
        public static void RegisterUTQuartzServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionScopedJobFactory();

                // Register the job, loading the schedule from configuration
                q.AddJobAndTrigger<WBCrossCheckJob>(hostContext.Configuration);
            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        }
    }
}
