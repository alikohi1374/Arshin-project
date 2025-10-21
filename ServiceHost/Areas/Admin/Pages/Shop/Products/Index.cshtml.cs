using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductCategory;


namespace ServiceHost.Areas.Admin.Pages.Shop.Products
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }

        public ProductSearchModel SearchModel;
        public List<ProductViewModel> Products;
        public SelectList ProductCategories;

        private readonly IProductApplication _productApplication;
        private readonly IProductCategoryApplication _productCategoryApplication;
        private readonly IProductAttributeApplication _productAttributeApplication; // اضافه شده

        public IndexModel(
            IProductApplication productApplication,
            IProductCategoryApplication categoryApplication,
            IProductAttributeApplication attributeApplication) // اضافه شده
        {
            _productApplication = productApplication;
            _productCategoryApplication = categoryApplication;
            _productAttributeApplication = attributeApplication;
        }

        // --------------------- [GET Index] ---------------------
        public void OnGet(ProductSearchModel searchModel)
        {
            ProductCategories = new SelectList(_productCategoryApplication.GetProductCategories(), "Id", "Name");
            Products = _productApplication.Search(searchModel);
        }

        // --------------------- [GET Create Partial] ---------------------
        public IActionResult OnGetCreate()
        {
            var command = new CreateProduct
            {
                Categories = _productCategoryApplication.GetProductCategories(),
                AvailableAttributes = _productAttributeApplication.GetAllAttributes()  // خواندن ویژگی‌ها
            };
            return Partial("./Create", command);
        }

        // --------------------- [POST Create] ---------------------
        public JsonResult OnPostCreate(CreateProduct command)
        {
            if (!ModelState.IsValid)
                return new JsonResult(new { success = false, message = "اطلاعات ورودی نامعتبر است." });

            var result = _productApplication.Create(command);

            // اگر محصول با موفقیت ساخته شد، ویژگی‌ها هم ذخیره شوند
            if (result.IsSucceeded)
            {
                var createdProduct = _productApplication.Search(new ProductSearchModel { Name = command.Name }).LastOrDefault();
                if (createdProduct != null)
                {
                    _productAttributeApplication.SetProductAttributes(createdProduct.Id, command.AttributeValues);
                }
            }

            return new JsonResult(result);
        }

        // --------------------- [GET Edit Partial] ---------------------
        public IActionResult OnGetEdit(long id)
        {
            var editProduct = _productApplication.GetDetails(id);
            editProduct.Categories = _productCategoryApplication.GetProductCategories();

            // ویژگی‌ها را هم واکشی کن
            editProduct.AvailableAttributes = _productAttributeApplication.GetAllAttributes();
            editProduct.AttributeValues = _productAttributeApplication.GetProductAttributesByProductId(id);

            return Partial("./Edit", editProduct);
        }

        // --------------------- [POST Edit] ---------------------
        public JsonResult OnPostEdit(EditProduct command)
        {
            if (!ModelState.IsValid)
                return new JsonResult(new { success = false, message = "اطلاعات ورودی نامعتبر است." });

            var operationResult = _productApplication.Edit(command);

            // به‌روزرسانی ویژگی‌ها
            if (operationResult.IsSucceeded && command.AttributeValues != null && command.AttributeValues.Count > 0)
            {
                _productAttributeApplication.SetProductAttributes(command.Id, command.AttributeValues);
            }

            return new JsonResult(operationResult);
        }
    }
}
