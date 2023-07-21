using HQSOFT.Configuration.HQAssigments;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace HQSOFT.Common.HQAssigneds
{
    public class HQAssigned : FullAuditedAggregateRoot<Guid>
    {
        public virtual Guid IDParent { get; set; }

        public virtual DateTime Completeby { get; set; }

        public virtual PriorityAssign Priority { get; set; }

        [CanBeNull]
        public virtual string? Comment { get; set; }

        public ICollection<HQAssignedIdentityUser> IdentityUsers { get; private set; }

        public HQAssigned()
        {

        }

        public HQAssigned(Guid id, Guid iDParent, DateTime completeby, PriorityAssign priority, string comment)
        {

            Id = id;
            IDParent = iDParent;
            Completeby = completeby;
            Priority = priority;
            Comment = comment;
            IdentityUsers = new Collection<HQAssignedIdentityUser>();
        }
        public void AddIdentityUser(Guid identityUserId)
        {
            Check.NotNull(identityUserId, nameof(identityUserId));

            if (IsInIdentityUsers(identityUserId))
            {
                return;
            }

            IdentityUsers.Add(new HQAssignedIdentityUser(Id, identityUserId));
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
            IdentityUsers.RemoveAll(x => x.HQAssignedId == Id);
        }

        private bool IsInIdentityUsers(Guid identityUserId)
        {
            return IdentityUsers.Any(x => x.IdentityUserId == identityUserId);
        }
    }
}