using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.Application.Contracts.ProductCategory;

namespace ServiceHost.Areas.Admin.Pages.Shop.ProductCategory
{
    public class IndexModel : PageModel
    {
        public ProductCategorySearchModel SearchModel;
        public List<ProductCategoryViewModel> ProductCategories;
        private readonly IProductCategoryApplication _productCategoryApplication;

        public IndexModel(IProductCategoryApplication productCategoryApplication)
        {
            _productCategoryApplication = productCategoryApplication;
        }


        public void OnGet(ProductCategorySearchModel searchModel)
        {
          ProductCategories=  _productCategoryApplication.Search(searchModel);
        }

        public IActionResult OnGetCreate()
        {
            return Partial("./Create", new CreateProductCategory());
        }

        public JsonResult OnPostCreate(CreateProductCategory command)
        {
           var result= _productCategoryApplication.Create(command);
           return new JsonResult(result);
        }

        public IActionResult OnGetEdit(long id)
        {
            var editProductCategory = _productCategoryApplication.GetDetails(id);
            return Partial("Edit", editProductCategory);
        }

        public JsonResult OnPostEdit(EditProductCategory command)
        {
            var operationResult = _productCategoryApplication.Edit(command);
            return new JsonResult(operationResult);
        }
    }
}
