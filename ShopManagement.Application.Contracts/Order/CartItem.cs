namespace ShopManagement.Application.Contracts.Order
{
     public class CartItem
    {

        public long Id { get; set; }
        public string Name { get; set; }
        public double UnitPrice { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public decimal Count { get; set; }
        public double TotalItemPrice { get; set; }
        public bool IsInStock { get; set; }
        public int DiscountRate { get; set; }
        public double DiscountAmount { get; set; }
        public double ItemPayAmount { get; set; }

        public CartItem()
        {
            TotalItemPrice = UnitPrice * (double)Count;
        }
        public void CalculateTotalItemPrice()
        {
            TotalItemPrice = UnitPrice * (double)Count;
        }

    }
}
