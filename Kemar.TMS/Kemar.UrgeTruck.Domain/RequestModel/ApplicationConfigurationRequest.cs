using System;

namespace Kemar.UrgeTruck.Domain.RequestModel
{
    public class ApplicationConfigurationRequest
    {
        public int AppConfigId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
