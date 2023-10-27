using PortalForgeX.Shared.FieldFilters.Internals;

namespace PortalForgeX.Shared.FieldFilters;

/// <summary>
/// Filter entities where the Field's (bool) value is equal-compared to the given Value.
/// </summary>
public record SwitchFieldFilter : IFieldFilter<bool>
{
    /// <inheritdoc />
    public string FilterTypeName => typeof(SwitchFieldFilter).FullName!;

    /// <inheritdoc />
    public string Field { get; set; }

    /// <inheritdoc />
    public bool Value { get; set; }

    /// <inheritdoc />
    public bool ShouldApply => HasChanged;

    /// <summary>
    /// Indicates if the value has been changed since init.
    /// </summary>
    public bool HasChanged { get; set; }

    public SwitchFieldFilter(string field)
        => Field = field;

    public SwitchFieldFilter(FieldFilterRecord record)
    {
        Field = record.Field;

        if (record.Value is bool value)
        {
            Value = value;
        }

        HasChanged = record.HasChanged;
    }

    /// <inheritdoc />
    public void Reset()
    {
        Value = false;
        HasChanged = false;
    }
}