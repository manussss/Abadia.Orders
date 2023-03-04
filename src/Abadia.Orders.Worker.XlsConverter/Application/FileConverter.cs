using Abadia.Orders.Domain.OrdersAggregate;
using Abadia.Orders.Domain.OrdersAggregate.Enums;
using Abadia.Orders.Domain.Services;
using ClosedXML.Excel;

namespace Abadia.Orders.Worker.XlsConverter.Application;

public class FileConverter : IFileConverter
{
    public Order Order { get; private set; }
    private List<OrderItem> _orderItems = new();

    public Order ConvertFile(byte[] file)
    {
        try
        {
            using var stream = new MemoryStream(file);
            using var workbook = new XLWorkbook(stream);

            return GetOrder(workbook.Worksheet(1));
        }
        catch (Exception ex)
        {
            //TODO logs

            return null;
        }
    }

    //TODO refactor
    private Order GetOrder(IXLWorksheet sheet)
    {
        var lastRow = sheet.LastCellUsed().Address.RowNumber;
        var contractId = sheet.Range("A2:A2").Rows().FirstOrDefault()?.Cell(1).GetString(); //TODO sempre o mesmo numero de contrato?
        var order = new Order(contractId) { CreatedAt = DateTime.Now, CreatedBy = "system-user" };

        foreach (var row in sheet.Range($"A2:A{lastRow}").Rows())
        {
            var address = new Address(row.Cell(6).GetString(), row.Cell(7).GetValue<int>(), row.Cell(8).GetString(), row.Cell(10).GetString(), row.Cell(9).GetString()) 
            { 
                CreatedAt = DateTime.Now, 
                CreatedBy = "system-user" 
            };

            var orderItem = new OrderItem(row.Cell(4).GetDateTime(), row.Cell(3).GetValue<EnumOrderItemProduct>(), row.Cell(2).GetString()) 
            { 
                CreatedAt = DateTime.Now, 
                CreatedBy = "system-user" 
            };

            orderItem.AddAddress(address);
            orderItem.AddOrderId(order.Id);

            _orderItems.Add(orderItem);
        }

        order.AddOrderItem(_orderItems);

        return order;
    }
}