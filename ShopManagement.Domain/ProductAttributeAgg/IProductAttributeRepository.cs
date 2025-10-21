using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement.Domain.ProductAttributeAgg
{
    public interface IProductAttributeRepository
    {
        // Async
        Task<long> CreateDefinitionAsync(ProductAttributeDefinition entity);
        Task EditDefinitionAsync(ProductAttributeDefinition entity);
        Task<ProductAttributeDefinition> GetDefinitionByIdAsync(long id);
        Task<List<ProductAttributeDefinition>> GetAllDefinitionsAsync();
        Task SetProductAttributeValueAsync(ProductAttributeValue entity);
        Task<List<ProductAttributeValue>> GetAttributesByProductIdAsync(long productId);

        // Sync
        List<ProductAttributeDefinition> GetAllDefinitions();
        void SetProductAttributes(List<ProductAttributeValue> entities);
        List<ProductAttributeValue> GetAttributesByProductId(long productId);

    }
}
