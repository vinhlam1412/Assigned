using HQSOFT.Common.HQNotifications;
using HQSOFT.Common.HQShares;
using HQSOFT.Common.HQAssigneds;
using HQSOFT.Common.HQTasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace HQSOFT.Common.MongoDB;

[DependsOn(
    typeof(CommonDomainModule),
    typeof(AbpMongoDbModule)
    )]
public class CommonMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<CommonMongoDbContext>(options =>
        {
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, MongoQuestionRepository>();
             */
            options.AddRepository<HQTask, HQTasks.MongoHQTaskRepository>();

            options.AddRepository<HQAssigned, HQAssigneds.MongoHQAssignedRepository>();

            options.AddRepository<HQShare, HQShares.MongoHQShareRepository>();

            options.AddRepository<HQNotification, HQNotifications.MongoHQNotificationRepository>();

        });
    }
}