using System.Collections.Generic;
using ShopManagement.Application.Contracts.Order;

namespace _01_ArshinQuery.Contracts
{
    public interface ICartCalculatorService
    {
        Cart ComputeCart(List<CartItem> cartItems);
    }
}