namespace TestProj.Exceptions;

public class GuidNotFoundException : Exception
{
    public Guid Guid { get; set; }

    public GuidNotFoundException()
    {

    }

    public GuidNotFoundException(string message) : base(message)
    {

    }

    public GuidNotFoundException(string message, Exception inner) : base(message, inner)
    {

    }

    public GuidNotFoundException(string message, Guid guid) : this(message)
    {
        Guid = guid;
    }
}