using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using HQSOFT.Common.HQNotifications;
using Volo.Abp.Content;
using HQSOFT.Common.Shared;
using System.Collections.Generic;

namespace HQSOFT.Common.HQNotifications
{
    [RemoteService(Name = "Common")]
    [Area("common")]
    [ControllerName("HQNotification")]
    [Route("api/common/h-qNotifications")]
    public class HQNotificationController : AbpController, IHQNotificationsAppService
    {
        private readonly IHQNotificationsAppService _hQNotificationsAppService;

        public HQNotificationController(IHQNotificationsAppService hQNotificationsAppService)
        {
            _hQNotificationsAppService = hQNotificationsAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<HQNotificationDto>> GetListAsync(GetHQNotificationsInput input)
        {
            return _hQNotificationsAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<HQNotificationDto> GetAsync(Guid id)
        {
            return _hQNotificationsAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<HQNotificationDto> CreateAsync(HQNotificationCreateDto input)
        {
            return _hQNotificationsAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<HQNotificationDto> UpdateAsync(Guid id, HQNotificationUpdateDto input)
        {
            return _hQNotificationsAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _hQNotificationsAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(HQNotificationExcelDownloadDto input)
        {
            return _hQNotificationsAppService.GetListAsExcelFileAsync(input);
        }

        [HttpGet]
        [Route("download-token")]
        public Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            return _hQNotificationsAppService.GetDownloadTokenAsync();
        }

        [HttpGet]
        [Route("getall")]
        public virtual Task<List<HQNotificationDto>> GetAllAsync()
        {
            return _hQNotificationsAppService.GetAllAsync();
        }
    }
}