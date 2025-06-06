﻿using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp;
using Volo.Abp.MultiTenancy;

namespace ShopNServe.ProductCatalog.Products;

public class Product : AuditedAggregateRoot<Guid>, IMultiTenant
{
    /// <summary>
    /// A unique value for this product.
    /// ProductManager ensures the uniqueness of it.
    /// It cannot be changed after creation of the product.
    /// </summary>
    public string Code { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public Money Price { get; private set; } = new NullMoney();

    public int StockCount { get; private set; }

    public string? ImageName { get; private set; }

    public Guid? TenantId { get; private set; }

    private Product()
    {
        //Default constructor is needed for ORMs.
    }

    internal Product(
        Guid id,
        string code,
        string name,
        Money price,
        int stockCount = 0,
        string? imageName = null)
    {
        Check.NotNullOrWhiteSpace(code, nameof(code));

        if (code.Length >= ProductConsts.MaxCodeLength)
        {
            throw new ArgumentException($"Product code cannot be longer than {ProductConsts.MaxCodeLength}");
        }

        Id = id;
        Code = code;
        Price = price ?? throw new ArgumentNullException(nameof(price));
        SetName(Check.NotNullOrWhiteSpace(name, nameof(name)));
        SetPrice(price);
        SetImageName(imageName);
        SetStockCountInternal(stockCount, triggerEvent: false);
    }

    public Product SetName(string name)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        if (name.Length >= ProductConsts.MaxNameLength)
        {
            throw new ArgumentException($"Product name cannot be longer than {ProductConsts.MaxNameLength}");
        }

        Name = name;
        return this;
    }

    public Product SetImageName(string? imageName)
    {
        if (imageName == null)
        {
            return this;
        }

        if (imageName.Length >= ProductConsts.MaxImageNameLength)
        {
            throw new ArgumentException($"Product image name cannot be longer than {ProductConsts.MaxImageNameLength}");
        }

        ImageName = imageName;
        return this;
    }

    public Product SetPrice(Money price)
    {
        if (price.Amount < 0.0m)
        {
            throw new ArgumentException($"{nameof(price)} cannot be less than 0.0!");
        }

        Price = price;
        return this;
    }

    public Product SetStockCount(int stockCount)
    {
        return SetStockCountInternal(stockCount);
    }

    private Product SetStockCountInternal(int stockCount, bool triggerEvent = true)
    {
        if (StockCount < 0)
        {
            throw new ArgumentException($"{nameof(stockCount)} cannot be less than 0!");
        }

        if (StockCount == stockCount)
        {
            return this;
        }

        // TODO: trigger event ProductStockCountChangedEto
        //if (triggerEvent)
        //{
        //    AddDistributedEvent(
        //        new ProductStockCountChangedEto(
        //            Id,
        //            StockCount,
        //            stockCount
        //        )
        //    );
        //}

        StockCount = stockCount;
        return this;
    }
}
