using FinanceTracker.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinanceTracker.Entities.PostRequests
{
    public class AccountPostRequest: BasePostRequest
    {
        #region thông tin cá nhân
        /// <summary>
        /// họ tên
        /// </summary>
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string full_name { get; set; }
        /// <summary>
        /// số điện thoại
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// email
        /// </summary>
        [EmailAddress]
        [Required(ErrorMessage = "Vui lòng nhập email")]
        public string email { get; set; }
        #endregion 

        #region authen
        /// <summary>
        /// tên đăng nhập
        /// </summary>
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        public string username { get; set; }
        /// <summary>
        /// mật khẩu
        /// </summary>
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string password { get; set; }
        #endregion
    }
}
