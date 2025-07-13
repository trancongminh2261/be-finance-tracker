using MonaDotNetTemplate.Entities.Base;
using MonaDotNetTemplate.Entities.Entities;
using MonaDotNetTemplate.Services.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonaDotNetTemplate.Services.Interfaces
{
    public interface IAccountInfoService: IBaseService<AccountInfo, BaseGetPaginationRequest>
    {
    }
}
