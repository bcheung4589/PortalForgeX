using MediatR;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Domain.Services;
using PortalForgeX.Shared.Features.Tenants;

namespace PortalForgeX.Application.Features.DevJobs;

public record GenerateTenantsRequest(int? Amount) : ICommand<GenerateTenantsResponse>
{
    public GenerateTenantsResponse NewResponse()
        => new();
}

internal sealed class GenerateTenants(ITenantSeeder seeder) : IRequestHandler<GenerateTenantsRequest, GenerateTenantsResponse>
{
    private readonly ISeedService _seeder = seeder;
    private const int DEFAULT_AMOUNT = 100;

    public async Task<GenerateTenantsResponse> Handle(GenerateTenantsRequest request, CancellationToken cancellationToken)
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
