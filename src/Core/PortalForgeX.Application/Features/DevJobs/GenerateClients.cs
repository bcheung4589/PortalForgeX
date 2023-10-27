using MediatR;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Domain.Services;
using PortalForgeX.Shared.Features.Clients;

namespace PortalForgeX.Application.Features.DevJobs;

public record GenerateClientsRequest(int? Amount) : ICommand<GenerateClientsResponse>
{
    public GenerateClientsResponse NewResponse()
        => new();
}

internal sealed class GenerateClientsHandler : IRequestHandler<GenerateClientsRequest, GenerateClientsResponse>
{
    private readonly ISeedService _seeder;
    private const int DEFAULT_AMOUNT = 2000;

    public GenerateClientsHandler(IClientSeeder seeder)
    {
        _seeder = seeder;
    }

    public async Task<GenerateClientsResponse> Handle(GenerateClientsRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        // work
        var resultsCount = await _seeder.ExecuteAsync(request.Amount ?? DEFAULT_AMOUNT, cancellationToken: cancellationToken);

        // process
        response.SetSuccess(resultsCount);

        // return
        return response;
    }
}
