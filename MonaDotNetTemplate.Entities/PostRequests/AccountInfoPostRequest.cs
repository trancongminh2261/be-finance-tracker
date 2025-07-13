using MonaDotNetTemplate.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonaDotNetTemplate.Entities.PostRequests
{
    public class AccountInfoPostRequest: BasePostRequest
    {
        #region info

        /// <summary>
        /// id của account
        /// </summary>
        public Guid? AccountId { get; set; }

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

        #endregion
    }
}
