using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace HQSOFT.Common;

[DependsOn(
    typeof(CommonDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationAbstractionsModule)
    )]
public class CommonApplicationContractsModule : AbpModule
{

}
