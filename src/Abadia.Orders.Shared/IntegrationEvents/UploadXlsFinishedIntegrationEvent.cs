namespace Abadia.Orders.Shared.IntegrationEvents;

public class UploadXlsFinishedIntegrationEvent : Event
{
    public Guid OrderUploadId { get; set; }

    public UploadXlsFinishedIntegrationEvent(Guid orderUploadId)
    {
        OrderUploadId = orderUploadId;
    }

    //TODO: add the rest of the information
}