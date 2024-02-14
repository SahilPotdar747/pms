using System.ComponentModel.DataAnnotations;

namespace Kemar.UrgeTruck.Domain.RequestModel
{
    public class ForgotPasswordRequest
    {
        [Required]
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
