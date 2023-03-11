namespace Abadia.Orders.Application.Queries.ViewModels;

public class OrdersDetailsViewModel
{
    public Guid Id { get; set; }
    public string Status { get; set; }
    public decimal TotalValue { get; set; }
    //public IEnumerable<OrderItemDetailsViewModel> Items { get; set; }
}

//public class OrderItemDetailsViewModel
//{
//    public Guid Id { get; set; }
//    public string Product { get; set; }
//    public decimal TaxValue { get; set; }
//    public DateTime DeliveryDate { get; set; }
//}