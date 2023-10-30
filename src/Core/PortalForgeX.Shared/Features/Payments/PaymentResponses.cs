using PortalForgeX.Shared.DTOs;

namespace PortalForgeX.Shared.Features.Payments;

public record GetPaymentsResponse : Result<PagedList<PaymentDto>>;

public record GetPaymentByIdResponse : Result<PaymentDto>;

public record CreatePaymentResponse : Result<PaymentDto>;

public record GeneratePaymentsResponse : Result<int>;

public record DeletePaymentResponse : Result;

public record UpdatePaymentResponse : Result<PaymentDto>;
