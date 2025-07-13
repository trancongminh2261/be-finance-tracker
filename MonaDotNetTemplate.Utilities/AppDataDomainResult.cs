using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonaDotNetTemplate.Utilities
{
    public class AppDataDomainResult: AppDomainResult
    {
        public object? Data { get; set; }
    }
}
