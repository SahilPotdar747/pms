using Kemar.UrgeTruck.Domain.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.UrgeTruck.Domain.ResponseModel
{
   public class UserControRequest
    {

            public UserControRequest()
            {
            UserAccessManagerRequest = new List<UserAccessManagerRequest>();
            }
            public int RoleId { get; set; }
            

            public virtual List<UserAccessManagerRequest> UserAccessManagerRequest { get; set; }
        
    }
}
