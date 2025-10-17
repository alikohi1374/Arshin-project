using System;
using System.Collections.Generic;
using System.Linq;
using _01_ArshinQuery.Contracts.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;
using ShopManagement.Application.Contracts.Order;

namespace ServiceHost.Pages
{
    public class CartModel : PageModel
    {
        public List<CartItem> CartItems;
        public const string CookieName = "cart-items";
        private readonly IProductQuery _productQuery;

        public CartModel(IProductQuery productQuery)
        {
            CartItems = new List<CartItem>();
            _productQuery = productQuery;
        }

        public void OnGet()
        {
            var serializer = new JavaScriptSerializer();
            var value = Request.Cookies[CookieName];

            List<CartItem> cartItems = new List<CartItem>();
            if (!string.IsNullOrWhiteSpace(value))
            {
                cartItems = serializer.Deserialize<List<CartItem>>(value);
            }

            // جلوگیری از خطا هنگام حذف آخرین آیتم
            if (cartItems == null || !cartItems.Any())
            {
                CartItems = new List<CartItem>();
                return;
            }

            // ادامه منطق محاسبه
            foreach (var item in cartItems)
            {
                item.CalculateTotalItemPrice();
            }

            CartItems = _productQuery.CheckInventoryStatus(cartItems);
        }


        public IActionResult OnGetRemoveFromCart(long id, decimal count)
        {
            var serializer = new JavaScriptSerializer();
            var value = Request.Cookies[CookieName];

            if (string.IsNullOrWhiteSpace(value))
                return Page(); // کوکی خالی بود

            var cartItems = serializer.Deserialize<List<CartItem>>(value) ?? new List<CartItem>();

            // حذف دقیق فقط همون ردیف
            var itemToRemove = cartItems.FirstOrDefault(x => x.Id == id && Math.Abs(x.Count - count) < 0.001M);
            if (itemToRemove != null)
                cartItems.Remove(itemToRemove);

            // بازنویسی کوکی
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(2),
                Path = "/" // حتماً لازمه برای همه صفحات قابل‌دسترسی بودن کوکی
            };
            Response.Cookies.Append(CookieName, serializer.Serialize(cartItems), options);

            // برای بازسازی صفحه بدون خطا
            CartItems = cartItems;
            return Page();
        }

       


    }
}
