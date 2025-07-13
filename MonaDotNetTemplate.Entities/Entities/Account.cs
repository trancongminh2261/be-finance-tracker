using MonaDotNetTemplate.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonaDotNetTemplate.Entities.Entities
{
    public class Account: BaseEntity
    {
        #region authen
        /// <summary>
        /// Mật khẩu
        /// </summary>
        [JsonIgnore]
        public string Password { get; set; }
        /// <summary>
        /// khóa mã hóa của mật khẩu
        /// </summary>
        [JsonIgnore]
        public string Salt { get; set; }

        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        [StringLength(30)]
        public string Username { get; set; }

        /// <summary>
        /// Id đăng nhập bằng tài khoản Google
        /// </summary>
        [JsonIgnore]
        public string AuthenGoogleId { get; set; }

        /// <summary>
        /// Id đăng nhập bằng tài khoản Facebook
        /// </summary>
        [JsonIgnore]
        public string AuthenFacebookId {  get; set; }

        [JsonIgnore]
        public string KeyForgotPassword { get; set; }
        [JsonIgnore]
        public double CreatedDateKeyForgot { get; set; }

        #endregion

        #region Noti
        /// <summary>
        /// Id gửi noti qua OneSignal
        /// </summary>
        [JsonIgnore]        
        public string OneSignalId { get; set; }

        #endregion

        /// <summary>
        /// Trạng thái tài khoản
        /// </summary>
        public int? Status { get; set; }
    }
}
