using System.Collections.Generic;

namespace Kemar.UrgeTruck.Repository.Entities
{
    public class RoleMaster
    {
        public RoleMaster()
        {
            UserAccessManager = new List<UserAccessManager>();
            UserManager = new List<UserManager>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleGroup { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<UserManager> UserManager { get; set; }
        public virtual ICollection<UserAccessManager> UserAccessManager { get; set; }
    }
}
