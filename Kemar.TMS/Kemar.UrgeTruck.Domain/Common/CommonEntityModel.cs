using System;

namespace Kemar.UrgeTruck.Domain.Common
{
    public class CommonEntityModel
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
