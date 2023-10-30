using PortalForgeX.Shared.FieldFilters;

namespace PortalForgeX.Shared.DTOs;

public record PagedList<TEntity> where TEntity : class
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int Count { get; set; }

    public string? SortField { get; set; }
    public bool SortAsc { get; set; }

    public IEnumerable<FieldFilterRecord>? AppliedFilters { get; set; }
    public IEnumerable<TEntity>? Entities { get; set; } = null!;
    public int PageCount => Entities?.Count() ?? 0;
}
