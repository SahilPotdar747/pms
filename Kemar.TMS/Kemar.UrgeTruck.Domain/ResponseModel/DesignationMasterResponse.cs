using Kemar.UrgeTruck.Domain.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.TMS.Domain.ResponseModel
{
    public class DesignationMasterResponse
    {
        public DesignationMasterResponse()
        {
            userManager = new List<UserResponse>();
        }
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public bool IsActive { get; set; }
        public int TotalRecord { get; set; }

        public ICollection<UserResponse> userManager { get; set; }
    }
}
