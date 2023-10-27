using PortalForgeX.Shared.FieldFilters.Internals;

namespace PortalForgeX.Shared.FieldFilters;

/// <summary>
/// Filter entities where the Field's (Numeric) value is between Value is between MaxValue.
/// </summary>
public record NumberBetweenFieldFilter : IFieldBetweenFilter<string?>
{
    /// <inheritdoc />
    public string FilterTypeName => typeof(NumberBetweenFieldFilter).FullName!;

    /// <inheritdoc />
    public string Field { get; set; }

    /// <inheritdoc />
    public string? Value { get; set; }

    /// <inheritdoc />
    public string? MaxValue { get; set; }

    /// <inheritdoc />
    public bool ShouldApply => !string.IsNullOrWhiteSpace(Value) || !string.IsNullOrWhiteSpace(MaxValue);

    public NumberBetweenFieldFilter(string field)
        => Field = field;

    public NumberBetweenFieldFilter(FieldFilterRecord record)
    {
        Field = record.Field;

        if (record.Value is string value)
        {
            Value = value;
        }

        if (record.MaxValue is string maxValue)
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
