using System.Collections.Generic;
using _0_Framework.Application;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Domain.ProductCategoryAgg;

namespace ShopManagement.Application
{
    public class ProductCategoryApplication: IProductCategoryApplication
    {
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoryApplication(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        public OperationResult Create(CreateProductCategory command)
        {
            var Operation = new OperationResult();
            if (_productCategoryRepository.Exist(x=>x.Name==command.Name))
                return Operation.Failed("امکان ثبت دسته بندی تکراری وجود ندارد");
            var slug = command.Slug.Slugify();
            var productcategory =new ProductCategory(command.Name,command.Description,command.Picture
           , command.Description,command.PictureAlt,command.Keywords,command.PictureTitle, slug);
             _productCategoryRepository.Create( productcategory);
             _productCategoryRepository.Save();

             return Operation.Succeeded("");
        }

        public OperationResult Edit(EditProductCategory command)
        {
            var Operation = new OperationResult();
            var productCategory = _productCategoryRepository.Get(command.Id);
            if (productCategory == null)
                return Operation.Failed("رکورد با اطلاعات درخوایت شده یافت نشد ، لطفا مجدد تلاش کنبد.");

            if (_productCategoryRepository.Exist(x => x.Name == command.Name && x.Id != command.Id))
                return Operation.Failed("یک دسته بندی با نام وارد شده موجود می باشد،لطفا مجدد تلاش کنید.");

            var slug = command.Slug.Slugify();
            productCategory.Edit(command.Name,command.Description,command.Picture,command.PictureAlt,command.PictureTitle,command.Keywords,command.MetaDescription,slug);
            _productCategoryRepository.Save();
            return Operation.Succeeded();
        }

        public EditProductCategory GetDetails(long id)
        {
            return _productCategoryRepository.GetDetails(id);
        }

        public List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel)
        {
            return _productCategoryRepository.Search(searchModel);
        }
    }
}
