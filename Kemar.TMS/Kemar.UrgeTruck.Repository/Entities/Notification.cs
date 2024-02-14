using Kemar.UrgeTruck.Domain.Common;
using Kemar.UrgeTruck.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.TMS.Repository.Entities
{
    public class Notification : CommonEntityModel
    {
        public int NotificationId { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public int UserId { get; set; }
        public bool PushedToDesktop { get; set; }
        public bool PushedToMobile { get; set; }
        public bool IsDeleted { get; set; }
        public virtual UserManager UserManager { get; set; }
    }
}
