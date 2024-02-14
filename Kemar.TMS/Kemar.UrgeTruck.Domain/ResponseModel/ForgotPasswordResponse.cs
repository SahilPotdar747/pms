using Kemar.UrgeTruck.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kemar.UrgeTruck.Domain.ResponseModel
{
    public class ForgotPasswordResponse : CommonEntityModel
    {
        public int Id { get; set; }
        public string ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public string ForgetPasswordOTP { get; set; }
        public string ErrorMessage { get; set; }
        public string EmailId { get; set; }
    }
}
