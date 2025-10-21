using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.Application;
using ShopManagement.Application.Contracts.DTOs;

namespace ServiceHost.Areas.Admin.Pages.Shop.ProductAttribute
{
    public class IndexModel : PageModel
    {
        private readonly IProductAttributeApplication _attributeApp;

        public List<AttributeDefinitionDto> Attributes { get; set; }

        public IndexModel(IProductAttributeApplication attributeApp)
        {
            _attributeApp = attributeApp;
        }

        public async Task OnGet()
        {
            Attributes = await _attributeApp.GetAllAttributesAsync();
        }
    }
}

