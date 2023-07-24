using Volo.Abp.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace HQSOFT.Common.HQShares
{
    public class HQShareManager : DomainService
    {
        private readonly IHQShareRepository _hQShareRepository;
        private readonly IRepository<IdentityUser, Guid> _identityUserRepository;

        public HQShareManager(IHQShareRepository hQShareRepository,
        IRepository<IdentityUser, Guid> identityUserRepository)
        {
            _hQShareRepository = hQShareRepository;
            _identityUserRepository = identityUserRepository;
        }

        public async Task<HQShare> CreateAsync(
        List<Guid> identityUserIds,
        string iDParent, bool canRead, bool canWrite, bool canSubmit, bool canShare)
        {

            var hQShare = new HQShare(
             GuidGenerator.Create(),
             iDParent, canRead, canWrite, canSubmit, canShare
             );

            await SetIdentityUsersAsync(hQShare, identityUserIds);

            return await _hQShareRepository.InsertAsync(hQShare);
        }

        public async Task<HQShare> UpdateAsync(
            Guid id,
            List<Guid> identityUserIds,
        string iDParent, bool canRead, bool canWrite, bool canSubmit, bool canShare, [CanBeNull] string concurrencyStamp = null
        )
        {

            var queryable = await _hQShareRepository.WithDetailsAsync(x => x.IdentityUsers);
            var query = queryable.Where(x => x.Id == id);

            var hQShare = await AsyncExecuter.FirstOrDefaultAsync(query);

            hQShare.IDParent = iDParent;
            hQShare.CanRead = canRead;
            hQShare.CanWrite = canWrite;
            hQShare.CanSubmit = canSubmit;
            hQShare.CanShare = canShare;

            await SetIdentityUsersAsync(hQShare, identityUserIds);

            hQShare.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _hQShareRepository.UpdateAsync(hQShare);
        }

        private async Task SetIdentityUsersAsync(HQShare hQShare, List<Guid> identityUserIds)
        {
            if (identityUserIds == null || !identityUserIds.Any())
            {
                hQShare.RemoveAllIdentityUsers();
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

            hQShare.RemoveAllIdentityUsersExceptGivenIds(identityUserIdsInDb);

            foreach (var identityUserId in identityUserIdsInDb)
            {
                hQShare.AddIdentityUser(identityUserId);
            }
        }

    }
}