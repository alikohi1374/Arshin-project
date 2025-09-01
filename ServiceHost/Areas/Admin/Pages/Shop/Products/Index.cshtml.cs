using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductCategory;

namespace ServiceHost.Areas.Admin.Pages.Shop.Products
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public ProductSearchModel SearchModel;
        public List<ProductViewModel> Product;
        public SelectList ProductCategories;
        private readonly IProductApplication _productApplication;
        private readonly IProductCategoryApplication _productCategoryApplication;

        public IndexModel(IProductApplication productApplication,IProductCategoryApplication categoryApplication)
        {
            _productApplication = productApplication;
            _productCategoryApplication = categoryApplication;
        }


        public void OnGet(ProductSearchModel searchModel)
        {
            ProductCategories = new SelectList(_productCategoryApplication.GetProductCategories(),"Id","Name");
            Product = _productApplication.Search(searchModel);
        }

        public IActionResult OnGetCreate()
        {
            var command = new CreateProduct
            {
                Categories = _productCategoryApplication.GetProductCategories()
            };
            return Partial("./Create", command);
        }

        public JsonResult OnPostCreate(CreateProduct command)
        {
           var result= _productApplication.Create(command);
           return new JsonResult(result);
        }

        public IActionResult OnGetEdit(long id)
        {
            var editProduct = _productApplication.GetDetails(id);
            editProduct.Categories = _productCategoryApplication.GetProductCategories();
            return Partial("Edit", editProduct);
        }

        public JsonResult OnPostEdit(EditProduct command)
        {
            var operationResult = _productApplication.Edit(command);
            return new JsonResult(operationResult);
        }

        public IActionResult OnGetNotInStock(long id)
        {
          var result= _productApplication.NotStock(id);
          if (result.IsSucceeded)
              return RedirectToPage("./index");

          Message = result.Message;
          return RedirectToPage("./index");

        }
        public IActionResult OnGetIsInStock(long id)
        {
            var result = _productApplication.InStock(id);
            if (result.IsSucceeded)
                return RedirectToPage("./index");

            Message = result.Message;
            return RedirectToPage("./index");

        }

    }
}
