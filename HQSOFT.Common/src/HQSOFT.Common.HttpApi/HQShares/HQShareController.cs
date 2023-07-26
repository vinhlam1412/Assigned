using HQSOFT.Common.Shared;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using HQSOFT.Common.HQShares;
using Volo.Abp.Content;
using HQSOFT.Common.Shared;
using System.Collections.Generic;

namespace HQSOFT.Common.HQShares
{
    [RemoteService(Name = "Common")]
    [Area("common")]
    [ControllerName("HQShare")]
    [Route("api/common/h-qShares")]
    public class HQShareController : AbpController, IHQSharesAppService
    {
        private readonly IHQSharesAppService _hQSharesAppService;

        public HQShareController(IHQSharesAppService hQSharesAppService)
        {
            _hQSharesAppService = hQSharesAppService;
        }

        [HttpGet]
        public Task<PagedResultDto<HQShareWithNavigationPropertiesDto>> GetListAsync(GetHQSharesInput input)
        {
            return _hQSharesAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("with-navigation-properties/{id}")]
        public Task<HQShareWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return _hQSharesAppService.GetWithNavigationPropertiesAsync(id);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<HQShareDto> GetAsync(Guid id)
        {
            return _hQSharesAppService.GetAsync(id);
        }

        [HttpGet]
        [Route("identity-user-lookup")]
        public Task<PagedResultDto<LookupDto<Guid>>> GetIdentityUserLookupAsync(LookupRequestDto input)
        {
            return _hQSharesAppService.GetIdentityUserLookupAsync(input);
        }

        [HttpPost]
        public virtual Task<HQShareDto> CreateAsync(HQShareCreateDto input)
        {
            return _hQSharesAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<HQShareDto> UpdateAsync(Guid id, HQShareUpdateDto input)
        {
            return _hQSharesAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _hQSharesAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(HQShareExcelDownloadDto input)
        {
            return _hQSharesAppService.GetListAsExcelFileAsync(input);
        }

        [HttpGet]
        [Route("download-token")]
        public Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            return _hQSharesAppService.GetDownloadTokenAsync();
        }

        [HttpGet]
        [Route("getShare-parentId/{id}")]
        public Task<List<HQShareDto>> GetParentAsync(string id)
        {
            return _hQSharesAppService.GetParentAsync(id);
        }
    }
}