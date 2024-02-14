using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.UrgeTruck.Repository.Entities
{
    public class TransGenerator
    {
        [Key]
        public int TransactionIdKey { get; set; }
        public int Year { get; set;  }
        public string TransactionType { get; set;  }  // I- Inbound , O- Outboud , P - InPlant
        public int LastTransactionNumber { get; set;  }
    }
}
