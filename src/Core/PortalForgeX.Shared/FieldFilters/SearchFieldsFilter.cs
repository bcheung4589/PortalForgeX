using PortalForgeX.Shared.FieldFilters.Internals;

namespace PortalForgeX.Shared.FieldFilters;

/// <summary>
/// Filter entities where the Fields value is like-compared to the selected Value.
/// Using % in the Value will execute the like-comparison.
/// </summary>
public record SearchFieldsFilter : IFieldFilter<string?>
{
    /// <inheritdoc />
    public string FilterTypeName => typeof(SearchFieldsFilter).FullName!;

    /// <inheritdoc />
    public string Field { get; set; }

    /// <summary>
    /// For searching in multiple fields, use the Fields property.
    /// </summary>
    public IEnumerable<string> Fields { get; set; }

    /// <inheritdoc />
    public string? Value { get; set; }

    /// <inheritdoc />
    public bool ShouldApply => Value is not null && Fields.Any();

    public SearchFieldsFilter(IEnumerable<string> fields)
    {
        Fields = fields;
        Field = Fields.First();
    }

    public SearchFieldsFilter(FieldFilterRecord record)
    {
        Fields = record.Fields;
        Field = Fields.First();

        if (record.Value is string value)
        {
            Value = value;
        }
    }

    public void Reset()
        => Value = null;
}
