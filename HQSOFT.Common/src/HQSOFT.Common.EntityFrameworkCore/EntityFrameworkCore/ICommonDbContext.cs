using HQSOFT.Common.HQShares;
using HQSOFT.Common.HQAssigneds;
using HQSOFT.Common.HQTasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace HQSOFT.Common.EntityFrameworkCore;

[ConnectionStringName(CommonDbProperties.ConnectionStringName)]
public interface ICommonDbContext : IEfCoreDbContext
{
    DbSet<HQShare> HQShares { get; set; }
    DbSet<HQAssigned> HQAssigneds { get; set; }
    DbSet<HQTask> HQTasks { get; set; }
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}