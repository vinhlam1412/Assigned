using HQSOFT.Common.HQAssigneds;
using HQSOFT.Common.HQTasks;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace HQSOFT.Common.MongoDB;

[ConnectionStringName(CommonDbProperties.ConnectionStringName)]
public interface ICommonMongoDbContext : IAbpMongoDbContext
{
    IMongoCollection<HQAssigned> HQAssigneds { get; }
    IMongoCollection<HQTask> HQTasks { get; }
    /* Define mongo collections here. Example:
     * IMongoCollection<Question> Questions { get; }
     */
}