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

        public HQShareManager(IHQShareRepository hQShareRepository)
        {
            _hQShareRepository = hQShareRepository;
        }

        public async Task<HQShare> CreateAsync(
        Guid? identityUserId, string iDParent, bool canRead, bool canWrite, bool canSubmit, bool canShare)
        {

            var hQShare = new HQShare(
             GuidGenerator.Create(),
             identityUserId, iDParent, canRead, canWrite, canSubmit, canShare
             );

            return await _hQShareRepository.InsertAsync(hQShare);
        }

        public async Task<HQShare> UpdateAsync(
            Guid id,
            Guid? identityUserId, string iDParent, bool canRead, bool canWrite, bool canSubmit, bool canShare, [CanBeNull] string concurrencyStamp = null
        )
        {

            var hQShare = await _hQShareRepository.GetAsync(id);

            hQShare.IdentityUserId = identityUserId;
            hQShare.IDParent = iDParent;
            hQShare.CanRead = canRead;
            hQShare.CanWrite = canWrite;
            hQShare.CanSubmit = canSubmit;
            hQShare.CanShare = canShare;

            hQShare.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _hQShareRepository.UpdateAsync(hQShare);
        }

    }
}