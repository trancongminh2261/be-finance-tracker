using MonaDotNetTemplate.Entities.Entities;
using MonaDotNetTemplate.Entities.Models;
using MonaDotNetTemplate.Entities.PostRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonaDotNetTemplate.Services.Interfaces.Auth
{
    public interface IAuthenticationService
    {
        Task<AuthenticationModel> ChangePasswordAsync(ChangePasswordPostRequest chargePassword);
        Task<AuthenticationModel> LoginAsync(LoginPostRequest login);
        Task<AuthenticationModel> RegisterAsync(RegisterPostRequest register);
        Task<AuthenticationModel> RefreshAsync(string refresh);
    }
}
