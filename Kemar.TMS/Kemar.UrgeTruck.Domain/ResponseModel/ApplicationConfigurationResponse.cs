using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Kemar.UrgeTruck.Domain.ResponseModel
{
    public class ApplicationConfigurationResponse : IEqualityComparer<ApplicationConfigurationResponse>
    {

        public int AppConfigId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public bool Equals(ApplicationConfigurationResponse x, ApplicationConfigurationResponse y)
        => (x.AppConfigId, x.Key, x.Value) ==
                 (x.AppConfigId, y.Key, y.Value);

        public int GetHashCode([DisallowNull] ApplicationConfigurationResponse appConfig) =>
           appConfig?.AppConfigId.GetHashCode() ?? throw new ArgumentNullException(nameof(appConfig));

    }
}
