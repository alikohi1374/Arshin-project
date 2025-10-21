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
    public class CreateModel : PageModel
    {
        private readonly IProductAttributeApplication _attributeApp;

        [BindProperty]
        public CreateAttributeDto Attribute { get; set; }

        public CreateModel(IProductAttributeApplication attributeApp)
        {
            _attributeApp = attributeApp;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _attributeApp.CreateAttributeAsync(Attribute);
            return RedirectToPage("./Index");
        }
    }
}

