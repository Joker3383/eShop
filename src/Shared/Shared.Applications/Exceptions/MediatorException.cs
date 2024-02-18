namespace Shared.CrudOperations;

public class MediatorException : Exception
{
    public override string Message { get; }

    public MediatorException(string Message)
    {
        this.Message = Message;
    }
}