using System.Collections.Generic;
using _0_Framework.Domain;
using ShopManagement.Application.Contracts.Product;

namespace ShopManagement.Domain.ProductAgg
{
   public interface IProductRepository:IRepository<long,Product>
   {
        List<ProductViewModel> GetProducts();
        Product GetWithCategory(long id);
        EditProduct GetDetails(long id);
       List<ProductViewModel> Search(ProductSearchModel searchModel);
       
   }
}
