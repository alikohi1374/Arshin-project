using System.Collections.Generic;

namespace ShopManagement.Domain.ProductAttributeAgg
{
    public class ProductAttributeDefinition
    {
        public long Id { get; private set; }
        public string Name { get; private set; }               // e.g. "Color", "Size"
        public string DataType { get; private set; }           // e.g. "String", "Number", "Boolean", "Enum"
        public bool IsRequired { get; private set; }
        public string AllowedValuesJson { get; private set; }  // optional: for enum/list -> store JSON array of allowed values

        // Navigation
        public ICollection<ProductAttributeValue> AttributeValues { get; private set; }

        protected ProductAttributeDefinition() { }

        public ProductAttributeDefinition(string name, string dataType, bool isRequired = false, string allowedValuesJson = null)
        {
            Name = name;
            DataType = dataType;
            IsRequired = isRequired;
            AllowedValuesJson = allowedValuesJson;
            AttributeValues = new List<ProductAttributeValue>();
        }

        public void Edit(string name, string dataType, bool isRequired, string allowedValuesJson)
        {
            Name = name;
            DataType = dataType;
            IsRequired = isRequired;
            AllowedValuesJson = allowedValuesJson;
        }
    }
}