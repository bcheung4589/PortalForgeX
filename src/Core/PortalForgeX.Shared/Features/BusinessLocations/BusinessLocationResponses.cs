using PortalForgeX.Shared.DTOs;

namespace PortalForgeX.Shared.Features.BusinessLocations;

public record GetBusinessLocationsResponse : Result<PagedList<BusinessLocationDto>>;

public record GetBusinessLocationByIdResponse : Result<BusinessLocationDto>;

public record CreateBusinessLocationResponse : Result<BusinessLocationDto>;

public record UpdateBusinessLocationResponse : Result<BusinessLocationDto>;

public record DeleteBusinessLocationResponse : Result;
