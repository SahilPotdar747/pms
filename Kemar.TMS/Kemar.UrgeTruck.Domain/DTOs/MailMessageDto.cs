namespace Kemar.UrgeTruck.Domain.DTOs
{

    public class MailMessageDto
    {
        public string MailFrom { get; set; } 
        public string MailTo { get; set; }
        public string MailBcc{ get; set; }
        public string MailCc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        
    }
}
