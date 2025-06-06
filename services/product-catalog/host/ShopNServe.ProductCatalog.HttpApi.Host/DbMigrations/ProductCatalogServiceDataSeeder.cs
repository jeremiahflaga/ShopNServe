﻿//using System;
//using System.Threading.Tasks;
//using ShopNServe.ProductCatalog.Products;
//using Volo.Abp.Data;
//using Volo.Abp.DependencyInjection;
//using Volo.Abp.Domain.Repositories;
//using Volo.Abp.Uow;

//namespace EShopOnAbp.CatalogService.DbMigrations;

//public class ProductCatalogServiceDataSeeder : IDataSeedContributor, ITransientDependency
//{
//    private readonly ProductManager _productManager;
//    private readonly IRepository<Product, Guid> _productRepository;

//    public ProductCatalogServiceDataSeeder(
//        IRepository<Product, Guid> productRepository, 
//        ProductManager productManager)
//    {
//        _productRepository = productRepository;
//        _productManager = productManager;
//    }

//    public async Task SeedAsync(DataSeedContext context)
//    {
//        await AddProductsAsync();
//    }

//    private async Task AddProductsAsync()
//    {
//        if (await _productRepository.GetCountAsync() > 0)
//        {
//            return;
//        }

//        await _productManager.CreateAsync(
//            "JMF0000001",
//            "Mere Christianity",
//            999,
//            42,
//            "lego.jpg"
//        );
            
//        await _productManager.CreateAsync(
//            "JMF0000002",
//            "Working Effectively with Legacy Code",
//            1499,
//            56,
//            "nikon.jpg"
//        );

//        await _productManager.CreateAsync(
//            "ABP82731",
//            "Beats Solo3 Wireless On-Ear Headphone",
//            97,
//            20,
//            "beats.jpg"
//        );

//        await _productManager.CreateAsync(
//            "ABP12322",
//            "Rampage Sn-Rw2 Gamer Headphone",
//            654,
//            42,
//            "rampage.jpg"
//        );

//        await _productManager.CreateAsync(
//            "ABP00291",
//            "Asus Transformer Book T300CHI-FH011H",
//            1249,
//            3,
//            "asus.jpg"
//        );

//        await _productManager.CreateAsync(
//            "ABP02918",
//            "OKI C332DN Dublex + Network A4 Laser Printer",
//            215,
//            6,
//            "oki.jpg"
//        );

//        await _productManager.CreateAsync(
//            "ABP11121",
//            "Bluecat Rd810 Mini Led",
//            449,
//            13,
//            "bluecat.jpg"
//        );

//        await _productManager.CreateAsync(
//            "ABP44432",
//            "Sunny 55\" TV 4K Ultra HD Curved Smart Led TV",
//            2249,
//            1,
//            "sunny.jpg"
//        );

//        await _productManager.CreateAsync(
//            "ABP37182",
//            "Sony Playstation 4 Slim 500 GB (PAL)",
//            699,
//            120,
//            "playstation.jpg"
//        );
//    }
//}