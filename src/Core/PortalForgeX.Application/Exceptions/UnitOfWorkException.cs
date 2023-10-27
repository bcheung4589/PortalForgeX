namespace PortalForgeX.Application.Exceptions;

[Serializable]
public class UnitOfWorkException : ApplicationException
{
    public UnitOfWorkException(string message) : base(message) { }

    public UnitOfWorkException(string message, Exception inner) : base(message, inner) { }
}
