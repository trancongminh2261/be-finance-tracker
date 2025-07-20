using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Utilities
{
    public class CoreContant
    {
        public enum ResponseMessageType {
            Unauthenticaion,
            NotFound,
            Success,
            BadRequest,
            Locked,
            Unauthorized,
            RequestTimeout,
            Forbidden,
            Duplicate
        }
    }
}
