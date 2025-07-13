using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using MonaDotNetTemplate.Entities.Base;
using MonaDotNetTemplate.Entities.Entities;
using MonaDotNetTemplate.Entities.Models;
using MonaDotNetTemplate.Entities.PostRequests;
using MonaDotNetTemplate.Extensions;
using MonaDotNetTemplate.Repository;
using MonaDotNetTemplate.Repository.Interfaces;
using MonaDotNetTemplate.Services.Interfaces.Auth;
using MonaDotNetTemplate.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MonaDotNetTemplate.Services.Services.Auth
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
            var account = await unitOfWork.Repository<Account>().GetQueryable().SingleOrDefaultAsync(x => x.Id == chargePassword.Id);
            if (account == null)
            {
                throw new AppException(CoreContant.ResponseMessageType.NotFound, [nameof(Account)]);
            }
            chargePassword.OldPassword = SecurityUtilities.Encode(chargePassword.OldPassword, account.Salt);
            if (account.Password != chargePassword.OldPassword)
            {
                throw new AppException(CoreContant.ResponseMessageType.BadRequest, ["Password"]);
            }
            account.Password = SecurityUtilities.Encode(chargePassword.NewPassword, account.Salt);
            unitOfWork.Repository<Account>().Update(account);
            await unitOfWork.SaveAsync();
            return new()
            {
                Token = await GenerateJwtTokenForLogin(account),
                Refresh = GenerateRefreshToken(account.Id.ToString())
            };
        }

        public async Task<AuthenticationModel> LoginAsync(LoginPostRequest login)
        {

            var account = await unitOfWork.Repository<Account>().GetQueryable().Where(x => x.Username == login.Username).FirstOrDefaultAsync();
            if (account == null)
            {
                throw new AppException(CoreContant.ResponseMessageType.BadRequest, ["Username"]);
            }
            login.Password = SecurityUtilities.Encode(login.Password, account.Salt);
            if (account.Password != login.Password)
            {
                throw new AppException(CoreContant.ResponseMessageType.BadRequest, ["Password"]);
            }
            return new()
            {
                Token = await GenerateJwtTokenForLogin(account),
                Refresh = GenerateRefreshToken(account.Id.ToString())
            };
        }

        public async Task<AuthenticationModel> RegisterAsync(RegisterPostRequest register)
        {


            var account = await unitOfWork.Repository<Account>().GetQueryable().Where(x => x.Username == register.Username).FirstOrDefaultAsync();
            if (account != null)
            {
                throw new AppException(CoreContant.ResponseMessageType.BadRequest, ["Username"]);
            }
            var newAccount = register.Adapt<Account>();
            var newAccountInfo = register.Adapt<AccountInfo>();
            newAccount.Salt = Guid.NewGuid().ToString();
            newAccount.Password = SecurityUtilities.Encode(newAccount.Password, newAccount.Salt);
            await unitOfWork.Repository<Account>().CreateAsync(newAccount);
            await unitOfWork.SaveAsync();
            newAccountInfo.AccountId = newAccount.Id;
            await unitOfWork.Repository<AccountInfo>().CreateAsync(newAccountInfo);
            await unitOfWork.SaveAsync();
            return new()
            {
                Token = await GenerateJwtTokenForLogin(newAccount),
                Refresh = GenerateRefreshToken(newAccount.Id.ToString())
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
                var account = await unitOfWork.Repository<Account>().GetQueryable().SingleOrDefaultAsync(x => x.Id == id);
                return new()
                {
                    Token = await GenerateJwtTokenForLogin(account),
                    Refresh = GenerateRefreshToken(account.Id.ToString())
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
            var accountInfo = await unitOfWork.Repository<AccountInfo>().GetQueryable().AsNoTracking().SingleOrDefaultAsync(x=> x.AccountId == account.Id);
            UserLoginModel loginModel = accountInfo.Adapt<UserLoginModel>();
            loginModel.Username = account.Username;
            loginModel.Id = account.Id;
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
