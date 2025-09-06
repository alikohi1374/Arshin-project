using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using InventoryManagement.Application.Contract.Inventory;
using InventoryManagement.Domain.InventoryAgg;
using ShopManagement.Infrastructure.EFCore;

namespace InventoryManagement.Infrastructure.EFCore.Repository
{
  public class InventoryRepository:RepositoryBase<long,Inventory>,IInventoryRepository
  {
      private readonly ShopContext _shopContext;
      private readonly InventoryContext _inventory;
        public InventoryRepository(InventoryContext inventory, ShopContext shopContext) : base(inventory)
        {
            _inventory = inventory;
            _shopContext = shopContext;
        }

        public EditInventory GetDetails(long id)
        {
            return _inventory.Inventory.Select(x => new EditInventory
            {
                Id = x.Id,
                ProductId = x.ProductId,
                UnitPrice = x.UnitPrice
            }).FirstOrDefault(x => x.Id == id);
        }

        public Inventory GetBy(long productId)
        {
            return _inventory.Inventory.FirstOrDefault(x => x.ProductId == productId);
        }

        public List<InventoryViewModel> Search(InventorySearchModel searchModel)
        {
            var products = _shopContext.Product.Select(x => new {x.Id, x.Name}).ToList();

            var query = _inventory.Inventory.Select(x => new InventoryViewModel
            {
                Id = x.Id,
                UnitPrice = x.UnitPrice,
                InStock = x.InStock,
                ProductId = x.ProductId,
                CurrentCount = x.CalculateCurrentCount(),
                CreationDate = x.CreationDate.ToFarsi()
            });
            if (searchModel.ProductId > 0)
                query = query.Where(x => x.ProductId == searchModel.ProductId);

            if (searchModel.InStock)
                query = query.Where(x =>!x.InStock);

            var inventory = query.OrderByDescending(x => x.Id).ToList();

            inventory.ForEach(item =>
            {
                item.Product = products.FirstOrDefault(x => x.Id == item.ProductId)?.Name;
            });
            return inventory;

        }

        public List<InventoryOperationViewModel> GetOperationLog(long inventoryId)
        {
            var inventory = _inventory.Inventory.FirstOrDefault(x => x.Id == inventoryId);
            return inventory.Operations.Select(x => new InventoryOperationViewModel
            {
                Id = x.Id,
                CurrentCount = x.CurrentCount,
                Count = x.Count,
                Description = x.Description,
                Operation = x.Operation,
                OrderId = x.OrderId,
                Operator = "مدیر",
                OperationDate = x.OperationDate.ToFarsi(),
                OperatorId = x.OperationId
            }).OrderByDescending(x=>x.Id).ToList();
        }
  }
}
