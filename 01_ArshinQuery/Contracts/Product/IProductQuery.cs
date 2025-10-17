using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopManagement.Application.Contracts.Order;

namespace _01_ArshinQuery.Contracts.Product
{
   public interface IProductQuery
   {
        ProductQueryModel GetProductDetails(string slug);
        List<ProductQueryModel> GetLastArrivals();
       List<ProductQueryModel> Search(string value);
       List<CartItem> CheckInventoryStatus(List<CartItem> cartItems);
   }
}
