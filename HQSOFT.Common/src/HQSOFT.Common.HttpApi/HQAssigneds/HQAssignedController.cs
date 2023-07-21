using HQSOFT.Common.Shared;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using HQSOFT.Common.HQAssigneds;
using Volo.Abp.Content;
using HQSOFT.Common.Shared;

namespace HQSOFT.Common.HQAssigneds
{
    [RemoteService(Name = "Common")]
    [Area("common")]
    [ControllerName("HQAssigned")]
    [Route("api/common/h-qAssigneds")]
    public class HQAssignedController : AbpController, IHQAssignedsAppService
    {
        private readonly IHQAssignedsAppService _hQAssignedsAppService;

        public HQAssignedController(IHQAssignedsAppService hQAssignedsAppService)
        {
            _hQAssignedsAppService = hQAssignedsAppService;
        }

        [HttpGet]
        public Task<PagedResultDto<HQAssignedWithNavigationPropertiesDto>> GetListAsync(GetHQAssignedsInput input)
        {
            return _hQAssignedsAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("with-navigation-properties/{id}")]
        public Task<HQAssignedWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return _hQAssignedsAppService.GetWithNavigationPropertiesAsync(id);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<HQAssignedDto> GetAsync(Guid id)
        {
            return _hQAssignedsAppService.GetAsync(id);
        }

        [HttpGet]
        [Route("identity-user-lookup")]
        public Task<PagedResultDto<LookupDto<Guid>>> GetIdentityUserLookupAsync(LookupRequestDto input)
        {
            return _hQAssignedsAppService.GetIdentityUserLookupAsync(input);
        }

        [HttpPost]
        public virtual Task<HQAssignedDto> CreateAsync(HQAssignedCreateDto input)
        {
            return _hQAssignedsAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<HQAssignedDto> UpdateAsync(Guid id, HQAssignedUpdateDto input)
        {
            return _hQAssignedsAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _hQAssignedsAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(HQAssignedExcelDownloadDto input)
        {
            return _hQAssignedsAppService.GetListAsExcelFileAsync(input);
        }

        [HttpGet]
        [Route("download-token")]
        public Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            return _hQAssignedsAppService.GetDownloadTokenAsync();
        }

        [HttpGet]
        [Route("getParentId/{id}")]
        public virtual Task<HQAssignedDto> GetParentAsync(Guid id)
        {
            return _hQAssignedsAppService.GetParentAsync(id);
        }
    }
}