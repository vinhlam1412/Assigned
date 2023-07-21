using System;
using Volo.Abp.Domain.Entities;

namespace HQSOFT.Common.HQAssigneds
{
    public class HQAssignedIdentityUser : Entity
    {

        public Guid HQAssignedId { get; protected set; }

        public Guid IdentityUserId { get; protected set; }

        private HQAssignedIdentityUser()
        {

        }

        public HQAssignedIdentityUser(Guid hQAssignedId, Guid identityUserId)
        {
            HQAssignedId = hQAssignedId;
            IdentityUserId = identityUserId;
        }

        public override object[] GetKeys()
        {
            return new object[]
                {
                    HQAssignedId,
                    IdentityUserId
                };
        }
    }
}