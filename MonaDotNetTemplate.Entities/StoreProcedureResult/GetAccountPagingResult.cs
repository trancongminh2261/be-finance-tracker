using MonaDotNetTemplate.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonaDotNetTemplate.Entities.StoreProcedureResult
{
    public class GetAccountPagingResult: BaseModel
    {

        public string Username { get; set; }

        /// <summary>
        /// Trạng thái tài khoản
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// tên tài khoản
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }
    }
}
