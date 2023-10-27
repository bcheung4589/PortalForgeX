namespace PortalForgeX.Shared.FieldFilters.Internals;

/// <summary>
/// Filter entities that are between the Value and MaxValue.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public interface IFieldBetweenFilter<TValue> : IFieldFilter<TValue>
{
    /// <summary>
    /// Upperbound to which the value can be.
    /// </summary>
    public TValue? MaxValue { get; set; }
}
