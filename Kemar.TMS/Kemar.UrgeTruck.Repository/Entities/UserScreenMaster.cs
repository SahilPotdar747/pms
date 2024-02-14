using System.Collections.Generic;

namespace Kemar.UrgeTruck.Repository.Entities
{
    public class UserScreenMaster
    {
        public UserScreenMaster()
        {
            UserAccessManager = new List<UserAccessManager>();
        }

        public int UserScreenId { get; set; }
        public string MenuName { get; set; } 
        public string ScreenName { get; set; }
        public string ScreenCode { get; set; }
        public int ParentId { get; set; }
        public string RoutingURL { get; set; }
        public string MenuIcon { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<UserAccessManager> UserAccessManager { get; set; }
    }
}
