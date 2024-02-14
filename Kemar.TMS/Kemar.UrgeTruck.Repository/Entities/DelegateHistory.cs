using Kemar.UrgeTruck.Domain.Common;
using Kemar.UrgeTruck.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.TMS.Repository.Entities
{
    public class DelegateHistory : CommonEntityModel
    {
        public int delegateHistoryId { get; set; }
        public string RaisedBy { get; set; }
        public int TaskId { get; set; }
        public int TransferToId { get; set; }
        public string TransferTo { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public bool isActive { get; set; }
        public string RejectRemarks { get; set; }
        public virtual TaskTransaction TaskTransaction { get; set; }
    }
}
