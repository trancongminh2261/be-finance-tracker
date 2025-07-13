using Microsoft.EntityFrameworkCore;
using MonaDotNetTemplate.Entities.Base;
using MonaDotNetTemplate.Entities.Entities;
using MonaDotNetTemplate.Repository.Interfaces;
using MonaDotNetTemplate.Services.Interfaces;
using MonaDotNetTemplate.Services.Services.Base;
using MonaDotNetTemplate.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonaDotNetTemplate.Services.Services
{
    public class AccountService : BaseService<Account, BaseGetPaginationRequest>, IAccountService
    {
        public AccountService(IUnitOfWork unitOfWork, IAppDbContext context) : base(unitOfWork, context)
        {
        }
    }
}
