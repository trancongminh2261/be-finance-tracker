using Microsoft.EntityFrameworkCore;
using FinanceTracker.Entities.Base;
using FinanceTracker.Entities.Entities;
using FinanceTracker.Repository.Interfaces;
using FinanceTracker.Services.Interfaces;
using FinanceTracker.Services.Services.Base;
using FinanceTracker.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Services.Services
{
    public class AccountService : BaseService<Account, BaseGetPaginationRequest>, IAccountService
    {
        public AccountService(IUnitOfWork unitOfWork, IAppDbContext context) : base(unitOfWork, context)
        {
        }
    }
}
