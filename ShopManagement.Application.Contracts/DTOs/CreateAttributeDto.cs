using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement.Application.Contracts.DTOs
{
    public class CreateAttributeDto
    {
        public string Name { get; set; }
        public string DataType { get; set; }  // "String","Number","Boolean","Enum"
        public bool IsRequired { get; set; }
        public string AllowedValuesJson { get; set; } // optional: JSON array string for enums
    }

    public class EditAttributeDto : CreateAttributeDto
    {
        public long Id { get; set; }
    }

    public class AttributeValueDto
    {
        public long ProductId { get; set; }
        public long AttributeDefinitionId { get; set; }
        public string Value { get; set; }
    }

    public class AttributeDefinitionDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public bool IsRequired { get; set; }
        public string AllowedValuesJson { get; set; }
    }
}
