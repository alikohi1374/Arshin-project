using System.Collections.Generic;

namespace _01_ArshinQuery.Contracts.ProductCategory
{
   public interface IProductCategoryQuery
   {
       List<ProductCategoryQueryModel>GetProductCategories();
   }
}
