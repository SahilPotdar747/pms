using Kemar.TMS.Domain.ResponseModel;
using System.Collections.Generic;

namespace Kemar.UrgeTruck.Domain.ResponseModel
{
    public class RoleMasterResponse
    {
        public RoleMasterResponse()
        {
            UserAccessManager = new List<UserAccessManagerResponse>();
            userManager = new List<UserResponse>();
        }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleGroup { get; set; }
        public bool IsActive { get; set; }
        public int TotalRecord { get; set; }

        public ICollection<UserAccessManagerResponse> UserAccessManager { get; set; }
        public ICollection<UserResponse> userManager { get; set; }
    }
}
