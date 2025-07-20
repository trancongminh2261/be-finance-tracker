using FinanceTracker.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Entities.PutRequests
{
    public class AccountPutRequest: BasePutRequest
    {
        #region thông tin cá nhân
        /// <summary>
        /// họ tên
        /// </summary>
        public string full_name { get; set; }
        /// <summary>
        /// số điện thoại
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// email
        /// </summary>
        [EmailAddress]
        public string email { get; set; }
        #endregion 

        #region authen
        /// <summary>
        /// tên đăng nhập
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// mật khẩu
        /// </summary>
        public string password { get; set; }
        #endregion
    }
}
