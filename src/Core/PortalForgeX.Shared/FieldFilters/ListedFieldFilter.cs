using PortalForgeX.Shared.FieldFilters.Internals;

namespace PortalForgeX.Shared.FieldFilters;

/// <summary>
/// Filter entities where the Field's value is equal-compared to the selected Value.
/// Usage: Dropdownlist, Radiobuttonlist
/// </summary>
public record ListedFieldFilter : IFieldFilter<string>, IFieldOptionsFilter<string>
{
    /// <inheritdoc />
    public string FilterTypeName => typeof(ListedFieldFilter).FullName!;

    /// <inheritdoc />
    public string Field { get; set; }

    /// <inheritdoc />
    public string? Value { get; set; }

    /// <inheritdoc />
    public bool ShouldApply => !string.IsNullOrWhiteSpace(Value);

    /// <inheritdoc />
    public IDictionary<string, string>? Options { get; set; }

    /// <inheritdoc />
    public int CollapsedItemsCount { get; set; }

    public ListedFieldFilter(string field, IDictionary<string, string>? options, int collapsedItemsCount = 0)
    {
        Field = field;
        Options = options ?? new Dictionary<string, string>();
        CollapsedItemsCount = collapsedItemsCount;
    }

    public ListedFieldFilter(FieldFilterRecord record)
    {
        Field = record.Field;

        if (record.Value is string value)
        {
            Value = value;
        }
    }

    /// <inheritdoc />
    public void ValueChanged(string? newValue)
        => Value = newValue;

    /// <inheritdoc />
    public void Reset()
        => Value = default;
}