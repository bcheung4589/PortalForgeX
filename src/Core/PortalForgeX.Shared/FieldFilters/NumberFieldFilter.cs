using PortalForgeX.Shared.FieldFilters.Internals;

namespace PortalForgeX.Shared.FieldFilters;

/// <summary>
/// Filter entities where the Field's value is equal-compared to the given Value.
/// </summary>
public record class NumberFieldFilter : IFieldFilter<string?>
{
    /// <inheritdoc />
    public string FilterTypeName => typeof(NumberFieldFilter).FullName!;

    /// <inheritdoc />
    public string Field { get; set; }

    /// <inheritdoc />
    public string? Value { get; set; }

    /// <inheritdoc />
    public bool ShouldApply => !string.IsNullOrWhiteSpace(Value);

    public NumberFieldFilter(string field)
        => Field = field;

    public NumberFieldFilter(FieldFilterRecord record)
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
        => Value = null;
}
