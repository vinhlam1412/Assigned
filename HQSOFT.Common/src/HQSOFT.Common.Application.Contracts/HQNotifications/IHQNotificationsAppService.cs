using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using HQSOFT.Common.Shared;
using System.Collections.Generic;

namespace HQSOFT.Common.HQNotifications
{
    public interface IHQNotificationsAppService : IApplicationService
    {
        Task<PagedResultDto<HQNotificationDto>> GetListAsync(GetHQNotificationsInput input);


        Task<List<HQNotificationDto>> GetAllAsync();

        Task<HQNotificationDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<HQNotificationDto> CreateAsync(HQNotificationCreateDto input);

        Task<HQNotificationDto> UpdateAsync(Guid id, HQNotificationUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(HQNotificationExcelDownloadDto input);

        Task<DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}