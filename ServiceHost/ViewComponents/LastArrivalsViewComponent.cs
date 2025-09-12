using _01_ArshinQuery.Contracts.Product;
using _01_ArshinQuery.Contracts.ProductCategory;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponents
{
    public class LastArrivalsViewComponent:ViewComponent
    {
        private readonly IProductQuery _productQuery;

        public LastArrivalsViewComponent(IProductQuery productQuery)
        {
            _productQuery = productQuery;
        }


        public IViewComponentResult Invoke()
        {
            var products = _productQuery.GetLastArrivals();
            return View(products);
        }
    }
}
