using MonaDotNetTemplate.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonaDotNetTemplate.Entities.PutRequests
{
    public class AccountPutRequest: BasePutRequest
    {

        /// <summary>
        /// Trạng thái tài khoản
        /// </summary>
        public int? Status { get; set; }
    }
}
