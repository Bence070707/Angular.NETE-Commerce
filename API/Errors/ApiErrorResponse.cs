namespace API.Errors;

public class ApiErrorResponse(int statusCode, string message, string? details)
{
    public int StatusCode { get; set; } = statusCode;
    public string ExceptionMessage { get; set; } = message;
    public string? ExceptionDetails { get; set; } = details;
}
