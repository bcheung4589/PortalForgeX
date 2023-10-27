using PortalForgeX.Shared.FieldFilters.Internals;

namespace PortalForgeX.Shared.FieldFilters;

/// <summary>
/// Filter entities where the Field's value is equal-compared to the given Value.
/// Using % in the Value will execute the like-comparison.
/// </summary>
/// <example>%search => StartsWith, search% => EndsWith, %search% => Contains</example>
public record TextFieldFilter : IFieldFilter<string?>
{
    /// <inheritdoc />
    public string FilterTypeName => typeof(TextFieldFilter).FullName!;

    /// <inheritdoc />
    public string Field { get; set; }

    /// <inheritdoc />
    public string? Value { get; set; }

    /// <inheritdoc />
    public bool ShouldApply => !string.IsNullOrWhiteSpace(Value);

    public TextFieldFilter(string field)
        => Field = field;

    public TextFieldFilter(FieldFilterRecord record)
    {
        Field = record.Field;

        if (record.Value is string value)
        {
            Value = value;
        }
    }

    /// <inheritdoc />
    public void Reset()
        => Value = null;
}
