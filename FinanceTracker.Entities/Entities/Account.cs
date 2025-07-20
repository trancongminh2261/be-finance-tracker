using FinanceTracker.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinanceTracker.Entities.Entities
{
    public class Account: BaseEntity
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
        [JsonIgnore]
        public string password { get; set; }
        /// <summary>
        /// khóa mã hóa của mật khẩu
        /// </summary>
        [JsonIgnore]
        public string salt { get; set; }
        /// <summary>
        /// key dùng khi sài tính năng quên mật khẩu
        /// </summary>
        [JsonIgnore]
        public string key_forgot_password { get; set; }
        /// <summary>
        /// ngày tạo key tính năng quên mật khẩu
        /// </summary>
        [JsonIgnore]
        public double? created_key_forgot { get; set; }
        #endregion
    }
}
