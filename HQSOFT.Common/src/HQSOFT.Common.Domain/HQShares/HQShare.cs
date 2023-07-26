using Volo.Abp.Identity;
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
        public Guid? IdentityUserId { get; set; }

        public HQShare()
        {

        }

        public HQShare(Guid id, Guid? identityUserId, string iDParent, bool canRead, bool canWrite, bool canSubmit, bool canShare)
        {

            Id = id;
            IDParent = iDParent;
            CanRead = canRead;
            CanWrite = canWrite;
            CanSubmit = canSubmit;
            CanShare = canShare;
            IdentityUserId = identityUserId;
        }

    }
}