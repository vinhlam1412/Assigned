using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace HQSOFT.Common.HQTasks
{
    public interface IHQTaskRepository : IRepository<HQTask, Guid>
    {
        Task<List<HQTask>> GetListAsync(
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
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
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
            CancellationToken cancellationToken = default);
    }
}