namespace Shared.CrudOperations;

public class NotFoundException : Exception
{
    public override string Message { get; }

    public NotFoundException(string Message)
    {
        this.Message = Message;
    }
}