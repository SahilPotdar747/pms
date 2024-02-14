using System;

namespace Kemar.UrgeTruck.Repository.Entities
{
    public class ApplicationConfigMaster
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
