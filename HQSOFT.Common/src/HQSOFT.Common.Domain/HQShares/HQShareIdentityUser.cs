using System;
using Volo.Abp.Domain.Entities;

namespace HQSOFT.Common.HQShares
{
    public class HQShareIdentityUser : Entity
    {

        public Guid HQShareId { get; protected set; }

        public Guid IdentityUserId { get; protected set; }

        private HQShareIdentityUser()
        {

        }

        public HQShareIdentityUser(Guid hQShareId, Guid identityUserId)
        {
            HQShareId = hQShareId;
            IdentityUserId = identityUserId;
        }

        public override object[] GetKeys()
        {
            return new object[]
                {
                    HQShareId,
                    IdentityUserId
                };
        }
    }
}