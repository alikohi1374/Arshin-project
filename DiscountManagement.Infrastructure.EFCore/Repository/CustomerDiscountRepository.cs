using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using DiscountManagement.Application.Contract.CustomerDiscount;
using DiscountManagement.Domain.CustomerDiscountAgg;
using ShopManagement.Infrastructure.EFCore;

namespace DiscountManagement.Infrastructure.EFCore.Repository
{
   public class CustomerDiscountRepository:RepositoryBase<long,CustomerDiscount>, ICustomerDiscountRepository
    {
       private readonly DiscountContext _context;
       private readonly ShopContext _shopContext;

       public CustomerDiscountRepository(DiscountContext context,ShopContext shopContext):base(context)
       {
           _context = context;
           _shopContext = shopContext;
       }


       public List<CustomerDiscountViewModel> Search(CustomerDiscountSearchModel searchModel)
       {
           var products = _shopContext.Product.Select(x => new {x.Id, x.Name}).ToList();
           var query = _context.CustomerDiscounts.Select(x => new CustomerDiscountViewModel
           {
               Id = x.Id,
               DiscountRate = x.DiscountRate,
               Reason = x.Reason,
               StartDate = x.StartDate.ToFarsi(),
               EndDate = x.EndDate.ToFarsi(),
               EndDateGr = x.EndDate,
               StartDateGr = x.StartDate,
               ProductId = x.ProductId,
               CreationDate = x.CreationDate.ToFarsi()
               
           });

           if (searchModel.ProductId > 0)
               query = query.Where(x => x.ProductId == searchModel.ProductId);
           if (!string.IsNullOrWhiteSpace(searchModel.StartDate))
           { 
               query = query.Where(x => x.StartDateGr > searchModel.StartDate.ToGeorgianDateTime());
           }

           if (!string.IsNullOrWhiteSpace(searchModel.EndDate))
           {
             
               query = query.Where(x => x.EndDateGr < searchModel.EndDate.ToGeorgianDateTime());
           }

           var discount = query.OrderByDescending(x => x.Id).ToList();

           discount.ForEach(discount=>
               discount.Product=products.FirstOrDefault(x=>x.Id==discount.ProductId)?.Name);
           return discount;


       }

       public EditCustomerDiscount GetDetails(long id)
       {
           return _context.CustomerDiscounts.Select(x => new EditCustomerDiscount
           {
               DiscountRate = x.DiscountRate,
               EndDate = x.EndDate.ToString(CultureInfo.InvariantCulture),
               Id = x.Id,
               ProductId = x.ProductId,
               Reason = x.Reason,
               StartDate = x.StartDate.ToString(CultureInfo.InvariantCulture)
           }).FirstOrDefault(x => x.Id ==id);
        }
   }
}
