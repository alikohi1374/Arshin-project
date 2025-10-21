using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement.Domain.ProductAttributeAgg
{
    public class ProductAttributeValue
    {
        public long Id { get; private set; }
        public long ProductId { get; private set; }                // FK to Product (existing table)
        public long AttributeDefinitionId { get; private set; }    // FK to ProductAttributeDefinition
        public string Value { get; private set; }                  // store as string, interpret based on DataType
        public DateTime CreatedAt { get; private set; }

        // Navigation
        public ProductAttributeDefinition AttributeDefinition { get; private set; }

        protected ProductAttributeValue() { }

        public ProductAttributeValue(long productId, long attributeDefinitionId, string value)
        {
            ProductId = productId;
            AttributeDefinitionId = attributeDefinitionId;
            Value = value;
            CreatedAt = DateTime.UtcNow;
        }

        public void EditValue(string value)
        {
            Value = value;
        }
    }
}
