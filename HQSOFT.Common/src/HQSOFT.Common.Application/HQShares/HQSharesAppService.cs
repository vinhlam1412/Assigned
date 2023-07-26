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
using HQSOFT.Common.HQShares;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using HQSOFT.Common.Shared;

namespace HQSOFT.Common.HQShares
{

    [Authorize(CommonPermissions.HQShares.Default)]
    public class HQSharesAppService : ApplicationService, IHQSharesAppService
    {
        private readonly IDistributedCache<HQShareExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        private readonly IHQShareRepository _hQShareRepository;
        private readonly HQShareManager _hQShareManager;
        private readonly IRepository<IdentityUser, Guid> _identityUserRepository;

        public HQSharesAppService(IHQShareRepository hQShareRepository, HQShareManager hQShareManager, IDistributedCache<HQShareExcelDownloadTokenCacheItem, string> excelDownloadTokenCache, IRepository<IdentityUser, Guid> identityUserRepository)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _hQShareRepository = hQShareRepository;
            _hQShareManager = hQShareManager; _identityUserRepository = identityUserRepository;
        }

        public virtual async Task<PagedResultDto<HQShareWithNavigationPropertiesDto>> GetListAsync(GetHQSharesInput input)
        {
            var totalCount = await _hQShareRepository.GetCountAsync(input.FilterText, input.IDParent, input.CanRead, input.CanWrite, input.CanSubmit, input.CanShare, input.IdentityUserId);
            var items = await _hQShareRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.IDParent, input.CanRead, input.CanWrite, input.CanSubmit, input.CanShare, input.IdentityUserId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<HQShareWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<HQShareWithNavigationProperties>, List<HQShareWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<HQShareWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return ObjectMapper.Map<HQShareWithNavigationProperties, HQShareWithNavigationPropertiesDto>
                (await _hQShareRepository.GetWithNavigationPropertiesAsync(id));
        }

        public virtual async Task<HQShareDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<HQShare, HQShareDto>(await _hQShareRepository.GetAsync(id));
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

        [Authorize(CommonPermissions.HQShares.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _hQShareRepository.DeleteAsync(id);
        }

        [Authorize(CommonPermissions.HQShares.Create)]
        public virtual async Task<HQShareDto> CreateAsync(HQShareCreateDto input)
        {

            var hQShare = await _hQShareManager.CreateAsync(
            input.IdentityUserId, input.IDParent, input.CanRead, input.CanWrite, input.CanSubmit, input.CanShare
            );

            return ObjectMapper.Map<HQShare, HQShareDto>(hQShare);
        }

        [Authorize(CommonPermissions.HQShares.Edit)]
        public virtual async Task<HQShareDto> UpdateAsync(Guid id, HQShareUpdateDto input)
        {

            var hQShare = await _hQShareManager.UpdateAsync(
            id,
            input.IdentityUserId, input.IDParent, input.CanRead, input.CanWrite, input.CanSubmit, input.CanShare, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<HQShare, HQShareDto>(hQShare);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(HQShareExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var hQShares = await _hQShareRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.IDParent, input.CanRead, input.CanWrite, input.CanSubmit, input.CanShare);
            var items = hQShares.Select(item => new
            {
                IDParent = item.HQShare.IDParent,
                CanRead = item.HQShare.CanRead,
                CanWrite = item.HQShare.CanWrite,
                CanSubmit = item.HQShare.CanSubmit,
                CanShare = item.HQShare.CanShare,

                IdentityUser = item.IdentityUser?.Email,

            });

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(items);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "HQShares.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new HQShareExcelDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new DownloadTokenResultDto
            {
                Token = token
            };
        }

        public async Task<List<HQShareDto>> GetParentAsync(string id)
        {
            // Use the FindAsync method of _hQShareRepository to retrieve the HQShare entity with an IDParent that matches the given id.
           var entity = await _hQShareRepository.GetListAsync(x => x.IDParent.ToLower() == id.ToLower());

            // Use the ObjectMapper to map the retrieved entity to a HQAssignedDto object.
           List<HQShareDto> dto = ObjectMapper.Map<List<HQShare>, List<HQShareDto>>(entity);

            // Return the mapped HQShareDto object.
            return dto;
        }
    }
}