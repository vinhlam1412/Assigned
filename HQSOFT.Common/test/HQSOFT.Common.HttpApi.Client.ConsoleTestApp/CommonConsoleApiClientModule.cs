using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace HQSOFT.Common;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(CommonHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelModule)
    )]
public class CommonConsoleApiClientModule : AbpModule
{

}
