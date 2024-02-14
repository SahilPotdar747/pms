using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.TMS.Domain.ResponseModel
{
    public class NotificationResponse
    {
        public int NotificationId { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class NotificationCount
    {
        public int Count { get; set; }
    }
}
