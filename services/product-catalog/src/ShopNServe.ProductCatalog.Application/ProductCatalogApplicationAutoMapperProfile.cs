using AutoMapper;
using ShopNServe.ProductCatalog.Products;

namespace ShopNServe.ProductCatalog;

public class ProductCatalogApplicationAutoMapperProfile : Profile
{
    public ProductCatalogApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Product, ProductDto>()
            .ForMember(x => x.PriceCurrency, opt => opt.MapFrom(x => x.Price.Currency))
            .ForMember(x => x.PriceAmount, opt => opt.MapFrom(x => x.Price.Amount));
    }
}
