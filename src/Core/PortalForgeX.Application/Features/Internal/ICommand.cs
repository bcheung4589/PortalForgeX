using MediatR;
using PortalForgeX.Shared;

namespace PortalForgeX.Application.Features.Internal;

/// <summary>
/// ICommand interface for exposing a NewResponse-method to build a appropiate Response.
/// </summary>
public interface ICommand : ICommand<Result>
{
}

/// <summary>
/// ICommand interface for exposing a generic NewResponse-method to build a appropiate Response.
/// </summary>
public interface ICommand<T> : IRequest<T>
{
    T NewResponse();
}
