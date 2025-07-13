using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonaDotNetTemplate.Entities.PostRequests
{
    public class RegisterPostRequest
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

        /// <summary>
        /// Id đăng nhập bằng tài khoản Google
        /// </summary>
        public string AuthenGoogleId { get; set; }

        /// <summary>
        /// Id đăng nhập bằng tài khoản Facebook
        /// </summary>
        public string AuthenFacebookId { get; set; }

        #endregion



        #region Noti
        /// <summary>
        /// Id gửi noti qua OneSignal
        /// </summary>
        public string OneSignalId { get; set; }


        #endregion

        /// <summary>
        /// Trạng thái tài khoản
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// tên tài khoản
        /// </summary>
        [StringLength(100)]
        public string FullName { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        [StringLength(11)]
        public string Phone { get; set; }

        /// <summary>
        /// email
        /// </summary>
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        [StringLength(250)]
        public string Address { get; set; }
    }
}
