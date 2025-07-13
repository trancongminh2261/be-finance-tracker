using MonaDotNetTemplate.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonaDotNetTemplate.Entities.PutRequests
{
    public class RolePutRequest: BasePutRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
