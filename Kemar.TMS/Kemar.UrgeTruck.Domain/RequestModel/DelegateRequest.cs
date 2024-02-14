using Kemar.UrgeTruck.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.TMS.Domain.RequestModel
{
    public class DelegateRequest : CommonEntityModel
    {
        public int delegateHistoryId { get; set; }
        public string RaisedBy { get; set; }
        public int RaisedById { get; set; }
        public int TaskId { get; set; }
        public int TransferToId { get; set; }
        public string Remarks { get; set; }
    }

    public class DelegateActionRequest
    {
        public int delegateHistoryId { get; set; }
        public string Status { get; set; }
        public string RejectRemarks { get; set; }
    }
}
