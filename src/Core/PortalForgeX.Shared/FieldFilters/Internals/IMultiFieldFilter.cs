namespace PortalForgeX.Shared.FieldFilters.Internals;

/// <summary>
/// Filter entities where the field value is present in the IEnumerable Values list.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public interface IMultiFieldFilter<TValue> : IFieldFilter<IList<TValue>>
{
    /// <summary>
    /// Add new value to the values list.
    /// </summary>
    /// <param name="newValue"></param>
    public void Add(TValue newValue);

    /// <summary>
    /// Remove the value from the values list.
    /// </summary>
    /// <param name="value"></param>
    public void Remove(TValue value);

    /// <summary>
    /// Clear the values list.
    /// </summary>
    public void Clear();
}
