using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.UrgeTruck.Domain.ResponseModel
{
   public class UserRoleScreenMappingResponse
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string PageAcessDescription{ get; set; }
        public bool IsActive { get; set; }
        public int PageCount { get; set; }

    }


   
}
