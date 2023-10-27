namespace PortalForgeX.Shared.FieldFilters;

/// <summary>
/// Internal record used to convert serialized objects back to IFieldFilter objects.
/// </summary>
/// <param name="FilterTypeName"></param>
/// <param name="Field"></param>
/// <param name="Fields">Used in SearchFieldsFilter</param>
/// <param name="Value"></param>
/// <param name="MaxValue"></param>
/// <param name="HasChanged">Used in SwitchFieldFilter</param>
public record FieldFilterRecord(
    string FilterTypeName,
    string Field,
    IEnumerable<string> Fields,
    object? MaxValue = null,
    bool HasChanged = false
)
{
    public object? Value { get; set; }
}
