using Volo.Abp.Modularity;

namespace HQSOFT.Common;

[DependsOn(
    typeof(CommonApplicationModule),
    typeof(CommonDomainTestModule)
    )]
public class CommonApplicationTestModule : AbpModule
{

}
