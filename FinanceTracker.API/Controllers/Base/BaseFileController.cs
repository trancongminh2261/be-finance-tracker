
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using FinanceTracker.Utilities;
using System.IO;
using System.Net;

namespace FinanceTracker.API.Controller.Base
{
    [ApiController]
    public sealed class BaseFileController : ControllerBase
    {
        protected readonly IWebHostEnvironment env;
        private readonly string[] IMAGE_EXTENSION = { ".JPEG", ".PNG", ".GIF", ".TIFF", ".BMP", ".SVG", ".JPG" , ".JFIF"};
        private readonly string[] EXCEL_EXTENSION = { ".XLS", ".XLSX" };
        private readonly string[] PDF_EXTENSION = { ".PDF" };
        private readonly string[] WORD_EXTENSION = { ".DOC", ".DOCX" };

        public BaseFileController(IWebHostEnvironment env)
        {
            this.env = env;
        }

        [HttpPost("upload-file")]
        [Authorize]
        public async Task<AppDataDomainResult> UploadFile(IFormFile file)
        {            
            string fileUrl = "";
            await Task.Run(async () =>
            {
                if (file != null && file.Length <= 2097152)
                {
                    string fileExtension = Path.GetExtension(file.FileName);
                    string fileName = string.Format("{0}{1}", Guid.NewGuid().ToString(), fileExtension);
                    string upLoadFolder = "";
                    if (IMAGE_EXTENSION.Contains(fileExtension.ToUpper()))
                    {
                        upLoadFolder = "Image";
                    }
                    else if (EXCEL_EXTENSION.Contains(fileExtension.ToUpper()))
                    {
                        upLoadFolder = "Excel";
                    }
                    else if (PDF_EXTENSION.Contains(fileExtension.ToUpper()))
                    {
                        upLoadFolder = "PDF";
                    }
                    else if (WORD_EXTENSION.Contains(fileExtension.ToUpper()))
                    {
                        upLoadFolder = "Word";
                    }
                    else
                    {
                        upLoadFolder = "Other";
                    }
                    string fileUploadPath = Path.Combine(env.ContentRootPath, "Uploads", upLoadFolder);
                    string path = Path.Combine(fileUploadPath, fileName);
                    FileUtilities.createdirectory(fileUploadPath);
                    var fileByte = FileUtilities.StreamToByte(file.OpenReadStream());
                    FileUtilities.SaveToPath(path, fileByte);
                    var currentLinkSite = $"https://{FinanceTracker.Extensions.HttpContext.Current.Request.Host}/{"Uploads"}/{upLoadFolder}";
                    fileUrl = currentLinkSite+ "/"+ fileName;
                }
            });
            return new AppDataDomainResult() {
                Data = fileUrl,
                StatusCode = (int)HttpStatusCode.OK
            };
        }
    }
}
