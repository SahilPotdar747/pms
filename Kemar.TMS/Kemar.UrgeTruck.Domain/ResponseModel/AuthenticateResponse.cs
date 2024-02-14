using System.Text.Json.Serialization;

namespace Kemar.UrgeTruck.Domain.ResponseModel
{
    public class AuthenticateResponse
    {
        public AuthenticateResponse()
        {
            RoleMaster = new RoleMasterResponse();
        }

        [JsonIgnore]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
        public bool IsVerified { get; set; }
        public string JwtToken { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }
        public string UserName { get; set; }
        public string MobileNumber { get; set; }
        public bool IsActive { get; set; }
        public RoleMasterResponse RoleMaster { get; set; }
    }
}
