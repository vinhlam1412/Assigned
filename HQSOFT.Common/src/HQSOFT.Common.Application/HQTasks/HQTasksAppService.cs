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
using HQSOFT.Common.HQTasks;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using HQSOFT.Common.Shared;

namespace HQSOFT.Common.HQTasks
{

    [Authorize(CommonPermissions.HQTasks.Default)]
    public class HQTasksAppService : ApplicationService, IHQTasksAppService
    {
        private readonly IDistributedCache<HQTaskExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        private readonly IHQTaskRepository _hQTaskRepository;
        private readonly HQTaskManager _hQTaskManager;

        public HQTasksAppService(IHQTaskRepository hQTaskRepository, HQTaskManager hQTaskManager, IDistributedCache<HQTaskExcelDownloadTokenCacheItem, string> excelDownloadTokenCache)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _hQTaskRepository = hQTaskRepository;
            _hQTaskManager = hQTaskManager;
        }

        public virtual async Task<PagedResultDto<HQTaskDto>> GetListAsync(GetHQTasksInput input)
        {
            var totalCount = await _hQTaskRepository.GetCountAsync(input.FilterText, input.Subject, input.Project, input.Status, input.Priority, input.ExpectedStartDateMin, input.ExpectedStartDateMax, input.ExpectedEndDateMin, input.ExpectedEndDateMax, input.ExpectedTimeMin, input.ExpectedTimeMax, input.ProcessMin, input.ProcessMax);
            var items = await _hQTaskRepository.GetListAsync(input.FilterText, input.Subject, input.Project, input.Status, input.Priority, input.ExpectedStartDateMin, input.ExpectedStartDateMax, input.ExpectedEndDateMin, input.ExpectedEndDateMax, input.ExpectedTimeMin, input.ExpectedTimeMax, input.ProcessMin, input.ProcessMax, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<HQTaskDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<HQTask>, List<HQTaskDto>>(items)
            };
        }

        public virtual async Task<HQTaskDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<HQTask, HQTaskDto>(await _hQTaskRepository.GetAsync(id));
        }

        [Authorize(CommonPermissions.HQTasks.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _hQTaskRepository.DeleteAsync(id);
        }

        [Authorize(CommonPermissions.HQTasks.Create)]
        public virtual async Task<HQTaskDto> CreateAsync(HQTaskCreateDto input)
        {

            var hQTask = await _hQTaskManager.CreateAsync(
            input.Subject, input.Project, input.Status, input.Priority, input.ExpectedStartDate, input.ExpectedEndDate, input.ExpectedTime, input.Process
            );

            return ObjectMapper.Map<HQTask, HQTaskDto>(hQTask);
        }

        [Authorize(CommonPermissions.HQTasks.Edit)]
        public virtual async Task<HQTaskDto> UpdateAsync(Guid id, HQTaskUpdateDto input)
        {

            var hQTask = await _hQTaskManager.UpdateAsync(
            id,
            input.Subject, input.Project, input.Status, input.Priority, input.ExpectedStartDate, input.ExpectedEndDate, input.ExpectedTime, input.Process, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<HQTask, HQTaskDto>(hQTask);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(HQTaskExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _hQTaskRepository.GetListAsync(input.FilterText, input.Subject, input.Project, input.Status, input.Priority, input.ExpectedStartDateMin, input.ExpectedStartDateMax, input.ExpectedEndDateMin, input.ExpectedEndDateMax, input.ExpectedTimeMin, input.ExpectedTimeMax, input.ProcessMin, input.ProcessMax);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<HQTask>, List<HQTaskExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "HQTasks.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new HQTaskExcelDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new DownloadTokenResultDto
            {
                Token = token
            };
        }
    }
}