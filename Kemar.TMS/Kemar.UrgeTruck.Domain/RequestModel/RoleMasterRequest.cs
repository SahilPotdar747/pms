using System.Collections.Generic;

namespace Kemar.UrgeTruck.Domain.RequestModel
{
    public class RoleMasterRequest
    {
        public RoleMasterRequest()
        {
            UserAccessManager = new List<UserAccessManagerRequest>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleGroup { get; set; }
        public bool IsActive { get; set; }

        public ICollection<UserAccessManagerRequest> UserAccessManager { get; set; }
    }
}
