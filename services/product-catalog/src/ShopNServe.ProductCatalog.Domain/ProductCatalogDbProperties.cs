namespace ShopNServe.ProductCatalog;

public static class ProductCatalogDbProperties
{
    public static string DbTablePrefix { get; set; } = "";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "ProductCatalog";
}
