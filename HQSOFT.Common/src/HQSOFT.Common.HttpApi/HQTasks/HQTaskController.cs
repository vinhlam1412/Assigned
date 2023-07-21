using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using HQSOFT.Common.HQTasks;
using Volo.Abp.Content;
using HQSOFT.Common.Shared;

namespace HQSOFT.Common.HQTasks
{
    [RemoteService(Name = "Common")]
    [Area("common")]
    [ControllerName("HQTask")]
    [Route("api/common/h-qTasks")]
    public class HQTaskController : AbpController, IHQTasksAppService
    {
        private readonly IHQTasksAppService _hQTasksAppService;

        public HQTaskController(IHQTasksAppService hQTasksAppService)
        {
            _hQTasksAppService = hQTasksAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<HQTaskDto>> GetListAsync(GetHQTasksInput input)
        {
            return _hQTasksAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<HQTaskDto> GetAsync(Guid id)
        {
            return _hQTasksAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<HQTaskDto> CreateAsync(HQTaskCreateDto input)
        {
            return _hQTasksAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<HQTaskDto> UpdateAsync(Guid id, HQTaskUpdateDto input)
        {
            return _hQTasksAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _hQTasksAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(HQTaskExcelDownloadDto input)
        {
            return _hQTasksAppService.GetListAsExcelFileAsync(input);
        }

        [HttpGet]
        [Route("download-token")]
        public Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            return _hQTasksAppService.GetDownloadTokenAsync();
        }
    }
}