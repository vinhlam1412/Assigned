using HQSOFT.Common.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
namespace HQSOFT.Common.HQShares
{
    public interface IHQSharesAppService : IApplicationService
    {
        Task<PagedResultDto<HQShareWithNavigationPropertiesDto>> GetListAsync(GetHQSharesInput input);

        Task<HQShareWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<HQShareDto> GetAsync(Guid id);

        Task<PagedResultDto<LookupDto<Guid>>> GetIdentityUserLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<HQShareDto> CreateAsync(HQShareCreateDto input);

        Task<HQShareDto> UpdateAsync(Guid id, HQShareUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(HQShareExcelDownloadDto input);

        Task<DownloadTokenResultDto> GetDownloadTokenAsync();

        Task<List<HQShareDto>> GetParentAsync(string id);
    }
}