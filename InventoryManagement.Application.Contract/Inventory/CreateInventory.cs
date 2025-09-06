using ShopManagement.Application.Contracts.Product;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using _0_Framework.Application;

namespace InventoryManagement.Application.Contract.Inventory
{
   public class CreateInventory
    {
        [Range(1,1000000,ErrorMessage = ValidationMessages.IsRequired)]
        public long ProductId { get; set; }
        [Range(1, double.MaxValue, ErrorMessage = ValidationMessages.IsRequired)]
        public double UnitPrice { get;  set; }
        public List<ProductViewModel> Products { get; set; }
    }
}
