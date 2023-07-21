using HQSOFT.Common.HQAssigneds;
using HQSOFT.Common.HQTasks;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Identity;

namespace HQSOFT.Common.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ConnectionStringName(CommonDbProperties.ConnectionStringName)]
public class CommonDbContext : AbpDbContext<CommonDbContext>, ICommonDbContext, IIdentityDbContext
{
    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }

    public DbSet<HQAssigned> HQAssigneds { get; set; }
    public DbSet<HQTask> HQTasks { get; set; }
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public CommonDbContext(DbContextOptions<CommonDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ConfigureIdentity();
        builder.ConfigureCommon();
    }
}