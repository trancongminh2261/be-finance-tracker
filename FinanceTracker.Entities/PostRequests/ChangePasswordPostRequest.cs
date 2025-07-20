using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Entities.PostRequests
{
    public class ChangePasswordPostRequest
    {
        public Guid id { get; set; }
        public string old_password { get; set; }
        public string new_password { get; set; }
    }
}
