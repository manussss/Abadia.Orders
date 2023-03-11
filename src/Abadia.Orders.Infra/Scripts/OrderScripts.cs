namespace Abadia.Orders.Infra.Scripts;

public static class OrderScripts
{
    public const string GetOrdersDetailsByContractId =
        @"
            SELECT
				O.Id,
				O.OrderStatus Status,
				O.TotalValue
			FROM
				[Order] O
			WHERE
				O.ContractId = @Id
        ";

    // public const string GetOrdersDetailsByContractId =
    //     @"
    //         SELECT
    //	O.Id,
    //	O.OrderStatus Status,
    //	O.TotalValue,
    //	OI.Id,
    //	OI.Product,
    //	OI.TaxValue,
    //	OI.DeliveryDate
    //FROM
    //	[Order] O
    //LEFT JOIN OrderItem OI ON
    //	O.Id = OI.OrderId
    //WHERE
    //	O.ContractId = @Id
    //     ";
}