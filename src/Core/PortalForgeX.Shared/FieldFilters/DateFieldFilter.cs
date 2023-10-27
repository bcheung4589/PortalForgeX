using PortalForgeX.Shared.FieldFilters.Internals;

namespace PortalForgeX.Shared.FieldFilters;

/// <summary>
/// Filter entities where the Field's (DateTime) value is equal-compared to the given Value.
/// </summary>
public record DateFieldFilter : IFieldFilter<DateTime?>
{
    /// <inheritdoc />
    public string FilterTypeName => typeof(DateFieldFilter).FullName!;

    /// <inheritdoc />
    public string Field { get; set; }

    /// <inheritdoc />
    public DateTime? Value { get; set; }

    /// <inheritdoc />
    public bool ShouldApply => Value is not null;

    public DateFieldFilter(string field)
        => Field = field;

    public DateFieldFilter(FieldFilterRecord record)
    {
        Field = record.Field;

        if (record.Value is DateTime value)
        {
            Value = value;
        }
    }

    /// <inheritdoc />
    public void ValueChanged(DateTime? newValue)
        => Value = newValue;

    /// <inheritdoc />
    public void Reset()
        => Value = null;
}
