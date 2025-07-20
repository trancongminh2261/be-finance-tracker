using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Entities.Models
{
    public class AuthenticationModel
    {
        public string Token { get; set; }
        public string Refresh { get; set; }
    }
}
