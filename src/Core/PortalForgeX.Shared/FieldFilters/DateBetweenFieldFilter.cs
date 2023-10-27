using PortalForgeX.Shared.FieldFilters.Internals;

namespace PortalForgeX.Shared.FieldFilters;

/// <summary>
/// Filter entities where the Field's (DateTime) value is between Value and MaxValue.
/// </summary>
public record DateBetweenFieldFilter : IFieldBetweenFilter<DateTime?>
{
    /// <inheritdoc />
    public string FilterTypeName => typeof(DateBetweenFieldFilter).FullName!;

    /// <inheritdoc />
    public string Field { get; set; }

    /// <inheritdoc />
    public DateTime? Value { get; set; }

    /// <inheritdoc />
    public DateTime? MaxValue { get; set; }

    /// <inheritdoc />
    public bool ShouldApply => Value is not null || MaxValue is not null;

    public DateBetweenFieldFilter(string field)
        => Field = field;

    public DateBetweenFieldFilter(FieldFilterRecord record)
    {
        Field = record.Field;

        if (record.Value is DateTime value)
        {
            Value = value;
        }

        if (record.MaxValue is DateTime maxValue)
        {
            MaxValue = maxValue;
        }
    }

    /// <inheritdoc />
    public void Reset()
    {
        Value = null;
        MaxValue = null;
    }
}
