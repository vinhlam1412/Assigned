using HQSOFT.Common.HQShares;
using HQSOFT.Common.HQAssigneds;
using HQSOFT.Common.HQTasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace HQSOFT.Common.EntityFrameworkCore;

[DependsOn(
    typeof(CommonDomainModule),
    typeof(AbpEntityFrameworkCoreModule),
    typeof(AbpIdentityEntityFrameworkCoreModule)
)]
public class CommonEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<CommonDbContext>(options =>
        {
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, EfCoreQuestionRepository>();
             */
            options.AddRepository<HQTask, HQTasks.EfCoreHQTaskRepository>();

            options.AddRepository<HQAssigned, HQAssigneds.EfCoreHQAssignedRepository>();

            options.AddRepository<HQShare, HQShares.EfCoreHQShareRepository>();

        });
    }
}