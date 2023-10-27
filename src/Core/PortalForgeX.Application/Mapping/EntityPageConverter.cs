using AutoMapper;
using PortalForgeX.Application.Data;
using PortalForgeX.Shared.DTOs;

namespace PortalForgeX.Application.Mapping;

public class EntityPageConverter<TEntitySource, TEntityDestination> : ITypeConverter<EntityPage<TEntitySource>, PageModel<TEntityDestination>>
    where TEntitySource : class
    where TEntityDestination : class
{
    public PageModel<TEntityDestination> Convert(EntityPage<TEntitySource> source, PageModel<TEntityDestination> destination, ResolutionContext context)
        => new()
        {
            PageIndex = source.Settings.PageIndex ?? 0,
            PageSize = source.Settings.PageSize,
            SortField = source.Settings.SortField,
            SortAsc = source.Settings.SortAsc ?? true,
            Count = source.Count,
            AppliedFilters = source.AppliedFilters,
            Entities = context.Mapper.Map<IEnumerable<TEntityDestination>>(source.Entities)
        };
}