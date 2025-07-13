using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonaDotNetTemplate.Entities.Base
{
    public class BasePagination<T> where T : BasePaginationItem
    {
        public int PageIndex { set; get; }
        public int PageSize { set; get; }
        public int TotalPage
        {
            get
            {
                if (PageIndex == 0 && PageSize == 0)
                    return 0;
                decimal count = this.TotalItem;
                if (count > 0)
                    return (int)Math.Ceiling(count / PageSize);
                else return 0;
            }
        }
        public int TotalItem { set; get; }
        public IList<T> Items { set; get; }
    }
}
