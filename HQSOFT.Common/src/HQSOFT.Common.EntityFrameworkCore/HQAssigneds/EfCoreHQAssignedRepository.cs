using HQSOFT.Configuration.HQAssigments;
using Volo.Abp.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using HQSOFT.Common.EntityFrameworkCore;

namespace HQSOFT.Common.HQAssigneds
{
    public class EfCoreHQAssignedRepository : EfCoreRepository<CommonDbContext, HQAssigned, Guid>, IHQAssignedRepository
    {
        public EfCoreHQAssignedRepository(IDbContextProvider<CommonDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<HQAssignedWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync()).Where(b => b.Id == id).Include(x => x.IdentityUsers)
                .Select(hQAssigned => new HQAssignedWithNavigationProperties
                {
                    HQAssigned = hQAssigned,
                    IdentityUsers = (from hQAssignedIdentityUsers in hQAssigned.IdentityUsers
                                     join _identityUser in dbContext.Set<IdentityUser>() on hQAssignedIdentityUsers.IdentityUserId equals _identityUser.Id
                                     select _identityUser).ToList()
                }).FirstOrDefault();
        }

        public async Task<List<HQAssignedWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string filterText = null,
            Guid? iDParent = null,
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
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, iDParent, completebyMin, completebyMax, priority, comment, identityUserId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? HQAssignedConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual async Task<IQueryable<HQAssignedWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
        {
            return from hQAssigned in (await GetDbSetAsync())

                   select new HQAssignedWithNavigationProperties
                   {
                       HQAssigned = hQAssigned,
                       IdentityUsers = new List<IdentityUser>()
                   };
        }

        protected virtual IQueryable<HQAssignedWithNavigationProperties> ApplyFilter(
            IQueryable<HQAssignedWithNavigationProperties> query,
            string filterText,
            Guid? iDParent = null,
            DateTime? completebyMin = null,
            DateTime? completebyMax = null,
            PriorityAssign? priority = null,
            string comment = null,
            Guid? identityUserId = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.HQAssigned.Comment.Contains(filterText))
                    .WhereIf(iDParent.HasValue, e => e.HQAssigned.IDParent == iDParent)
                    .WhereIf(completebyMin.HasValue, e => e.HQAssigned.Completeby >= completebyMin.Value)
                    .WhereIf(completebyMax.HasValue, e => e.HQAssigned.Completeby <= completebyMax.Value)
                    .WhereIf(priority.HasValue, e => e.HQAssigned.Priority == priority)
                    .WhereIf(!string.IsNullOrWhiteSpace(comment), e => e.HQAssigned.Comment.Contains(comment))
                    .WhereIf(identityUserId != null && identityUserId != Guid.Empty, e => e.HQAssigned.IdentityUsers.Any(x => x.IdentityUserId == identityUserId));
        }

        public async Task<List<HQAssigned>> GetListAsync(
            string filterText = null,
            Guid? iDParent = null,
            DateTime? completebyMin = null,
            DateTime? completebyMax = null,
            PriorityAssign? priority = null,
            string comment = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, iDParent, completebyMin, completebyMax, priority, comment);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? HQAssignedConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(
            string filterText = null,
            Guid? iDParent = null,
            DateTime? completebyMin = null,
            DateTime? completebyMax = null,
            PriorityAssign? priority = null,
            string comment = null,
            Guid? identityUserId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, iDParent, completebyMin, completebyMax, priority, comment, identityUserId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<HQAssigned> ApplyFilter(
            IQueryable<HQAssigned> query,
            string filterText,
            Guid? iDParent = null,
            DateTime? completebyMin = null,
            DateTime? completebyMax = null,
            PriorityAssign? priority = null,
            string comment = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Comment.Contains(filterText))
                    .WhereIf(iDParent.HasValue, e => e.IDParent == iDParent)
                    .WhereIf(completebyMin.HasValue, e => e.Completeby >= completebyMin.Value)
                    .WhereIf(completebyMax.HasValue, e => e.Completeby <= completebyMax.Value)
                    .WhereIf(priority.HasValue, e => e.Priority == priority)
                    .WhereIf(!string.IsNullOrWhiteSpace(comment), e => e.Comment.Contains(comment));
        }
    }
}