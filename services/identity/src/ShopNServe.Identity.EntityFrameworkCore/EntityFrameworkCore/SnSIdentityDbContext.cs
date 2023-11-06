using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;

namespace ShopNServe.Identity.EntityFrameworkCore;

[ConnectionStringName(IdentityDbProperties.ConnectionStringName)]
public class SnSIdentityDbContext : AbpDbContext<SnSIdentityDbContext>, ISnSIdentityDbContext,
    Volo.Abp.Identity.EntityFrameworkCore.IIdentityDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public DbSet<IdentityUser> Users { get; set; }

    public DbSet<IdentityRole> Roles { get; set; }

    public DbSet<IdentityClaimType> ClaimTypes { get; set; }

    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }

    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }

    public DbSet<IdentityLinkUser> LinkUsers { get; set; }

    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }

    public SnSIdentityDbContext(DbContextOptions<SnSIdentityDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureIdentity();
        builder.ConfigureSnSIdentity();
    }
}
