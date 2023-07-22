using HQSOFT.Common.Shared;
using Volo.Abp.Identity;
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
using HQSOFT.Common.HQAssigneds;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using HQSOFT.Common.Shared;

namespace HQSOFT.Common.HQAssigneds
{

    [Authorize(CommonPermissions.HQAssigneds.Default)]
    public class HQAssignedsAppService : ApplicationService, IHQAssignedsAppService
    {
        private readonly IDistributedCache<HQAssignedExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        private readonly IHQAssignedRepository _hQAssignedRepository;
        private readonly HQAssignedManager _hQAssignedManager;
        private readonly IRepository<IdentityUser, Guid> _identityUserRepository;

        public HQAssignedsAppService(IHQAssignedRepository hQAssignedRepository, HQAssignedManager hQAssignedManager, IDistributedCache<HQAssignedExcelDownloadTokenCacheItem, string> excelDownloadTokenCache, IRepository<IdentityUser, Guid> identityUserRepository)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _hQAssignedRepository = hQAssignedRepository;
            _hQAssignedManager = hQAssignedManager; _identityUserRepository = identityUserRepository;
        }

        public virtual async Task<PagedResultDto<HQAssignedWithNavigationPropertiesDto>> GetListAsync(GetHQAssignedsInput input)
        {
            var totalCount = await _hQAssignedRepository.GetCountAsync(input.FilterText, input.IDParent, input.CompletebyMin, input.CompletebyMax, input.Priority, input.Comment, input.IdentityUserId);
            var items = await _hQAssignedRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.IDParent, input.CompletebyMin, input.CompletebyMax, input.Priority, input.Comment, input.IdentityUserId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<HQAssignedWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<HQAssignedWithNavigationProperties>, List<HQAssignedWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<HQAssignedWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return ObjectMapper.Map<HQAssignedWithNavigationProperties, HQAssignedWithNavigationPropertiesDto>
                (await _hQAssignedRepository.GetWithNavigationPropertiesAsync(id));
        }

        public virtual async Task<HQAssignedDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<HQAssigned, HQAssignedDto>(await _hQAssignedRepository.GetAsync(id));
        }

        public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetIdentityUserLookupAsync(LookupRequestDto input)
        {
            var query = (await _identityUserRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    x => x.Email != null &&
                         x.Email.Contains(input.Filter));

            var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<IdentityUser>();
            var totalCount = query.Count();
            return new PagedResultDto<LookupDto<Guid>>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<IdentityUser>, List<LookupDto<Guid>>>(lookupData)
            };
        }

        [Authorize(CommonPermissions.HQAssigneds.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _hQAssignedRepository.DeleteAsync(id);
        }

        [Authorize(CommonPermissions.HQAssigneds.Create)]
        public virtual async Task<HQAssignedDto> CreateAsync(HQAssignedCreateDto input)
        {

            var hQAssigned = await _hQAssignedManager.CreateAsync(
            input.IdentityUserIds, input.IDParent, input.Completeby, input.Priority, input.Comment
            );

            return ObjectMapper.Map<HQAssigned, HQAssignedDto>(hQAssigned);
        }

        [Authorize(CommonPermissions.HQAssigneds.Edit)]
        public virtual async Task<HQAssignedDto> UpdateAsync(Guid id, HQAssignedUpdateDto input)
        {

            var hQAssigned = await _hQAssignedManager.UpdateAsync(
            id,
            input.IdentityUserIds, input.IDParent, input.Completeby, input.Priority, input.Comment, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<HQAssigned, HQAssignedDto>(hQAssigned);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(HQAssignedExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var hQAssigneds = await _hQAssignedRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.IDParent, input.CompletebyMin, input.CompletebyMax, input.Priority, input.Comment);
            var items = hQAssigneds.Select(item => new
            {
                IDParent = item.HQAssigned.IDParent,
                Completeby = item.HQAssigned.Completeby,
                Priority = item.HQAssigned.Priority,
                Comment = item.HQAssigned.Comment,

            });

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(items);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "HQAssigneds.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new HQAssignedExcelDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new DownloadTokenResultDto
            {
                Token = token
            };
        }
        public virtual async Task<HQAssignedDto> GetParentAsync(string id)
        {
            // Use the FindAsync method of _hQAssignedRepository to retrieve the HQAssigned entity with an IDParent that matches the given id.
            HQAssigned entity = await _hQAssignedRepository.FindAsync(x => x.IDParent.ToLower() == id.ToLower());

            // Use the ObjectMapper to map the retrieved entity to a HQAssignedDto object.
            HQAssignedDto dto = ObjectMapper.Map<HQAssigned, HQAssignedDto>(entity);

            // Return the mapped HQAssignedDto object.
            return dto;
        }
    }
}