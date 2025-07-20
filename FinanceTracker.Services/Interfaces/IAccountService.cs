using FinanceTracker.Entities.Base;
using FinanceTracker.Entities.Entities;
using FinanceTracker.Services.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Services.Interfaces
{
    public interface IAccountService: IBaseService<Account, BaseGetPaginationRequest>
    {
    }
}
