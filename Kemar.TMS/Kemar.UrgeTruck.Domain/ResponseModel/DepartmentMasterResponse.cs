using Kemar.UrgeTruck.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.TMS.Domain.ResponseModel
{
    public class DepartmentMasterResponse 
    {
        public DepartmentMasterResponse()
        {
            userManager = new List<UserResponse>();
            taskTypeMaster = new List<TaskTypeMasterResponse>();
        }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public bool IsActive { get; set; }
        public int TotalRecord { get; set; }
        public int? coordinatingIncharge { get; set; }
        public string coordinatingInchargeName { get; set; }

        public ICollection<UserResponse> userManager { get; set; }
        public ICollection<TaskTypeMasterResponse> taskTypeMaster { get; set; }
    }
}
