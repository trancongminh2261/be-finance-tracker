using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Entities.Base
{
    public class BaseGetPaginationRequest
    {
        /// <summary>
        /// trang hiện tại
        /// </summary>
        public int? PageIndex { get; set; }

        /// <summary>
        /// kích thước trang
        /// </summary>
        public int? PageSize { get; set;}
    }
}
