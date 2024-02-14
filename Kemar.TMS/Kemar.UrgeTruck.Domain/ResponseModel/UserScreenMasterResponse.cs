using System.Collections.Generic;

namespace Kemar.UrgeTruck.Domain.ResponseModel
{
    public class UserScreenMasterResponse
    {
        public UserScreenMasterResponse()
        {
            UserAccessManager = new List<UserAccessManagerResponse>();
        }

        public int UserScreenId { get; set; }
        public string MenuName { get; set; }
        public string ScreenName { get; set; }
        public string ScreenCode { get; set; }
        public int ParentId { get; set; }
        public string RoutingURL { get; set; }
        public string MenuIcon { get; set; }
        public bool IsActive { get; set; }

        public ICollection<UserAccessManagerResponse> UserAccessManager { get; set; }
    }
}
