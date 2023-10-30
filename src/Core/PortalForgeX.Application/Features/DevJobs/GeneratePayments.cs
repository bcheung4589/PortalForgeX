using MediatR;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Domain.Services;
using PortalForgeX.Shared.Features.Payments;

namespace PortalForgeX.Application.Features.DevJobs;

public record GeneratePaymentsRequest(int? Amount) : ICommand<GeneratePaymentsResponse>
{
    public GeneratePaymentsResponse NewResponse()
        => new();
}

internal sealed class GeneratePaymentsHandler(IPaymentSeeder seeder) : IRequestHandler<GeneratePaymentsRequest, GeneratePaymentsResponse>
{
    private readonly ISeedService _seeder = seeder;
    private const int DEFAULT_AMOUNT = 10000;

    public async Task<GeneratePaymentsResponse> Handle(GeneratePaymentsRequest request, CancellationToken cancellationToken)
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
