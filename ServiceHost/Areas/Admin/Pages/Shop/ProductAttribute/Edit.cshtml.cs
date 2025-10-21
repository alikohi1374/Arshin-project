using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.Application;
using ShopManagement.Application.Contracts.DTOs;

namespace ServiceHost.Areas.Admin.Pages.Shop.ProductAttribute
{
    public class EditModel : PageModel
    {
        private readonly IProductAttributeApplication _attributeApp;

        [BindProperty]
        public EditAttributeDto Attribute { get; set; }

        public EditModel(IProductAttributeApplication attributeApp)
        {
            _attributeApp = attributeApp;
        }

        public async Task<IActionResult> OnGet(long id)
        {
            var all = await _attributeApp.GetAllAttributesAsync();
            var item = all.FirstOrDefault(x => x.Id == id);
            if (item == null) return NotFound();

            Attribute = new EditAttributeDto
            {
                Id = item.Id,
                Name = item.Name,
                DataType = item.DataType,
                IsRequired = item.IsRequired,
                AllowedValuesJson = item.AllowedValuesJson
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _attributeApp.EditAttributeAsync(Attribute);
            return RedirectToPage("./Index");
        }
    }
}

