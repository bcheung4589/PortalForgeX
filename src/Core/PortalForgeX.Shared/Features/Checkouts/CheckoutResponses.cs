namespace PortalForgeX.Shared.Features.Checkouts;

public record GetCheckoutByIdResponse : Result<CheckoutDto>;

public record CreateCheckoutResponse : Result<CheckoutDto>;

public record UpdateCheckoutResponse : Result<CheckoutDto>;

public record DeleteCheckoutResponse : Result;
