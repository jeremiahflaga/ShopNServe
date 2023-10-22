using System;
using Volo.Abp.Application.Dtos;

namespace ShopNServe.ProductCatalog.Products;

public class ProductDto : AuditedEntityDto<Guid>
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string ImageName { get; set; }
    public string PriceCurrency { get; set; }
    public decimal PriceAmount { get; set; }
    public int StockCount { get; set; }
}