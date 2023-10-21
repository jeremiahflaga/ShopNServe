using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ShopNServe.ProductCatalog.EntityFrameworkCore;

public class ProductCatalogHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<ProductCatalogHttpApiHostMigrationsDbContext>
{
    public ProductCatalogHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<ProductCatalogHttpApiHostMigrationsDbContext>()
            .UseNpgsql(configuration.GetConnectionString("ProductCatalog"));

        return new ProductCatalogHttpApiHostMigrationsDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
