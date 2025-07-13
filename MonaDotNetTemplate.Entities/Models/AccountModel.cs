using MonaDotNetTemplate.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonaDotNetTemplate.Entities.Models
{
    public class AccountModel: BaseModel
    {
        public int? Status { get; set; }
        public string Username { get; set; }
    }
}
