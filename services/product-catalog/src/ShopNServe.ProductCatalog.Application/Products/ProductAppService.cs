﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ShopNServe.ProductCatalog.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ShopNServe.ProductCatalog.Products;

[Authorize(ProductCatalogPermissions.Products.Default)]
public class ProductAppService : ProductCatalogAppService, IProductAppService
{
    private readonly ProductManager _productManager;
    private readonly IRepository<Product, Guid> _productRepository;

    public ProductAppService(ProductManager productManager, IRepository<Product, Guid> productRepository)
    {
        _productManager = productManager;
        _productRepository = productRepository;
    }

    public async Task<PagedResultDto<ProductDto>> GetListPagedAsync(PagedAndSortedResultRequestDto input)
    {
        var queryable = await _productRepository.GetQueryableAsync();

        var products = await AsyncExecuter.ToListAsync(
            queryable
                .OrderBy(input.Sorting ?? "Name")
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
        );

        var totalCount = await _productRepository.GetCountAsync();

        return new PagedResultDto<ProductDto>(
            totalCount,
            ObjectMapper.Map<List<Product>, List<ProductDto>>(products)
        );
    }

    public async Task<ListResultDto<ProductDto>> GetListAsync()
    {
        var products = await _productRepository.GetListAsync();
        return new ListResultDto<ProductDto>(
            ObjectMapper.Map<List<Product>, List<ProductDto>>(products)
        );
    }

    public async Task<ProductDto> GetAsync(Guid id)
    {
        var product = await _productRepository.GetAsync(id);
        return ObjectMapper.Map<Product, ProductDto>(product);
    }

    [Authorize(ProductCatalogPermissions.Products.Create)]
    public async Task<ProductDto> CreateAsync(CreateProductDto input)
    {
        var product = await _productManager.CreateAsync(
            input.Code,
            input.Name,
            new Money(input.PriceCurrency, input.PriceAmount),
            input.StockCount,
            input.ImageName
        );

        return ObjectMapper.Map<Product, ProductDto>(product);
    }

    [Authorize(ProductCatalogPermissions.Products.Update)]
    public async Task<ProductDto> UpdateAsync(Guid id, UpdateProductDto input)
    {
        var product = await _productRepository.GetAsync(id);

        product.SetName(input.Name);
        product.SetPrice(new Money(input.PriceCurrency, input.PriceAmount));
        product.SetStockCount(input.StockCount);
        product.SetImageName(input.ImageName);

        await _productRepository.UpdateAsync(product);

        return ObjectMapper.Map<Product, ProductDto>(product);
    }

    [Authorize(ProductCatalogPermissions.Products.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _productRepository.DeleteAsync(id);
    }
}