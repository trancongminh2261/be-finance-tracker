using MonaDotNetTemplate.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonaDotNetTemplate.Entities.PostRequests
{
    public class AccountPostRequest: BasePostRequest
    {
        #region authen
        /// <summary>
        /// Mật khẩu
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        [StringLength(30)]
        public string Username { get; set; }

        #endregion

        /// <summary>
        /// Trạng thái tài khoản
        /// </summary>
        public int? Status { get; set; }
    }
}
