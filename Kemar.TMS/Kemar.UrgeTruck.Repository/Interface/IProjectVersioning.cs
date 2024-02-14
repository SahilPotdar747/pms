using Kemar.UrgeTruck.Domain.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.UrgeTruck.Repository.Interface
{
    public interface IProjectVersioning 
    {
        public Task<AboutUrgeTruckResponce> GetProjectVersion();
    }
}
