using PortalForgeX.Shared.FieldFilters.Internals;

namespace PortalForgeX.Shared.FieldFilters;

public record AutoCompleteFieldFilter : TextFieldFilter, IFieldOptionsFilter<string>
{
    /// <inheritdoc />
    public new string FilterTypeName => typeof(AutoCompleteFieldFilter).FullName!;

    /// <inheritdoc />
    public IDictionary<string, string>? Options { get; set; }

    /// <inheritdoc />
    public int CollapsedItemsCount { get; set; }

    /// <summary>
    /// The text used to represent the Value.
    /// </summary>
    public string? DisplayValue { get; set; }

    public AutoCompleteFieldFilter(string field)
        : base(field)
    {
    }

    public AutoCompleteFieldFilter(FieldFilterRecord record)
        : base(record)
    {
    }

    /// <inheritdoc />
    public new void Reset()
    {
        Value = null;
        DisplayValue = null;
    }
}
