using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PortalForgeX.Domain.Entities.Internal;
using PortalForgeX.Shared.Extensions;
using PortalForgeX.Shared.FieldFilters;
using PortalForgeX.Shared.FieldFilters.Internals;
using PortalForgeX.Shared.Extensions;
using System.Linq.Expressions;
using System.Reflection;

namespace PortalForgeX.Application.Data;

/// <summary>
/// Efficiently loads a paged list of entities from the database.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class EntityPage<TEntity>
{
    private static readonly Assembly ASSEMBLY = Assembly.GetAssembly(typeof(IFieldFilter))!;

    /// <summary>
    /// Count is the total amount of entities found.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// The loaded entities for the current page.
    /// </summary>
    public IEnumerable<TEntity> Entities { get; private set; } = null!;

    /// <summary>
    /// Query on which to execute operations.
    /// </summary>
    public IQueryable<TEntity> Query { get; private set; } = null!;

    /// <summary>
    /// The applied filters on the query.
    /// </summary>
    public IList<FieldFilterRecord>? AppliedFilters { get; private set; }

    /// <summary>
    /// The settings for loading the paged entities.
    /// </summary>
    public EntityPageSetting Settings { get; private set; } = null!;

    /// <summary>
    /// Internally create an instance with the specified query as base.
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="lastItemCreationTime"></param>
    private EntityPage(IQueryable<TEntity> query)
        => Query = query;

    /// <summary>
    /// Create an instance with the specified settings.
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="lastItemCreationTime"></param>
    public static EntityPage<TEntity> Create(IQueryable<TEntity> query)
        => new(query);

    /// <summary>
    /// Sort and skip the entities based on the Page settings.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<EntityPage<TEntity>> ApplyAsync(EntityPageSetting settings, CancellationToken cancellationToken = default)
    {
        Settings = settings;

        // apply filters
        ApplyFilters();

        // count before skipping
        Count = await Query.CountAsync(cancellationToken: cancellationToken);

        // skip and sort
        ApplySkipAndSorting();

        // take entities if pagesize is assigned
        if (Settings.PageSize > 0)
        {
            Query = Query.Take(Settings.PageSize);
        }

        return this;
    }

    /// <summary>
    /// Execute the Query and load the results into Entities.
    /// </summary>
    /// <param name="projectionExpression">Execute the query with the specified projection.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<EntityPage<TEntity>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        // use projection if any given
        ApplyFieldProjections();

        // execute the query on the database
        Entities = await Query.ToListAsync(cancellationToken: cancellationToken);

        return this;
    }

    /// <summary>
    /// Decode and deserialize the projection fields to apply them on the Query.
    /// </summary>
    private void ApplyFieldProjections()
    {
        if (string.IsNullOrWhiteSpace(Settings.EncodedProjectionFields))
        {
            return;
        }

        var fieldsList = Settings.EncodedProjectionFields.DecodeFromString<string>();
        if (fieldsList is null || !fieldsList.Any())
        {
            return;
        }

        var parameter = Expression.Parameter(typeof(TEntity), "x");
        var newInstance = Expression.New(typeof(TEntity));
        var bindings = (from field in fieldsList
                        let property = typeof(TEntity).GetProperty(field)
                        select Expression.Bind(property, Expression.Property(parameter, property))).ToList();
        var memberInit = Expression.MemberInit(newInstance, bindings);

        Query = Query.Select(Expression.Lambda<Func<TEntity, TEntity>>(memberInit, parameter));
    }

    /// <summary>
    /// Decode and deserialize the filters to apply them on the Query.
    /// </summary>
    private void ApplyFilters()
    {
        if (string.IsNullOrWhiteSpace(Settings.EncodedFilters))
        {
            return;
        }

        var filterTemplates = Settings.EncodedFilters.DecodeFromString<FieldFilterRecord>();
        if (filterTemplates is null)
        {
            return;
        }

        Query = Query.Where(ConvertToFilters(filterTemplates));
    }

    /// <summary>
    /// Skip or filters out the previous records based on the Settings
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    private void ApplySkipAndSorting()
    {
        var hasFieldSorting = !string.IsNullOrWhiteSpace(Settings.SortField);

        // sort on custom field, if assigned
        if (hasFieldSorting)
        {
            ApplySortingByField();
        }

        // order the entities before taking based on IHasCreationTime.CreationTime
        if (typeof(TEntity).GetInterfaces().Contains(typeof(IHasCreationTime)))
        {
            ApplySortingByCreationTime(hasFieldSorting);
        }

        var skipAmount = (Settings.PageIndex ?? 0) * Settings.PageSize;
        Query = Query.Skip(skipAmount);
    }

    /// <summary>
    /// Order the query based on the custom field.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    private void ApplySortingByField()
        => Query = Settings.SortAsc is true
            ? Query.OrderBy(Settings.SortField!)
            : Query.OrderByDescending(Settings.SortField!);

    /// <summary>
    /// Order the query by based on the CreationTime.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="hasFieldSorting"></param>
    /// <returns></returns>
    private void ApplySortingByCreationTime(bool hasFieldSorting)
        => Query = Settings.SortAsc is false
            ? !hasFieldSorting // order by asc
                ? Query.OrderByDescending(x => (x as IHasCreationTime)!.CreationTime)
                : ((IOrderedQueryable<TEntity>)Query).ThenByDescending(x => (x as IHasCreationTime)!.CreationTime)
            : !hasFieldSorting // order by desc
                ? Query.OrderBy(x => (x as IHasCreationTime)!.CreationTime)
                : ((IOrderedQueryable<TEntity>)Query).ThenBy(x => (x as IHasCreationTime)!.CreationTime);

    /// <summary>
    /// Convert FieldFilterRecord to IFieldFilter instances.
    /// </summary>
    /// <param name="filterTemplates"></param>
    /// <returns></returns>
    private List<IFieldFilter> ConvertToFilters(IEnumerable<FieldFilterRecord> filterTemplates)
    {
        AppliedFilters = new List<FieldFilterRecord>();

        var filters = new List<IFieldFilter>();
        foreach (var filterTemplate in filterTemplates)
        {
            var type = ASSEMBLY?.GetType(filterTemplate.FilterTypeName);
            if (type is null)
            {
                continue;
            }

            var instance = Activator.CreateInstance(type, filterTemplate);
            if (instance is null)
            {
                continue;
            }

            if (instance is not IFieldFilter fieldFilter)
            {
                continue;
            }

            if (fieldFilter is IMultiFieldFilter<string> multiValuedFilter && filterTemplate.Value is not null)
            {
                if (filterTemplate.Value is not JArray valueAsJArray)
                {
                    continue;
                }

                var valueAsStringArray = valueAsJArray.ToObject<string[]>();
                if (valueAsStringArray is null)
                {
                    continue;
                }

                multiValuedFilter.Value = new List<string>(valueAsStringArray);
                filterTemplate.Value = multiValuedFilter.Value; // reset
            }

            if (!fieldFilter.ShouldApply)
            {
                continue;
            }

            filters.Add(fieldFilter);
            AppliedFilters.Add(filterTemplate);
        }

        return filters;
    }
}
