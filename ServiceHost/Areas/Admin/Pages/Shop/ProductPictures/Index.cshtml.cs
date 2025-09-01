using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductPicture;

namespace ServiceHost.Areas.Admin.Pages.Shop.ProductPictures
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public ProductPictureSearchModel SearchModel;
        public List<ProductPictureViewModel> ProductPictures;
        public SelectList Products;
        private readonly IProductApplication _productApplication;
        private readonly IProductPictureApplication _productPictureApplication;
      

        public IndexModel(IProductApplication productApplication, IProductPictureApplication productPictureApplication)
        {
            _productApplication = productApplication;
            _productPictureApplication = productPictureApplication;
        }


        public void OnGet(ProductPictureSearchModel searchModel)
        {
            Products= new SelectList(_productApplication.GetProducts(),"Id","Name");
            ProductPictures = _productPictureApplication.Search(searchModel);
        }

        public IActionResult OnGetCreate()
        {
            var command = new CreateProductPicture()
            {
               Products = _productApplication.GetProducts()
            };
            return Partial("./Create", command);
        }

        public JsonResult OnPostCreate(CreateProductPicture command)
        {
           var result= _productPictureApplication.Create(command);
           return new JsonResult(result);
        }

        public IActionResult OnGetEdit(long id)
        {
            var editProduct = _productPictureApplication.GetDetails(id);
            editProduct.Products = _productApplication.GetProducts();
            return Partial("Edit", editProduct);
        }

        public JsonResult OnPostEdit(EditProductPicture command)
        {
            var operationResult = _productPictureApplication.Edit(command);
            return new JsonResult(operationResult);
        }

        public IActionResult OnGetRemove(long id)
        {
          var result= _productPictureApplication.Remove(id);
          if (result.IsSucceeded)
              return RedirectToPage("./index");

          Message = result.Message;
          return RedirectToPage("./index");

        }
        public IActionResult OnGetRestore(long id)
        {
            var result = _productPictureApplication.Restore(id);
            if (result.IsSucceeded)
                return RedirectToPage("./index");

            Message = result.Message;
            return RedirectToPage("./index");

        }

    }
}
