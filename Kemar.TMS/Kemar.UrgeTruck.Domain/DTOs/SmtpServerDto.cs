using System;
using System.Collections.Generic;
using System.Text;

namespace Kemar.UrgeTruck.Domain.DTOs
{
    public class SmtpServerDto
    {
        public string ServerUrl { get; set; }
        public int Port { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; }
    }
}
