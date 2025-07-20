using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Extensions
{
    public sealed class LoginContext
    {
        private static LoginContext instance = null;
        private LoginContext()
        {
        }

        public static LoginContext Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LoginContext();
                }
                return instance;
            }
        }

        public UserLoginRequest CurrentUser
        {
            get
            {
                var user = Extensions.HttpContext.Current == null ? null : (UserLoginRequest)Extensions.HttpContext.Current.Items["User"];
                return user;
            }
        }


    }

    public class UserLoginModel
    {
        public Guid id { get; set; }
        public string username { get; set; }
        public string full_name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
    }

    public class UserLoginRequest: UserLoginModel
    {
        public string language { get; set; }
    }

}
