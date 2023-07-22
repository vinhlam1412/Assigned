using HQSOFT.Configuration.HQAssigments;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace HQSOFT.Common.HQAssigneds
{
    public interface IHQAssignedRepository : IRepository<HQAssigned, Guid>
    {
        Task<HQAssignedWithNavigationProperties> GetWithNavigationPropertiesAsync(
    Guid id,
    CancellationToken cancellationToken = default
);

        Task<List<HQAssignedWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string filterText = null,
            string iDParent = null,
            DateTime? completebyMin = null,
            DateTime? completebyMax = null,
            PriorityAssign? priority = null,
            string comment = null,
            Guid? identityUserId = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<List<HQAssigned>> GetListAsync(
                    string filterText = null,
                    string iDParent = null,
                    DateTime? completebyMin = null,
                    DateTime? completebyMax = null,
                    PriorityAssign? priority = null,
                    string comment = null,
                    string sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string filterText = null,
            string iDParent = null,
            DateTime? completebyMin = null,
            DateTime? completebyMax = null,
            PriorityAssign? priority = null,
            string comment = null,
            Guid? identityUserId = null,
            CancellationToken cancellationToken = default);
    }
}