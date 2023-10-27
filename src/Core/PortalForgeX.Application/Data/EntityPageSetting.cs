namespace PortalForgeX.Application.Data;

/// <summary>
/// Record for configuring the EntityPage.
/// </summary>
/// <param name="PageSize"></param>
/// <param name="PageIndex"></param>
/// <param name="LastItemCreationTime"></param>
/// <param name="SortField"></param>
/// <param name="SortAsc"></param>
/// <param name="EncodedFilters"></param>
/// <param name="EncodedProjectionFields"></param>
public record EntityPageSetting(
    int PageSize,
    int? PageIndex,
    string? SortField,
    bool? SortAsc = true,
    string? EncodedFilters = null,
    string? EncodedProjectionFields = null
);
