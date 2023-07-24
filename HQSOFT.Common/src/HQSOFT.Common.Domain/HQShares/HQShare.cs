using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace HQSOFT.Common.HQShares
{
    public class HQShare : FullAuditedAggregateRoot<Guid>
    {
        [CanBeNull]
        public virtual string? IDParent { get; set; }

        public virtual bool CanRead { get; set; }

        public virtual bool CanWrite { get; set; }

        public virtual bool CanSubmit { get; set; }

        public virtual bool CanShare { get; set; }

        public ICollection<HQShareIdentityUser> IdentityUsers { get; private set; }

        public HQShare()
        {

        }

        public HQShare(Guid id, string iDParent, bool canRead, bool canWrite, bool canSubmit, bool canShare)
        {

            Id = id;
            IDParent = iDParent;
            CanRead = canRead;
            CanWrite = canWrite;
            CanSubmit = canSubmit;
            CanShare = canShare;
            IdentityUsers = new Collection<HQShareIdentityUser>();
        }
        public void AddIdentityUser(Guid identityUserId)
        {
            Check.NotNull(identityUserId, nameof(identityUserId));

            if (IsInIdentityUsers(identityUserId))
            {
                return;
            }

            IdentityUsers.Add(new HQShareIdentityUser(Id, identityUserId));
        }

        public void RemoveIdentityUser(Guid identityUserId)
        {
            Check.NotNull(identityUserId, nameof(identityUserId));

            if (!IsInIdentityUsers(identityUserId))
            {
                return;
            }

            IdentityUsers.RemoveAll(x => x.IdentityUserId == identityUserId);
        }

        public void RemoveAllIdentityUsersExceptGivenIds(List<Guid> identityUserIds)
        {
            Check.NotNullOrEmpty(identityUserIds, nameof(identityUserIds));

            IdentityUsers.RemoveAll(x => !identityUserIds.Contains(x.IdentityUserId));
        }

        public void RemoveAllIdentityUsers()
        {
            IdentityUsers.RemoveAll(x => x.HQShareId == Id);
        }

        private bool IsInIdentityUsers(Guid identityUserId)
        {
            return IdentityUsers.Any(x => x.IdentityUserId == identityUserId);
        }
    }
}