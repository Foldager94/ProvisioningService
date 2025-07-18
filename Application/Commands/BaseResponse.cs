namespace Application.Commands;

public abstract class BaseResponse(string? message = null, bool isError = false)
{
    public string? Message { get; set; } = message;

    public bool IsError { get; set; } = isError;
}