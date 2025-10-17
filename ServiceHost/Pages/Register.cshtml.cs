using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountManagement.Application.Contracts.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class RegisterModel : PageModel
    {
        [TempData]
        public string RegisterMessage { get; set; }


        private readonly IAccountApplication _accountApplication;

        public RegisterModel(IAccountApplication accountApplication)
        {
            _accountApplication = accountApplication;
        }
        public void OnGet()
        {
        }
        public IActionResult OnPostRegister(RegisterAccount command)
        {
            var result = _accountApplication.Register(command);
            if (result.IsSucceeded)
                return RedirectToPage("/Account");
            RegisterMessage = result.Message;
            return RedirectToPage("/Account");
        }
    }
}
