using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonaDotNetTemplate.Extensions
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
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }

    public class UserLoginRequest: UserLoginModel
    {
        public string Language { get; set; }
    }

}
