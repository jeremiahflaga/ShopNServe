using Microsoft.EntityFrameworkCore;
using ShopNServe.ProductCatalog.Products;
using System.Reflection.Emit;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ShopNServe.ProductCatalog.EntityFrameworkCore;

public static class ProductCatalogDbContextModelCreatingExtensions
{
    public static void ConfigureProductCatalog(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        /* Configure all entities here. Example:

        builder.Entity<Question>(b =>
        {
            //Configure table & schema name
            b.ToTable(ProductCatalogDbProperties.DbTablePrefix + "Questions", ProductCatalogDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties
            b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

            //Relations
            b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

            //Indexes
            b.HasIndex(q => q.CreationTime);
        });
        */

        builder.Entity<Product>(b =>
        {
            //Configure table & schema name
            b.ToTable(ProductCatalogDbProperties.DbTablePrefix + "Product", ProductCatalogDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties
            b.Property(q => q.Code).IsRequired().HasMaxLength(ProductConsts.MaxCodeLength);
            b.Property(q => q.Name).IsRequired().HasMaxLength(ProductConsts.MaxNameLength);
            b.Property(q => q.ImageName).IsRequired().HasMaxLength(ProductConsts.MaxImageNameLength);
        });
    }
}
