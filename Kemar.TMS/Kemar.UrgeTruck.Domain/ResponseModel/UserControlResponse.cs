using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.UrgeTruck.Domain.ResponseModel
{
  

    public class UserControlResponse
    {

        public UserControlResponse()
        {
            UserAccessManagerResonse = new List<UserAccessManagerResponse>();
        }
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public virtual List<UserAccessManagerResponse> UserAccessManagerResonse { get; set; }

    }
}
