using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using HQSOFT.Common.Permissions;
using HQSOFT.Common.HQNotifications;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using HQSOFT.Common.Shared;
using Volo.Abp.ObjectMapping;

namespace HQSOFT.Common.HQNotifications
{

    [Authorize(CommonPermissions.HQNotifications.Default)]
    public class HQNotificationsAppService : ApplicationService, IHQNotificationsAppService
    {
        private readonly IDistributedCache<HQNotificationExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        private readonly IHQNotificationRepository _hQNotificationRepository;
        private readonly HQNotificationManager _hQNotificationManager;

        public HQNotificationsAppService(IHQNotificationRepository hQNotificationRepository, HQNotificationManager hQNotificationManager, IDistributedCache<HQNotificationExcelDownloadTokenCacheItem, string> excelDownloadTokenCache)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _hQNotificationRepository = hQNotificationRepository;
            _hQNotificationManager = hQNotificationManager;
        }

        public virtual async Task<PagedResultDto<HQNotificationDto>> GetListAsync(GetHQNotificationsInput input)
        {
            var totalCount = await _hQNotificationRepository.GetCountAsync(input.FilterText, input.IDParent, input.ToUser, input.FromUser, input.NotiTitle, input.NotiBody, input.Type, input.isRead);
            var items = await _hQNotificationRepository.GetListAsync(input.FilterText, input.IDParent, input.ToUser, input.FromUser, input.NotiTitle, input.NotiBody, input.Type, input.isRead, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<HQNotificationDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<HQNotification>, List<HQNotificationDto>>(items)
            };
        }

        public virtual async Task<HQNotificationDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<HQNotification, HQNotificationDto>(await _hQNotificationRepository.GetAsync(id));
        }

        [Authorize(CommonPermissions.HQNotifications.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _hQNotificationRepository.DeleteAsync(id);
        }

        [Authorize(CommonPermissions.HQNotifications.Create)]
        public virtual async Task<HQNotificationDto> CreateAsync(HQNotificationCreateDto input)
        {

            var hQNotification = await _hQNotificationManager.CreateAsync(
            input.IDParent, input.ToUser, input.FromUser, input.NotiTitle, input.NotiBody, input.Type, input.isRead
            );

            return ObjectMapper.Map<HQNotification, HQNotificationDto>(hQNotification);
        }

        [Authorize(CommonPermissions.HQNotifications.Edit)]
        public virtual async Task<HQNotificationDto> UpdateAsync(Guid id, HQNotificationUpdateDto input)
        {

            var hQNotification = await _hQNotificationManager.UpdateAsync(
            id,
            input.IDParent, input.ToUser, input.FromUser, input.NotiTitle, input.NotiBody, input.Type, input.isRead, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<HQNotification, HQNotificationDto>(hQNotification);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(HQNotificationExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _hQNotificationRepository.GetListAsync(input.FilterText, input.IDParent, input.ToUser, input.FromUser, input.NotiTitle, input.NotiBody, input.Type, input.isRead);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<HQNotification>, List<HQNotificationExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "HQNotifications.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new HQNotificationExcelDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new DownloadTokenResultDto
            {
                Token = token
            };
        }

        public async Task<List<HQNotificationDto>> GetAllAsync()
        {
            var listNotifies = await _hQNotificationRepository.GetListAsync();
            return ObjectMapper.Map<List<HQNotification>,List<HQNotificationDto>>(listNotifies);
        }
    }
}