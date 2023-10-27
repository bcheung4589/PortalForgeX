using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace PortalForgeX.Shared.Extensions;

public static partial class StringExtensions
{
    #region String Cleanups

    /// <summary>
    /// Only accepts numbers, a-z upper- and lowercase, dash '-' and underscore '_'
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string SanitizeAlphaNum(this string source)
        => new(source.Where(x =>
            (x >= '0' && x <= '9') ||
            (x >= 'A' && x <= 'Z') ||
            (x >= 'a' && x <= 'z') ||
            x == '-' || x == '_'
        ).ToArray());

    /// <summary>
    /// Recursively remove all multiple forward slashes '//' from a string.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string RemoveDoubleSlashes(this string source)
        => MultipleSlashesRegex().Replace(source, "/");

    /// <summary>
    /// Regex to match multiple slashes, eg // or ///.
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex(@"\/+")]
    private static partial Regex MultipleSlashesRegex();

    #endregion String Cleanups

    #region String Template Formatting

    /// <summary>
    /// Format the source template. The target model will be scanned based on the tags and will be formatted appropriately.
    /// </summary>
    /// <typeparam name="T">Class to scan for formatting.</typeparam>
    /// <param name="source">Template with tags.</param>
    /// <param name="target">The Model to scan.</param>
    /// <param name="startTagPattern"></param>
    /// <param name="endTagPattern"></param>
    /// <returns>String with the model processed into the template.</returns>
    public static string FormatTemplate<T>(this string source, T target, string startTagPattern = "[", string endTagPattern = "]")
        where T : class
    {
        /**
		 * Dynamically find wich tags are used in the source template.
		 */
        var foundTags = source.ExtractTags(startTagPattern, endTagPattern);

        /**
		 * Get the values in the target object 
		 * where the tags are treated as properties of the target object.
		 */
        var tagValues = GetValuesFromObject(foundTags, target);

        /**
		 * Remove tags that dont have assigned values.
		 */
        foundTags = foundTags.Where(tagValues.ContainsKey);

        /**
		 * Convert the source template to a suitable string for string.Format()
		 */
        var formatTemplate = source.ConvertTagsForFormatting(foundTags, startTagPattern, endTagPattern);

        /**
		 * Get the tag values as suitable array for string.Format()
		 */
        var args = foundTags.Select(tag => tagValues[tag]);

        return string.Format(formatTemplate.ToString(), args.ToArray());
    }

    /// <summary>
    /// Convert the source string with tags to be usable in string.Format().
    /// </summary>
    /// <param name="source"></param>
    /// <param name="foundTags"></param>
    /// <param name="startTagPattern"></param>
    /// <param name="endTagPattern"></param>
    /// <returns>StringBuilder that holds a string suitable for string.Format()</returns>
    private static StringBuilder ConvertTagsForFormatting(this string source, IEnumerable<string> foundTags, string startTagPattern, string endTagPattern)
    {
        var index = 0;
        var formattedSource = new StringBuilder(source);
        foreach (var tag in foundTags)
        {
            string? formatting = null;
            if (tag.Contains(':'))
            {
                formatting = tag.Split(':')[1];
            }

            formattedSource.Replace($"{startTagPattern}{tag}{endTagPattern}", formatting is null ? $"{{{index}}}" : $"{{{index}:{formatting}}}");

            index++;
        }

        return formattedSource;
    }

    /// <summary>
    /// Find and extract the tags found in the source string.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="startTagPattern"></param>
    /// <param name="endTagPattern"></param>
    /// <returns>Unique list of tags.</returns>
    private static IEnumerable<string> ExtractTags(this string source, string startTagPattern, string endTagPattern)
    {
        var foundTags = new List<string>();
        int startIndex = 0;
        while (startIndex < source.Length)
        {
            int openBracketIndex = source.IndexOf(startTagPattern, startIndex);
            int closeBracketIndex = source.IndexOf(endTagPattern, openBracketIndex + 1);

            if (openBracketIndex != -1 && closeBracketIndex != -1)
            {
                var tag = source.Substring(openBracketIndex + 1, closeBracketIndex - openBracketIndex - 1);
                if (!foundTags.Contains(tag))
                {
                    foundTags.Add(tag);
                }

                startIndex = closeBracketIndex + 1;
            }
            else
            {
                break;
            }
        }

        return foundTags;
    }

    /// <summary>
    /// Get the values in the target model to replace the tags with.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="paths"></param>
    /// <param name="target"></param>
    /// <returns>Dictionary where Key => Tag and Value => Content</returns>
    private static IDictionary<string, object> GetValuesFromObject<T>(IEnumerable<string> paths, T target)
    {
        var foundValues = new Dictionary<string, object>();
        if (target is null)
        {
            return foundValues;
        }

        var targetType = typeof(T);
        foreach (var path in paths)
        {
            var currentPropertyName = path;

            // strip formatting
            if (currentPropertyName.Contains(':'))
            {
                currentPropertyName = currentPropertyName.Split(':')[0];
            }

            if (!currentPropertyName.Contains('.'))
            {
                var property = targetType.GetProperty(currentPropertyName);
                if (property == null)
                {
                    continue;
                }

                var propertyValue = property.GetValue(target);

                foundValues[path] = propertyValue ?? "";
            }
            else
            {
                var propertyValue = GetNestedPropertyValue(target, currentPropertyName);

                foundValues[path] = propertyValue ?? "";
            }
        }

        return foundValues;
    }

    /// <summary>
    /// Get the value of the property path in the source.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="propertyPath"></param>
    /// <returns></returns>
    private static object? GetNestedPropertyValue(object source, string propertyPath)
    {
        if (source == null)
        {
            return null;
        }

        var properties = propertyPath.Split('.');
        var value = source;

        foreach (var property in properties)
        {
            var propertyInfo = value.GetType().GetProperty(property);

            if (propertyInfo == null)
            {
                return null;
            }

            value = propertyInfo.GetValue(value);

            if (value == null)
            {
                return null;
            }
        }

        return value;
    }

    #endregion String Template Formatting

    /// <summary>
    /// Decode an encoded IEnumerable<typeparamref name="T"/> string back.
    /// Encoding: use IEnumerable.ToEncodedString() to convert IEnumerable<typeparamref name="T"/> to string.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IEnumerable<T>? DecodeFromString<T>(this string source)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            return null;
        }

        var decodedItems = Encoding.UTF8.GetString(Convert.FromBase64String(source));

        return !string.IsNullOrWhiteSpace(decodedItems)
            ? JsonConvert.DeserializeObject<IEnumerable<T>>(decodedItems)
            : null;
    }
}
