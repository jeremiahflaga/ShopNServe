using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopNServe.ProductCatalog.Products;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ShopNServe.AdminPanel.Web.Pages
{
    public class ProductCatalogModel : PageModel
    {
        public IReadOnlyList<ProductDto> Products { get; private set; } = new List<ProductDto>();

        private readonly IProductAppService productAppService;

        public ProductCatalogModel(IProductAppService productAppService)
        {
            this.productAppService = productAppService;
        }

        public async Task OnGet()
        {
            try
            {
                Products = (await productAppService.GetListAsync()).Items;

            }
            catch (Exception e)
            {
                Products = new ReadOnlyCollection<ProductDto>(new List<ProductDto>());
                Console.WriteLine(e);
            }
        }
    }
}
