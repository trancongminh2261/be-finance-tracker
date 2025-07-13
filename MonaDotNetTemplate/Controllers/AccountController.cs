using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MonaDotNetTemplate.API.Controllers.Base;
using MonaDotNetTemplate.API.Extensions;
using MonaDotNetTemplate.Entities.Base;
using MonaDotNetTemplate.Entities.Entities;
using MonaDotNetTemplate.Entities.Models;
using MonaDotNetTemplate.Entities.PostRequests;
using MonaDotNetTemplate.Entities.PutRequests;
using MonaDotNetTemplate.Entities.StoreProcedureResult;
using MonaDotNetTemplate.Services.Interfaces;
using MonaDotNetTemplate.Utilities;
using System.Net;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MonaDotNetTemplate.API.Controllers
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
