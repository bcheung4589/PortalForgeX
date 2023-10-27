using PortalForgeX.Shared.FieldFilters.Internals;

namespace PortalForgeX.Shared.FieldFilters;

/// <summary>
/// Filter entities where the Field's value is present in the Value list.
/// Usage: Checkboxlist
/// </summary>
public record ListedMultiFieldFilter : IMultiFieldFilter<string>, IFieldOptionsFilter<string>
{
    /// <inheritdoc />
    public string FilterTypeName => typeof(ListedMultiFieldFilter).FullName!;

    /// <inheritdoc />
    public string Field { get; set; }

    /// <inheritdoc />
    public IList<string>? Value { get; set; }

    /// <inheritdoc />
    public bool ShouldApply => Value is not null && Value.Any();

    /// <inheritdoc />
    public IDictionary<string, string>? Options { get; set; }

    /// <inheritdoc />
    public int CollapsedItemsCount { get; set; }

    public ListedMultiFieldFilter(string field, IDictionary<string, string>? options, int collapsedItemsCount = 0)
    {
        Field = field;
        Options = options;
        CollapsedItemsCount = collapsedItemsCount;
        Value = new List<string>();
    }

    public ListedMultiFieldFilter(FieldFilterRecord record)
    {
        Field = record.Field;
        Value = record.Value is IList<string> value
            ? Value = value
            : new List<string>();
    }

    /// <summary>
    /// If newValue is null, an new List will be initialized. The Value field may not me null.
    /// </summary>
    /// <param name="newValue"></param>
    public void ValueChanged(IList<string>? newValue)
    {
        if (newValue is null)
        {
            Value = new List<string>();
            return;
        }

        Value = newValue;
    }

    /// <inheritdoc />
    public void Reset()
        => Value = new List<string>();

    /// <inheritdoc />
    public void Add(string newValue)
    {
        if (Value is null)
        {
            return;
        }

        if (Value.Contains(newValue))
        {
            return;
        }

        Value.Add(newValue);
    }

    /// <inheritdoc />
    public void Remove(string value)
    {
        if (Value is null)
        {
            return;
        }

        if (!Value.Contains(value))
        {
            return;
        }

        Value.Remove(value);
    }

    /// <inheritdoc />
    public void Clear()
    {
        if (Value is null)
        {
            return;
        }

        Value.Clear();
    }
}
