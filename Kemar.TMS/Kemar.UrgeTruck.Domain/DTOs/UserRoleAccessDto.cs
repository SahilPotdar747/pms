using Kemar.UrgeTruck.Domain.ResponseModel;
using System.Collections.Generic;

namespace Kemar.UrgeTruck.Domain.DTOs
{
    public class UserRoleAccessDto
    {
        public UserRoleAccessDto()
        {
            UserAccessManagerResponse = new List<UserAccessManagerResponse>();
        }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }

        public List<UserAccessManagerResponse> UserAccessManagerResponse { get; set; }
    }


    public class AllUserRoleAccessDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string UserAccess { get; set; }
        public int IsRead { get; set; }
        public int IsCreate { get; set; }
        public int IsUpdate { get; set; }
        public int IsDelete { get; set; }
        public bool Status { get; set; }
        public int Count { get; set; }


    }
}
