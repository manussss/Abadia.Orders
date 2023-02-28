namespace Abadia.Orders.Application.Contracts;

public class ResponseContract
{
    public bool Valid { get; private set; }
    public object? Data { get; private set; }
    public HttpStatusCode ResponseCode { get; private set; }
    public string? Message { get; private set; }
    public IReadOnlyList<string> Errors => _errors;
    private readonly List<string> _errors = new();

    public void AddError(string error) => _errors.Add(error);
    public void SetData(object data) => Data = data;

    public void SetResponse(bool valid = true, HttpStatusCode code = HttpStatusCode.OK, string message = null)
    {
        Valid = valid;
        ResponseCode = code;
        Message = message;
    }
}