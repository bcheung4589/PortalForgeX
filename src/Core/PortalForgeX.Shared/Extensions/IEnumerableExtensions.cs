using Newtonsoft.Json;
using PortalForgeX.Shared.Extensions;
using System.Text;

namespace PortalForgeX.Shared.Extensions;

public static class IEnumerableExtensions
{
    /// <summary>
    /// Parse a string list to a list with T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="searchList"></param>
    /// <returns></returns>
    public static IEnumerable<T?> ParseList<T>(this IEnumerable<string> source, out Type returnType)
    {
        var parseMethod = typeof(T).GetMethod("TryParse", new[] { typeof(string), typeof(T).MakeByRefType() });

        if (parseMethod == null)
        {
            returnType = typeof(List<T>);
            return new List<T>();
        }

        var parsedList = source
            .Select(searchItem =>
            {
                T resultValue = default!; // placeholder for out parameter
                var parameters = new object[] { searchItem, resultValue };
                if ((bool?)parseMethod.Invoke(null, parameters) == true)
                {
                    return (T)parameters[1];
                }
                return default; // Return default value if parsing fails
            })
            .Where(resultValue => !EqualityComparer<T>.Default.Equals(resultValue, default))
            .ToList();

        returnType = parsedList.GetType();
        return parsedList;
    }

    /// <summary>
    /// Serialize and Base64 Encode the IEnumerable with its values.
    /// Decoding: Use string.DecodeFromString<typeparamref name="T"/>() to revert back to IEnumerable<typeparamref name="T"/>.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string? ToEncodedString<T>(this IEnumerable<T> source)
        => source is not null
            ? Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(source)))
            : null;
}
