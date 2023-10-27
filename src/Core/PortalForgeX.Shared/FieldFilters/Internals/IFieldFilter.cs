namespace PortalForgeX.Shared.FieldFilters.Internals;

/// <summary>
/// Filter entities based on the given field's Value.
/// NULL-values are ignored.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IFieldFilter
{
    /// <summary>
    /// Name of the Filter Type to apply.
    /// </summary>
    string FilterTypeName { get; }

    /// <summary>
    /// The field to apply the filter on.
    /// </summary>
    string Field { get; set; }

    /// <summary>
    /// Determine if this filter should be applied.
    /// </summary>
    bool ShouldApply { get; }

    /// <summary>
    /// Reset the Field to its original values.
    /// </summary>
    void Reset();
}


/// <summary>
/// Filter entities based on the given field's Value.
/// NULL-values are ignored.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public interface IFieldFilter<TValue> : IFieldFilter
{
    /// <summary>
    /// Value to apply the filter for.
    /// </summary>
    TValue? Value { get; set; }
}
