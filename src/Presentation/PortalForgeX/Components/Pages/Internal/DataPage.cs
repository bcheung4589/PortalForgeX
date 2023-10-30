using PortalForgeX.Shared.DTOs;
using PortalForgeX.Shared.FieldFilters;
using System.Text.Json;

namespace PortalForgeX.Components.Pages.Internal;

public abstract class DataPage<TModel> : PageBase
    where TModel : class
{
    /// <summary>
    /// Datasource that will be used.
    /// </summary>
    protected PagedList<TModel>? DataSource { get; set; }

    /// <summary>
    /// Current page when retrieving data.
    /// </summary>
    protected int PageIndex { get; set; }

    /// <summary>
    /// The size per page when retrieving data. Default: 25.
    /// </summary>
    protected int PageSize { get; set; } = 25;

    /// <summary>
    /// The field on wich the data needs to be sorted. (default: asc)
    /// </summary>
    protected string? SortField { get; set; }

    /// <summary>
    /// Defines the direction the data will be sorted. 
    /// Descending on false, else Ascending.
    /// </summary>
    protected bool? SortAsc { get; set; }

    /// <summary>
    /// Get the applied filter for the specified field.
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    protected FieldFilterRecord? GetAppliedFilter(string field)
        => DataSource?.AppliedFilters?.FirstOrDefault(x => x.Field == field);

    /// <summary>
    /// Get the applied filter value as IList for the specified field.
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    protected IList<TValue>? GetAppliedFilterValueAsList<TValue>(string field)
    {
        var record = GetAppliedFilter(field);
        if (record is null || record.Value is null)
        {
            return null;
        }

        if (record.Value is not JsonElement valueElement)
        {
            return null;
        }

        var valueAsList = valueElement.Deserialize<IEnumerable<TValue>>();
        if (valueAsList is null)
        {
            return null;
        }

        return new List<TValue>(valueAsList!);
    }

    /// <summary>
    /// Reset the sorting.
    /// </summary>
    /// <returns></returns>
    protected async Task ResetSorting()
    {
        SortField = null;
        SortAsc = null;

        await InitDataSourcesAsync();
    }

    /// <summary>
    /// Changes the page and update the datasource.
    /// </summary>
    /// <param name="newPage"></param>
    /// <returns></returns>
    protected async Task DataPageChanged(int newPage)
    {
        PageIndex = newPage;

        await InitDataSourcesAsync();
    }

    /// <summary>
    /// Change the page size and update the datasource.
    /// </summary>
    /// <param name="newPageSize"></param>
    /// <returns></returns>
    protected async Task DataPageSizeChanged(int newPageSize)
    {
        PageSize = newPageSize;

        await InitDataSourcesAsync();
    }

    /// <summary>
    /// Change the sorting field and update the datasource.
    /// </summary>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    protected async Task DataSortChanged(string fieldName)
    {
        // toggle direction only if the fieldname is the same
        if (SortField == fieldName)
        {
            SortAsc = SortAsc is null ? true : !SortAsc;
        }

        SortField = fieldName;

        await InitDataSourcesAsync();
    }
}
