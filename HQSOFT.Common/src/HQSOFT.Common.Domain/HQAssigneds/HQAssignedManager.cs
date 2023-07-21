using Volo.Abp.Identity;
using HQSOFT.Configuration.HQAssigments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace HQSOFT.Common.HQAssigneds
{
    public class HQAssignedManager : DomainService
    {
        private readonly IHQAssignedRepository _hQAssignedRepository;
        private readonly IRepository<IdentityUser, Guid> _identityUserRepository;

        public HQAssignedManager(IHQAssignedRepository hQAssignedRepository,
        IRepository<IdentityUser, Guid> identityUserRepository)
        {
            _hQAssignedRepository = hQAssignedRepository;
            _identityUserRepository = identityUserRepository;
        }

        public async Task<HQAssigned> CreateAsync(
        List<Guid> identityUserIds,
        Guid iDParent, DateTime completeby, PriorityAssign priority, string comment)
        {
            Check.NotNull(completeby, nameof(completeby));
            Check.NotNull(priority, nameof(priority));

            var hQAssigned = new HQAssigned(
             GuidGenerator.Create(),
             iDParent, completeby, priority, comment
             );

            await SetIdentityUsersAsync(hQAssigned, identityUserIds);

            return await _hQAssignedRepository.InsertAsync(hQAssigned);
        }

        public async Task<HQAssigned> UpdateAsync(
            Guid id,
            List<Guid> identityUserIds,
        Guid iDParent, DateTime completeby, PriorityAssign priority, string comment, [CanBeNull] string concurrencyStamp = null
        )
        {
            Check.NotNull(completeby, nameof(completeby));
            Check.NotNull(priority, nameof(priority));

            var queryable = await _hQAssignedRepository.WithDetailsAsync(x => x.IdentityUsers);
            var query = queryable.Where(x => x.Id == id);

            var hQAssigned = await AsyncExecuter.FirstOrDefaultAsync(query);

            hQAssigned.IDParent = iDParent;
            hQAssigned.Completeby = completeby;
            hQAssigned.Priority = priority;
            hQAssigned.Comment = comment;

            await SetIdentityUsersAsync(hQAssigned, identityUserIds);

            hQAssigned.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _hQAssignedRepository.UpdateAsync(hQAssigned);
        }

        private async Task SetIdentityUsersAsync(HQAssigned hQAssigned, List<Guid> identityUserIds)
        {
            if (identityUserIds == null || !identityUserIds.Any())
            {
                hQAssigned.RemoveAllIdentityUsers();
                return;
            }

            var query = (await _identityUserRepository.GetQueryableAsync())
                .Where(x => identityUserIds.Contains(x.Id))
                .Select(x => x.Id);

            var identityUserIdsInDb = await AsyncExecuter.ToListAsync(query);
            if (!identityUserIdsInDb.Any())
            {
                return;
            }

            hQAssigned.RemoveAllIdentityUsersExceptGivenIds(identityUserIdsInDb);

            foreach (var identityUserId in identityUserIdsInDb)
            {
                hQAssigned.AddIdentityUser(identityUserId);
            }
        }

    }
}