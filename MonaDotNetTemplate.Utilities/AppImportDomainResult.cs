using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonaDotNetTemplate.Utilities
{
    public class AppImportDomainResult: AppDomainResult
    {
        public byte[]? ResultFile { get; set; }

        public string? DownloadFileName { get; set; }
    }
}
