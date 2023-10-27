using System.Text.Json.Serialization;

namespace PortalForgeX.Shared.FieldFilters.Internals;

public interface IFieldOptionsFilter<TKey>
{
    /// <summary>
    /// The list of options of which values can be selected.
    /// The Dictionary Key expects the value while the Dictionairy Value expects the display text for the Option.
    /// </summary>
    [JsonIgnore]
    IDictionary<TKey, string>? Options { get; set; }

    /// <summary>
    /// Amount of items to display in collapsed mode. Used for Checkboxlist and Radiobuttonlist.
    /// </summary>
    [JsonIgnore]
    public int CollapsedItemsCount { get; set; }
}
