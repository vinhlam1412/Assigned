using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace HQSOFT.Common.HQTasks
{
    public class HQTaskManager : DomainService
    {
        private readonly IHQTaskRepository _hQTaskRepository;

        public HQTaskManager(IHQTaskRepository hQTaskRepository)
        {
            _hQTaskRepository = hQTaskRepository;
        }

        public async Task<HQTask> CreateAsync(
        string subject, string project, StatusTask status, PriorityTask priority, DateTime expectedStartDate, DateTime expectedEndDate, double expectedTime, double process)
        {
            Check.NotNullOrWhiteSpace(subject, nameof(subject));
            Check.NotNullOrWhiteSpace(project, nameof(project));
            Check.NotNull(status, nameof(status));
            Check.NotNull(priority, nameof(priority));
            Check.NotNull(expectedStartDate, nameof(expectedStartDate));
            Check.NotNull(expectedEndDate, nameof(expectedEndDate));

            var hQTask = new HQTask(
             GuidGenerator.Create(),
             subject, project, status, priority, expectedStartDate, expectedEndDate, expectedTime, process
             );

            return await _hQTaskRepository.InsertAsync(hQTask);
        }

        public async Task<HQTask> UpdateAsync(
            Guid id,
            string subject, string project, StatusTask status, PriorityTask priority, DateTime expectedStartDate, DateTime expectedEndDate, double expectedTime, double process, [CanBeNull] string concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(subject, nameof(subject));
            Check.NotNullOrWhiteSpace(project, nameof(project));
            Check.NotNull(status, nameof(status));
            Check.NotNull(priority, nameof(priority));
            Check.NotNull(expectedStartDate, nameof(expectedStartDate));
            Check.NotNull(expectedEndDate, nameof(expectedEndDate));

            var hQTask = await _hQTaskRepository.GetAsync(id);

            hQTask.Subject = subject;
            hQTask.Project = project;
            hQTask.Status = status;
            hQTask.Priority = priority;
            hQTask.ExpectedStartDate = expectedStartDate;
            hQTask.ExpectedEndDate = expectedEndDate;
            hQTask.ExpectedTime = expectedTime;
            hQTask.Process = process;

            hQTask.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _hQTaskRepository.UpdateAsync(hQTask);
        }

    }
}