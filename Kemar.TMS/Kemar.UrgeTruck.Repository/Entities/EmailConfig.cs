namespace Kemar.UrgeTruck.Repository.Entities
{
    public class EmailConfig
    {
        public int EmailId { get; set; }
        public string Key { get; set; }
        public string Body { get; set; }
        public string To { get; set; }
        public string CC { get; set; }
        public string From { get; set; }
        public bool IsActive { get; set; }
    }
}
