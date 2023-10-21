using Volo.Abp;
using Volo.Abp.MongoDB;

namespace ShopNServe.ProductCatalog.MongoDB;

public static class ProductCatalogMongoDbContextExtensions
{
    public static void ConfigureProductCatalog(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
    }
}
