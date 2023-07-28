using HQSOFT.Common.HQNotifications;
using HQSOFT.Common.HQShares;
using HQSOFT.Common.HQAssigneds;
using Volo.Abp.EntityFrameworkCore.Modeling;
using HQSOFT.Common.HQTasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Identity;

namespace HQSOFT.Common.EntityFrameworkCore;

public static class CommonDbContextModelCreatingExtensions
{
    public static void ConfigureCommon(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        /* Configure all entities here. Example:

        builder.Entity<Question>(b =>
        {
            //Configure table & schema name
            b.ToTable(CommonDbProperties.DbTablePrefix + "Questions", CommonDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties
            b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

            //Relations
            b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

            //Indexes
            b.HasIndex(q => q.CreationTime);
        });
        */
        if (builder.IsHostDatabase())
        {
            builder.Entity<HQTask>(b =>
{
    b.ToTable(CommonDbProperties.DbTablePrefix + "HQTasks", CommonDbProperties.DbSchema);
    b.ConfigureByConvention();
    b.Property(x => x.Subject).HasColumnName(nameof(HQTask.Subject)).IsRequired();
    b.Property(x => x.Project).HasColumnName(nameof(HQTask.Project)).IsRequired();
    b.Property(x => x.Status).HasColumnName(nameof(HQTask.Status));
    b.Property(x => x.Priority).HasColumnName(nameof(HQTask.Priority));
    b.Property(x => x.ExpectedStartDate).HasColumnName(nameof(HQTask.ExpectedStartDate));
    b.Property(x => x.ExpectedEndDate).HasColumnName(nameof(HQTask.ExpectedEndDate));
    b.Property(x => x.ExpectedTime).HasColumnName(nameof(HQTask.ExpectedTime));
    b.Property(x => x.Process).HasColumnName(nameof(HQTask.Process));
});

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<HQAssigned>(b =>
{
    b.ToTable(CommonDbProperties.DbTablePrefix + "HQAssigneds", CommonDbProperties.DbSchema);
    b.ConfigureByConvention();
    b.Property(x => x.IDParent).HasColumnName(nameof(HQAssigned.IDParent));
    b.Property(x => x.Completeby).HasColumnName(nameof(HQAssigned.Completeby));
    b.Property(x => x.Priority).HasColumnName(nameof(HQAssigned.Priority));
    b.Property(x => x.Comment).HasColumnName(nameof(HQAssigned.Comment));
    b.HasMany(x => x.IdentityUsers).WithOne().HasForeignKey(x => x.HQAssignedId).IsRequired().OnDelete(DeleteBehavior.NoAction);
});

            builder.Entity<HQAssignedIdentityUser>(b =>
{
    b.ToTable(CommonDbProperties.DbTablePrefix + "HQAssignedIdentityUser" + CommonDbProperties.DbSchema);
    b.ConfigureByConvention();

    b.HasKey(
        x => new { x.HQAssignedId, x.IdentityUserId }
    );

    b.HasOne<HQAssigned>().WithMany(x => x.IdentityUsers).HasForeignKey(x => x.HQAssignedId).IsRequired().OnDelete(DeleteBehavior.NoAction);
    b.HasOne<IdentityUser>().WithMany().HasForeignKey(x => x.IdentityUserId).IsRequired().OnDelete(DeleteBehavior.NoAction);

    b.HasIndex(
            x => new { x.HQAssignedId, x.IdentityUserId }
    );
});
        }
        if (builder.IsHostDatabase())
        {

            builder.Entity<HQShareIdentityUser>(b =>
{
    b.ToTable(CommonDbProperties.DbTablePrefix + "HQShareIdentityUser" + CommonDbProperties.DbSchema);
    b.ConfigureByConvention();

    b.HasKey(
        x => new { x.HQShareId, x.IdentityUserId }
    );

    b.HasOne<IdentityUser>().WithMany().HasForeignKey(x => x.IdentityUserId).IsRequired().OnDelete(DeleteBehavior.NoAction);

    b.HasIndex(
            x => new { x.HQShareId, x.IdentityUserId }
    );
});
        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<HQShare>(b =>
{
    b.ToTable(CommonDbProperties.DbTablePrefix + "HQShares", CommonDbProperties.DbSchema);
    b.ConfigureByConvention();
    b.Property(x => x.IDParent).HasColumnName(nameof(HQShare.IDParent));
    b.Property(x => x.CanRead).HasColumnName(nameof(HQShare.CanRead));
    b.Property(x => x.CanWrite).HasColumnName(nameof(HQShare.CanWrite));
    b.Property(x => x.CanSubmit).HasColumnName(nameof(HQShare.CanSubmit));
    b.Property(x => x.CanShare).HasColumnName(nameof(HQShare.CanShare));
    b.HasOne<IdentityUser>().WithMany().HasForeignKey(x => x.IdentityUserId).OnDelete(DeleteBehavior.NoAction);
});

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<HQNotification>(b =>
{
    b.ToTable(CommonDbProperties.DbTablePrefix + "HQNotifications", CommonDbProperties.DbSchema);
    b.ConfigureByConvention();
    b.Property(x => x.IDParent).HasColumnName(nameof(HQNotification.IDParent));
    b.Property(x => x.ToUser).HasColumnName(nameof(HQNotification.ToUser));
    b.Property(x => x.FromUser).HasColumnName(nameof(HQNotification.FromUser));
    b.Property(x => x.NotiTitle).HasColumnName(nameof(HQNotification.NotiTitle));
    b.Property(x => x.NotiBody).HasColumnName(nameof(HQNotification.NotiBody));
    b.Property(x => x.Type).HasColumnName(nameof(HQNotification.Type));
    b.Property(x => x.isRead).HasColumnName(nameof(HQNotification.isRead));
});

        }
    }
}