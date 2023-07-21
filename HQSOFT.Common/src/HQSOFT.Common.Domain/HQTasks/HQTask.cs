using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace HQSOFT.Common.HQTasks
{
    public class HQTask : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string Subject { get; set; }

        [NotNull]
        public virtual string Project { get; set; }

        public virtual StatusTask Status { get; set; }

        public virtual PriorityTask Priority { get; set; }

        public virtual DateTime ExpectedStartDate { get; set; }

        public virtual DateTime ExpectedEndDate { get; set; }

        public virtual double ExpectedTime { get; set; }

        public virtual double Process { get; set; }

        public HQTask()
        {

        }

        public HQTask(Guid id, string subject, string project, StatusTask status, PriorityTask priority, DateTime expectedStartDate, DateTime expectedEndDate, double expectedTime, double process)
        {

            Id = id;
            Check.NotNull(subject, nameof(subject));
            Check.NotNull(project, nameof(project));
            Subject = subject;
            Project = project;
            Status = status;
            Priority = priority;
            ExpectedStartDate = expectedStartDate;
            ExpectedEndDate = expectedEndDate;
            ExpectedTime = expectedTime;
            Process = process;
        }

    }
}