using HQSOFT.Common.Shared;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using HQSOFT.Common.Shared;

namespace HQSOFT.Common.HQAssigneds
{
    public interface IHQAssignedsAppService : IApplicationService
    {
        Task<PagedResultDto<HQAssignedWithNavigationPropertiesDto>> GetListAsync(GetHQAssignedsInput input);

        Task<HQAssignedWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<HQAssignedDto> GetAsync(Guid id);

        Task<PagedResultDto<LookupDto<Guid>>> GetIdentityUserLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<HQAssignedDto> CreateAsync(HQAssignedCreateDto input);

        Task<HQAssignedDto> UpdateAsync(Guid id, HQAssignedUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(HQAssignedExcelDownloadDto input);

        Task<DownloadTokenResultDto> GetDownloadTokenAsync();

        Task<HQAssignedDto> GetParentAsync(string id);
    }
}