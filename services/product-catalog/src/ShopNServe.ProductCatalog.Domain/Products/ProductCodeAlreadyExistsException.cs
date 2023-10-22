using Volo.Abp;

namespace ShopNServe.ProductCatalog.Products;

public class ProductCodeAlreadyExistsException : BusinessException
{
    public ProductCodeAlreadyExistsException(string productCode)
        : base("CatalogService:000001", $"A product with code {productCode} already exists!")
    {
        WithData("productCode", productCode);
    }
}