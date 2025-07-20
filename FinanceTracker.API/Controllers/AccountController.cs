using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using FinanceTracker.API.Controllers.Base;
using FinanceTracker.API.Extensions;
using FinanceTracker.Entities.Base;
using FinanceTracker.Entities.Entities;
using FinanceTracker.Entities.Models;
using FinanceTracker.Entities.PostRequests;
using FinanceTracker.Entities.PutRequests;
using FinanceTracker.Entities.StoreProcedureResult;
using FinanceTracker.Services.Interfaces;
using FinanceTracker.Utilities;
using System.Net;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?Linkid=397860

namespace FinanceTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController<Account, AccountModel, BaseGetPaginationRequest, AccountPostRequest, AccountPutRequest>
    {
        public AccountController(IServiceProvider serviceProvider, ILogger<ControllerBase> logger, IWebHostEnvironment env) : base(serviceProvider, logger, env)
        {
            this.baseService = serviceProvider.GetRequiredService<IAccountService>();
            this.baseService.GetSQLFilePath = "SQL/AccountGetPaging.sql";
        }
    }
}
