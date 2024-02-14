using System.ComponentModel.DataAnnotations;

namespace Kemar.UrgeTruck.Domain.UserManagement
{
    public class ValidateResetTokenRequest
    {
        [Required]
        public string Token { get; set; }
    }
}
