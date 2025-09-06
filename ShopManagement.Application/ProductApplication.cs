using System.Collections.Generic;
using _0_Framework.Application;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Domain.ProductAgg;

namespace ShopManagement.Application
{
   public class ProductApplication:IProductApplication
   {
       private readonly IProductRepository _productRepository;

       public ProductApplication(IProductRepository productRepository)
       {
           _productRepository = productRepository;
       }

       public OperationResult Create(CreateProduct command)
       {
           var operation = new OperationResult();
           if (_productRepository.Exist(x => x.Name == command.Name))
               return operation.Failed(ApplicationMessages.DuplicatedRecord);

           var Slug =command.Slug.Slugify();

           var product = new Product(command.Description,command.Name, command.Code, command.ShortDescription,
               command.Picture,
               command.PictureAlt, command.PictureTitle,
               command.Keywords, command.MetaDescription, Slug,
               command.CategoryId);
                _productRepository.Create(product);
                _productRepository.Save();
                return operation.Succeeded();
       }

      
        public OperationResult Edit(EditProduct command)
        {
            var operation = new OperationResult();
            var product = _productRepository.Get(command.Id);
            if (product == null)
                operation.Failed(ApplicationMessages.RecordNotFound);
            if (_productRepository.Exist(x => x.Name == command.Name && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            var Slug = command.Slug.Slugify();
            product.Edit(command.Description,command.Name, command.Code, command.ShortDescription,
                command.Picture,
                command.PictureAlt, command.PictureTitle, command.Keywords, command.MetaDescription, command.Slug,
                command.CategoryId);
            _productRepository.Save();
            return operation.Succeeded();

        }

        public EditProduct GetDetails(long id)
        {
            return _productRepository.GetDetails(id);
        }

        public List<ProductViewModel> Search(ProductSearchModel searchModel)
        {
            return _productRepository.Search(searchModel);
        }

        public List<ProductViewModel> GetProducts()
        {
            return _productRepository.GetProducts();
        }
   }
}
