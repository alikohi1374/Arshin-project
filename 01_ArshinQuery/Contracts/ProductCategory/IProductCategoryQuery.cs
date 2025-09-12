using System.Collections.Generic;
using _01_ArshinQuery.Contracts.Product;
using ShopManagement.Application.Contracts.ProductCategory;

namespace _01_ArshinQuery.Contracts.ProductCategory
{
   public interface IProductCategoryQuery
   {
       ProductCategoryQueryModel GetProductCategoryWithProductsBy(string slug);
       List<ProductCategoryQueryModel>GetProductCategories();
       List<ProductCategoryQueryModel> GetProductCategoriesWithProducts();
   }
}
