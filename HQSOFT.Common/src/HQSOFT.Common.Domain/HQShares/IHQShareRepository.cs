using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace HQSOFT.Common.HQShares
{
    public interface IHQShareRepository : IRepository<HQShare, Guid>
    {
        Task<HQShareWithNavigationProperties> GetWithNavigationPropertiesAsync(
    Guid id,
    CancellationToken cancellationToken = default
);

        Task<List<HQShareWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string filterText = null,
            string iDParent = null,
            bool? canRead = null,
            bool? canWrite = null,
            bool? canSubmit = null,
            bool? canShare = null,
            Guid? identityUserId = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<List<HQShare>> GetListAsync(
                    string filterText = null,
                    string iDParent = null,
                    bool? canRead = null,
                    bool? canWrite = null,
                    bool? canSubmit = null,
                    bool? canShare = null,
                    string sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string filterText = null,
            string iDParent = null,
            bool? canRead = null,
            bool? canWrite = null,
            bool? canSubmit = null,
            bool? canShare = null,
            Guid? identityUserId = null,
            CancellationToken cancellationToken = default);
    }
}