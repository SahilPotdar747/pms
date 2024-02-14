using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.TMS.Domain.RequestModel
{
    public class RepoenedRequest
    {
        public int TaskId { get;set;}
        public string Remarks { get;set;}
        public string ReopedBy { get; set; }
    }
}
