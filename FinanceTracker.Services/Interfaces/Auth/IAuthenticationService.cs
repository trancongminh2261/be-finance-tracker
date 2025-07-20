using FinanceTracker.Entities.Entities;
using FinanceTracker.Entities.Models;
using FinanceTracker.Entities.PostRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Services.Interfaces.Auth
{
    public interface IAuthenticationService
    {
        Task<AuthenticationModel> ChangePasswordAsync(ChangePasswordPostRequest chargePassword);
        Task<AuthenticationModel> LoginAsync(LoginPostRequest login);
        Task<AuthenticationModel> RegisterAsync(RegisterPostRequest register);
        Task<AuthenticationModel> RefreshAsync(string refresh);
    }
}
