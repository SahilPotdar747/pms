using Kemar.UrgeTruck.Domain.Common;
using Kemar.UrgeTruck.Domain.ResponseModel;
using Kemar.UrgeTruck.Repository.Interface;
using System.Threading.Tasks;

namespace Kemar.UrgeTruck.Repository.Repositories
{
    public class ProjectVersionRepository : IProjectVersioning
    {
        public async Task<AboutUrgeTruckResponce> GetProjectVersion()
        {
            AboutUrgeTruckResponce ProjectCurrentVersion = new AboutUrgeTruckResponce();
            ProjectCurrentVersion.ProjectCurrentVersion = UrgeTruckVersion.ProjectCurrentVersion;
            return ProjectCurrentVersion;
        }
    }
}
