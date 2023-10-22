using System.ComponentModel.DataAnnotations;

namespace ShopNServe.ProductCatalog.Products;

public class CreateProductDto
{
    [Required]
    [StringLength(ProductConsts.MaxCodeLength)]
    public string Code { get; set; }

    [Required]
    [StringLength(ProductConsts.MaxNameLength)]
    public string Name { get; set; }

    [StringLength(ProductConsts.MaxImageNameLength)]
    public string ImageName { get; set; }

    public string PriceCurrency { get; set; }
    public decimal PriceAmount { get; set; }

    public int StockCount { get; set; }
}