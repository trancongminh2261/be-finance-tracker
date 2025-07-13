using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonaDotNetTemplate.Entities.Base
{
    public abstract class BasePaginationItem
    {

        [JsonIgnore]
        public int TotalItem { get; set; }
    }
}
