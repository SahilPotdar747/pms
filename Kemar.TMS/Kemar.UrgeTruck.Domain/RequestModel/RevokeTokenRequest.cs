using System.ComponentModel.DataAnnotations;

namespace Kemar.UrgeTruck.Domain.RequestModel
{
    public class RevokeTokenRequest
    {
        [Required]
        public string Token { get; set; }
    }
}
