using HQSOFT.Configuration.HQTasks;
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

namespace HQSOFT.Common.HQTasks
{
    public class MongoHQTaskRepository : MongoDbRepository<CommonMongoDbContext, HQTask, Guid>, IHQTaskRepository
    {
        public MongoHQTaskRepository(IMongoDbContextProvider<CommonMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<HQTask>> GetListAsync(
            string filterText = null,
            string subject = null,
            string project = null,
            StatusTask? status = null,
            PriorityTask? priority = null,
            DateTime? expectedStartDateMin = null,
            DateTime? expectedStartDateMax = null,
            DateTime? expectedEndDateMin = null,
            DateTime? expectedEndDateMax = null,
            double? expectedTimeMin = null,
            double? expectedTimeMax = null,
            double? processMin = null,
            double? processMax = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, subject, project, status, priority, expectedStartDateMin, expectedStartDateMax, expectedEndDateMin, expectedEndDateMax, expectedTimeMin, expectedTimeMax, processMin, processMax);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? HQTaskConsts.GetDefaultSorting(false) : sorting);
            return await query.As<IMongoQueryable<HQTask>>()
                .PageBy<HQTask, IMongoQueryable<HQTask>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<long> GetCountAsync(
           string filterText = null,
           string subject = null,
           string project = null,
           StatusTask? status = null,
           PriorityTask? priority = null,
           DateTime? expectedStartDateMin = null,
           DateTime? expectedStartDateMax = null,
           DateTime? expectedEndDateMin = null,
           DateTime? expectedEndDateMax = null,
           double? expectedTimeMin = null,
           double? expectedTimeMax = null,
           double? processMin = null,
           double? processMax = null,
           CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, subject, project, status, priority, expectedStartDateMin, expectedStartDateMax, expectedEndDateMin, expectedEndDateMax, expectedTimeMin, expectedTimeMax, processMin, processMax);
            return await query.As<IMongoQueryable<HQTask>>().LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<HQTask> ApplyFilter(
            IQueryable<HQTask> query,
            string filterText,
            string subject = null,
            string project = null,
            StatusTask? status = null,
            PriorityTask? priority = null,
            DateTime? expectedStartDateMin = null,
            DateTime? expectedStartDateMax = null,
            DateTime? expectedEndDateMin = null,
            DateTime? expectedEndDateMax = null,
            double? expectedTimeMin = null,
            double? expectedTimeMax = null,
            double? processMin = null,
            double? processMax = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Subject.Contains(filterText) || e.Project.Contains(filterText))
                    .WhereIf(!string.IsNullOrWhiteSpace(subject), e => e.Subject.Contains(subject))
                    .WhereIf(!string.IsNullOrWhiteSpace(project), e => e.Project.Contains(project))
                    .WhereIf(status.HasValue, e => e.Status == status)
                    .WhereIf(priority.HasValue, e => e.Priority == priority)
                    .WhereIf(expectedStartDateMin.HasValue, e => e.ExpectedStartDate >= expectedStartDateMin.Value)
                    .WhereIf(expectedStartDateMax.HasValue, e => e.ExpectedStartDate <= expectedStartDateMax.Value)
                    .WhereIf(expectedEndDateMin.HasValue, e => e.ExpectedEndDate >= expectedEndDateMin.Value)
                    .WhereIf(expectedEndDateMax.HasValue, e => e.ExpectedEndDate <= expectedEndDateMax.Value)
                    .WhereIf(expectedTimeMin.HasValue, e => e.ExpectedTime >= expectedTimeMin.Value)
                    .WhereIf(expectedTimeMax.HasValue, e => e.ExpectedTime <= expectedTimeMax.Value)
                    .WhereIf(processMin.HasValue, e => e.Process >= processMin.Value)
                    .WhereIf(processMax.HasValue, e => e.Process <= processMax.Value);
        }
    }
}