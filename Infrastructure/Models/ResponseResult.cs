namespace Infrastructure.Models;

public enum StatusCodes
{
    OK = 200,
    ERROR = 400,
    NOT_FOUND = 404,
    EXISTS = 409
}

public class ResponseResult
{
    public StatusCodes StatusCode { get; set; }
    public string? Message { get; set; }
    public object? Result { get; set; }
}
