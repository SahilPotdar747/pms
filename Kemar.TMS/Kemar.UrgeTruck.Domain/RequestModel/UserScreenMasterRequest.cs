using System.Collections.Generic;

namespace Kemar.UrgeTruck.Domain.RequestModel
{
    public class UserScreenMasterRequest
    {
        public UserScreenMasterRequest()
        {
            UserAccessManager = new List<UserAccessManagerRequest>();
        }

        public int UserScreenId { get; set; }
        public string ScreenName { get; set; }
        public string ScreenCode { get; set; }
        public bool IsActive { get; set; }

        public ICollection<UserAccessManagerRequest> UserAccessManager { get; set; }
    }
}
