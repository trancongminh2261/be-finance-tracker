using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace FinanceTracker.Entities.Base
{
    public class BaseEntity
    {
        /// <summary>
        /// id, khóa chính
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Guid id { get; set; }
        /// <summary>
        /// Người tạo
        /// </summary>
        public Guid? created_by { get; set; }
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public double? created { get; set; }
        /// <summary>
        /// Người cập nhật
        /// </summary>
        public Guid? updated_by { get; set; }
        /// <summary>
        /// Ngày cập nhật
        /// </summary>
        public double? updated { get; set; }
        /// <summary>
        /// Cờ xóa
        /// </summary>
        public bool is_deleted { get; set; } = false;
    }
}
