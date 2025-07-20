using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using FinanceTracker.Entities.Base;
using FinanceTracker.Entities.Entities;
using FinanceTracker.Entities.Models;
using FinanceTracker.Entities.PostRequests;
using FinanceTracker.Extensions;
using FinanceTracker.Repository;
using FinanceTracker.Repository.Interfaces;
using FinanceTracker.Services.Interfaces.Auth;
using FinanceTracker.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Services.Services.Auth
{
    public class AuthenticationService: IAuthenticationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;

        public AuthenticationService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.configuration = configuration;
        }

        public async Task<AuthenticationModel> ChangePasswordAsync(ChangePasswordPostRequest chargePassword)
        {
            var account = await unitOfWork.Repository<Account>().GetQueryable().SingleOrDefaultAsync(x => x.id == chargePassword.id);
            if (account == null)
            {
                throw new AppException(CoreContant.ResponseMessageType.NotFound, [nameof(Account)]);
            }
            chargePassword.old_password = SecurityUtilities.Encode(chargePassword.old_password, account.salt);
            if (account.password != chargePassword.old_password)
            {
                throw new AppException(CoreContant.ResponseMessageType.BadRequest, ["password"]);
            }
            account.password = SecurityUtilities.Encode(chargePassword.new_password, account.salt);
            unitOfWork.Repository<Account>().Update(account);
            await unitOfWork.SaveAsync();
            return new()
            {
                Token = await GenerateJwtTokenForLogin(account),
                Refresh = GenerateRefreshToken(account.id.ToString())
            };
        }

        public async Task<AuthenticationModel> LoginAsync(LoginPostRequest login)
        {

            var account = await unitOfWork.Repository<Account>().GetQueryable().Where(x => x.username == login.username).FirstOrDefaultAsync();
            if (account == null)
            {
                throw new AppException(CoreContant.ResponseMessageType.BadRequest, ["username"]);
            }
            login.password = SecurityUtilities.Encode(login.password, account.salt);
            if (account.password != login.password)
            {
                throw new AppException(CoreContant.ResponseMessageType.BadRequest, ["password"]);
            }
            return new()
            {
                Token = await GenerateJwtTokenForLogin(account),
                Refresh = GenerateRefreshToken(account.id.ToString())
            };
        }

        public async Task<AuthenticationModel> RegisterAsync(RegisterPostRequest register)
        {
            var account = await unitOfWork.Repository<Account>().GetQueryable().Where(x => x.username == register.username).FirstOrDefaultAsync();
            if (account != null)
            {
                throw new AppException(CoreContant.ResponseMessageType.BadRequest, ["username"]);
            }
            var newAccount = register.Adapt<Account>();
            newAccount.salt = Guid.NewGuid().ToString();
            newAccount.password = SecurityUtilities.Encode(newAccount.password, newAccount.salt);
            await unitOfWork.Repository<Account>().CreateAsync(newAccount);
            await unitOfWork.SaveAsync();
            return new()
            {
                Token = await GenerateJwtTokenForLogin(newAccount),
                Refresh = GenerateRefreshToken(newAccount.id.ToString())
            };
        }

        public async Task<AuthenticationModel> RefreshAsync(string refresh)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();
            string key = appSettings.Refresh;
            var decodeString = SecurityUtilities.DecodeString(refresh, key).Split('&');
            double exDate = Convert.ToDouble(decodeString[1]);
            if (exDate >= Timestamp.Now)
            {
                Guid id = new Guid(decodeString[0]);
                var account = await unitOfWork.Repository<Account>().GetQueryable().SingleOrDefaultAsync(x => x.id == id);
                return new()
                {
                    Token = await GenerateJwtTokenForLogin(account),
                    Refresh = GenerateRefreshToken(account.id.ToString())
                };
            }
            throw new AppException(CoreContant.ResponseMessageType.Unauthenticaion);
        }

        private string GenerateRefreshToken(string id)
        {
            id += "&" + Timestamp.Now.AddDay(10).ToString() + "&" + Guid.NewGuid().ToString();
            var appSettingsSection = configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();
            string key = appSettings.Refresh;
            return SecurityUtilities.EncodeString(id, key);
        }

        private async Task<string> GenerateJwtTokenForLogin(Account account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var appSettingsSection = configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            UserLoginModel loginModel = account.Adapt<UserLoginModel>();
            loginModel.username = account.username;
            loginModel.id = account.id;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(loginModel))
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
