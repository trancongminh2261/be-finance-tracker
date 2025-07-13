using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonaDotNetTemplate.Entities.Base;
using MonaDotNetTemplate.Services.Interfaces.Base;
using MonaDotNetTemplate.Services.Services;
using MonaDotNetTemplate.Utilities;
using System.Net;

namespace MonaDotNetTemplate.API.Controllers.Base
{
    [ApiController]
    public abstract class BaseController<Entity, Model, GetPagingR, PostR, PutR> : ControllerBase
        where Entity : BaseEntity where Model : BaseModel where GetPagingR : BaseGetPaginationRequest
        where PostR : BasePostRequest where PutR : BasePutRequest
    {
        protected readonly ILogger<ControllerBase> logger;
        protected readonly IServiceProvider serviceProvider;
        protected IBaseService<Entity, GetPagingR> baseService;
        protected IWebHostEnvironment env;

        protected BaseController(IServiceProvider serviceProvider, ILogger<ControllerBase> logger, IWebHostEnvironment env)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
            this.env = env;
        }

        /// <summary>
        /// Lấy thông tin theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public virtual async Task<AppDataDomainResult> GetById(Guid id)
        {
            var item = await this.baseService.GetByIdAsync<Model>(id);
            if (item == null)
                throw new KeyNotFoundException();
            return new AppDataDomainResult()
            {
                Data = item,
                StatusCode = (int)HttpStatusCode.OK
            };
        }

        [HttpPost]
        public virtual async Task<AppDataDomainResult> AddItem([FromBody] PostR itemModel)
        {
            if (!ModelState.IsValid)
                throw new ArgumentException();
            var item = itemModel.Adapt<Entity>();
            var success = await this.baseService.CreateAsync(item);
            if (success)
            {
                return new AppDataDomainResult()
                {
                    Data = item,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            throw new Exception("Lỗi trong quá trình xử lý");
        }

        [HttpPut]
        public virtual async Task<AppSuccessDomainResult> UpdateItem([FromBody] PutR itemModel)
        {
            if (!ModelState.IsValid)
                throw new ArgumentException();
            var item = itemModel.Adapt<Entity>();
            var success = await this.baseService.UpdateAsync(item);
            if (success)
            {
                return new AppSuccessDomainResult()
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            throw new Exception("Lỗi trong quá trình xử lý");
        }

        /// <summary>
        /// Xóa item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual async Task<AppSuccessDomainResult> DeleteItem(Guid id)
        {
            if (!ModelState.IsValid)
                throw new ArgumentException();
            bool success = await this.baseService.DeleteAsync(id);
            if (success)
            {
                return new AppSuccessDomainResult()
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            throw new Exception("Lỗi trong quá trình xử lý");
        }

        /// <summary>
        /// Lấy danh sách item phân trang
        /// </summary>
        /// <param name="baseSearch"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<AppDomainResult> GetPagination([FromQuery] GetPagingR baseSearch)
        {
            if (!ModelState.IsValid)
                throw new ArgumentException();
            BasePagination<Model> pagedData = await this.baseService.GetPagedListData<Model>(baseSearch);
            if(!pagedData.Items.Any())
                throw new KeyNotFoundException();
            return new AppDataDomainResult
            {
                Data = pagedData,
                StatusCode = (int)HttpStatusCode.OK
            };
        }

    }
}
