using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace ShopNServe.ProductCatalog.Products;

public class ProductManager : DomainService
{
    private readonly IRepository<Product, Guid> _productRepository;

    public ProductManager(IRepository<Product, Guid> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product> CreateAsync(
        string code,
        string name,
        Money price,
        int stockCount = 0,
        string? imageName = null)
    {
        var existingProduct = await _productRepository.FirstOrDefaultAsync(p => p.Code == code);
        if (existingProduct != null)
        {
            throw new ProductCodeAlreadyExistsException(code);
        }

        return await _productRepository.InsertAsync(
            new Product(
                GuidGenerator.Create(),
                code,
                name,
                price,
                stockCount,
                imageName
            )
        );
    }
}
