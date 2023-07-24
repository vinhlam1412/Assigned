using HQSOFT.Common.HQShares;
using HQSOFT.Common.HQAssigneds;
using HQSOFT.Common.HQTasks;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace HQSOFT.Common.MongoDB;

[ConnectionStringName(CommonDbProperties.ConnectionStringName)]
public class CommonMongoDbContext : AbpMongoDbContext, ICommonMongoDbContext
{
    public IMongoCollection<HQShare> HQShares => Collection<HQShare>();
    public IMongoCollection<HQAssigned> HQAssigneds => Collection<HQAssigned>();
    public IMongoCollection<HQTask> HQTasks => Collection<HQTask>();
    /* Add mongo collections here. Example:
     * public IMongoCollection<Question> Questions => Collection<Question>();
     */

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureCommon();

        modelBuilder.Entity<HQTask>(b => { b.CollectionName = CommonDbProperties.DbTablePrefix + "HQTasks"; });

        modelBuilder.Entity<HQAssigned>(b => { b.CollectionName = CommonDbProperties.DbTablePrefix + "HQAssigneds"; });

        modelBuilder.Entity<HQShare>(b => { b.CollectionName = CommonDbProperties.DbTablePrefix + "HQShares"; });
    }
}