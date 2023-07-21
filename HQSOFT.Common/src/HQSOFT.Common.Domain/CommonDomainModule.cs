using Volo.Abp.Caching;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace HQSOFT.Common;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpCachingModule),
    typeof(CommonDomainSharedModule)
)]
public class CommonDomainModule : AbpModule
{

}
