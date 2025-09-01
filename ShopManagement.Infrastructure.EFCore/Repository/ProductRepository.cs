using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Domain.ProductAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository
{
    public class ProductRepository : RepositoryBase<long, Product>, IProductRepository
    {
        private readonly ShopContext _context;

        public ProductRepository(ShopContext context) : base(context)
        {
            _context = context;
        }

        public List<ProductViewModel> GetProducts()
        {
            return _context.Product.Select(x => new ProductViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

        }

        public EditProduct GetDetails(long id)
        {
            return _context.Product.Select(x => new EditProduct
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                Slug = x.Slug,
                CategoryId = x.CategoryId,
                ShortDescription = x.ShortDescription,
                Keywords = x.Keywords,
                PictureAlt = x.PictureAlt,
                MetaDescription = x.MetaDescription,
                Picture = x.Picture,
                PictureTitle = x.PictureTitle,
                UnitPrice = x.UnitPrice,
                Description = x.Description,

            }).FirstOrDefault(s => s.Id == id);

        }

        public List<ProductViewModel> Search(ProductSearchModel searchModel)
        {
            var query = _context.Product
                .Include(x => x.Category)
                .Select(x => new ProductViewModel
                {
                    Id = x.Id,
                    Category = x.Category.Name,
                    Code = x.Code,
                    Picture = x.Picture,
                    Name = x.Name,
                    UnitPrice = x.UnitPrice,
                    CategoryId = x.CategoryId,
                    IsInStock = x.IsInStock,
                    CreationDate = x.CreationDate.ToString(CultureInfo.InvariantCulture)

                });
            if (!string.IsNullOrWhiteSpace(searchModel.Code))
                query = query.Where(x => x.Code.Contains(searchModel.Code));

            if (!string.IsNullOrWhiteSpace(searchModel.Name))
                query = query.Where(x => x.Name.Contains(searchModel.Name));

            if (searchModel.CategoryId != 0)
                query = query.Where(x => x.CategoryId == searchModel.CategoryId);


            return query.OrderByDescending(x => x.Id).ToList();
        }


    }
}
