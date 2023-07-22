using HQSOFT.Configuration.HQAssigments;
using Volo.Abp.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using HQSOFT.Common.MongoDB;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using MongoDB.Driver.Linq;
using MongoDB.Driver;

namespace HQSOFT.Common.HQAssigneds
{
    public class MongoHQAssignedRepository : MongoDbRepository<CommonMongoDbContext, HQAssigned, Guid>, IHQAssignedRepository
    {
        public MongoHQAssignedRepository(IMongoDbContextProvider<CommonMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<HQAssignedWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var hQAssigned = await (await GetMongoQueryableAsync(cancellationToken))
                .FirstOrDefaultAsync(e => e.Id == id, GetCancellationToken(cancellationToken));

            var identityUserIds = hQAssigned.IdentityUsers.Select(x => x.IdentityUserId).ToList();
            var identityUsers = await (await GetDbContextAsync(cancellationToken)).Database.GetCollection<IdentityUser>("AbpUsers").AsQueryable().Where(e => identityUserIds.Contains(e.Id)).ToListAsync(cancellationToken: cancellationToken);

            return new HQAssignedWithNavigationProperties
            {
                HQAssigned = hQAssigned,
                IdentityUsers = identityUsers,

            };
        }

        public async Task<List<HQAssignedWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
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
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, iDParent, completebyMin, completebyMax, priority, comment, identityUserId);
            var hQAssigneds = await query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? HQAssignedConsts.GetDefaultSorting(false) : sorting.Split('.').Last())
                .As<IMongoQueryable<HQAssigned>>()
                .PageBy<HQAssigned, IMongoQueryable<HQAssigned>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));

            var dbContext = await GetDbContextAsync(cancellationToken);
            return hQAssigneds.Select(s => new HQAssignedWithNavigationProperties
            {
                HQAssigned = s,
                IdentityUsers = new List<IdentityUser>(),

            }).ToList();
        }

        public async Task<List<HQAssigned>> GetListAsync(
            string filterText = null,
            string iDParent = null,
            DateTime? completebyMin = null,
            DateTime? completebyMax = null,
            PriorityAssign? priority = null,
            string comment = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, iDParent, completebyMin, completebyMax, priority, comment);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? HQAssignedConsts.GetDefaultSorting(false) : sorting);
            return await query.As<IMongoQueryable<HQAssigned>>()
                .PageBy<HQAssigned, IMongoQueryable<HQAssigned>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<long> GetCountAsync(
           string filterText = null,
           string iDParent = null,
           DateTime? completebyMin = null,
           DateTime? completebyMax = null,
           PriorityAssign? priority = null,
           string comment = null,
           Guid? identityUserId = null,
           CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, iDParent, completebyMin, completebyMax, priority, comment, identityUserId);
            return await query.As<IMongoQueryable<HQAssigned>>().LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<HQAssigned> ApplyFilter(
            IQueryable<HQAssigned> query,
            string filterText,
            string iDParent = null,
            DateTime? completebyMin = null,
            DateTime? completebyMax = null,
            PriorityAssign? priority = null,
            string comment = null,
            Guid? identityUserId = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.IDParent.Contains(filterText) || e.Comment.Contains(filterText))
                    .WhereIf(!string.IsNullOrWhiteSpace(iDParent), e => e.IDParent.Contains(iDParent))
                    .WhereIf(completebyMin.HasValue, e => e.Completeby >= completebyMin.Value)
                    .WhereIf(completebyMax.HasValue, e => e.Completeby <= completebyMax.Value)
                    .WhereIf(priority.HasValue, e => e.Priority == priority)
                    .WhereIf(!string.IsNullOrWhiteSpace(comment), e => e.Comment.Contains(comment))
                    .WhereIf(identityUserId != null && identityUserId != Guid.Empty, e => e.IdentityUsers.Any(x => x.IdentityUserId == identityUserId));
        }
    }
}