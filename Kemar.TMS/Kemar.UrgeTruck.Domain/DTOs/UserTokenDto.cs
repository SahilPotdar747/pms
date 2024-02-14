using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kemar.UrgeTruck.Domain.DTOs
{
    public class UserTokenDto
    {
        public UserTokenDto()
        {
            UserAccess = new List<UserAccessDto>();
            MenuAccess = new List<MenuAccessDto>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string RoleName { get; set; }
        public bool IsVerified { get; set; }
        public string JwtToken { get; set; }
        public int DesignationId { get; set; }
        public int DepartmentId { get; set; }
        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string empCode { get; set; }
        public string ProfileImagePath { get; set; }

        public ICollection<UserAccessDto> UserAccess { get; set; }

        public ICollection<MenuAccessDto> MenuAccess { get; set; }
    }

    public class UserAccessDto
    {
        public string ScreenCode { get; set; }
        public bool CanRead { get; set; }
        public bool CanCreate { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDeactivate { get; set; }
    }
}
