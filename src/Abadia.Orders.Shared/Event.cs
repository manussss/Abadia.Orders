namespace Abadia.Orders.Shared;

public abstract class Event
{
    public DateTime CreatedAt { get; private set; }
    public string EventName { get; private set; }

    protected Event()
    {
        CreatedAt = DateTime.Now;
        EventName = GetType().Name;
    }
}