using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonaDotNetTemplate.Extensions
{
    public static class HttpContext
    {
        private static IHttpContextAccessor _contextAccessor;
        public static IHttpContextAccessor ContextAccessor { set { _contextAccessor = value; } }
        public static Microsoft.AspNetCore.Http.HttpContext Current => _contextAccessor.HttpContext;
    }
}
