using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FinanceTracker.Utilities.CoreContant;

namespace FinanceTracker.Utilities
{
    public class AppException: Exception
    {
        public object[] Param {  get; set; }
        public ResponseMessageType Code { get; set; }

        public AppException(ResponseMessageType code, object[] param)
        {
            Param = param;
            Code = code;
        }

        public AppException(ResponseMessageType code)
        {
            Code = code;
        }

        public AppException(string? message) : base(message)
        {
        }
    }
}
