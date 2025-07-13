using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonaDotNetTemplate.Entities.Base
{
    public class BaseModel: BasePaginationItem
    {
        public Guid? Id { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        [Timestamp]
        public double? Created { get; set; }

        /// <summary>
        /// Người cập nhật
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Ngày cập nhật
        /// </summary>
        [Timestamp]
        public double? Updated { get; set; }
    }
}
