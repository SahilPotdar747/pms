using System;

namespace Kemar.UrgeTruck.Domain.SPDTOs
{
    public class ApiTrackingReportSPdto
    {
        public string requestJson { get; set; }
        public string requestId { get; set; }
        public DateTime? InitiatedTime { get; set; }
        public string TransactionStatus { get; set; }
        public DateTime? CompletedTime { get; set; }
    }
}
