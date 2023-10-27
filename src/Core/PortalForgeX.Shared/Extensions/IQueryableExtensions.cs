using PortalForgeX.Shared.Extensions;
using PortalForgeX.Shared.FieldFilters;
using PortalForgeX.Shared.FieldFilters.Internals;
using System.Linq.Expressions;
using System.Reflection;

namespace PortalForgeX.Shared.Extensions;

public static class IQueryableExtensions
{
    #region [ Order: Property as String ]

    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, IComparer<object>? comparer = null)
        => source.CallOrderedQueryable(nameof(OrderBy), propertyName, comparer);

    public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName, IComparer<object>? comparer = null)
        => source.CallOrderedQueryable(nameof(OrderByDescending), propertyName, comparer);

    public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propertyName, IComparer<object>? comparer = null)
        => source.CallOrderedQueryable(nameof(ThenBy), propertyName, comparer);

    public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string propertyName, IComparer<object>? comparer = null)
        => source.CallOrderedQueryable(nameof(ThenByDescending), propertyName, comparer);

    private static IOrderedQueryable<T> CallOrderedQueryable<T>(this IQueryable<T> source, string methodName, string propertyName, IComparer<object>? comparer = null)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var body = propertyName.Split('.').Aggregate<string, Expression>(param, Expression.PropertyOrField);

        return comparer is not null
            ? (IOrderedQueryable<T>)source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable),
                    methodName,
                    new[] { typeof(T), body.Type },
                    source.Expression,
                    Expression.Lambda(body, param),
                    Expression.Constant(comparer)
                )
            )
            : (IOrderedQueryable<T>)source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable),
                    methodName,
                    new[] { typeof(T), body.Type },
                    source.Expression,
                    Expression.Lambda(body, param)
                )
            );
    }

    #endregion [ Order: Property as String ]

    #region [ Where: Filters ]

    public static IQueryable<T> Where<T>(this IQueryable<T> source, IEnumerable<IFieldFilter> filters, string? pattern = "*")
    {
        foreach (var filter in filters)
        {
            source = filter switch
            {
                AutoCompleteFieldFilter fieldFilter
                    => source.WhereEquals(fieldFilter.Field, fieldFilter.Value!),
                TextFieldFilter fieldFilter
                    => source.WhereLike(fieldFilter.Field, fieldFilter.Value!, pattern),
                SearchFieldsFilter fieldFilter
                    => source.WhereLike(fieldFilter.Fields, fieldFilter.Value!),
                SwitchFieldFilter fieldFilter
                    => source.WhereEquals(fieldFilter.Field, fieldFilter.Value),
                DateFieldFilter fieldFilter
                    => source.WhereDateEquals(fieldFilter.Field, fieldFilter.Value),
                NumberFieldFilter fieldFilter
                    => source.WhereEquals(fieldFilter.Field, fieldFilter.Value!),
                DateBetweenFieldFilter fieldFilter
                    => source.WhereDateBetween(fieldFilter.Field, fieldFilter.Value, fieldFilter.MaxValue),
                NumberBetweenFieldFilter fieldFilter
                    => source.WhereBetween(fieldFilter.Field, fieldFilter.Value, fieldFilter.MaxValue),
                ListedFieldFilter fieldFilter
                    => source.WhereEquals(fieldFilter.Field, fieldFilter.Value!),
                ListedMultiFieldFilter fieldFilter
                    => source.WhereContains(fieldFilter.Field, fieldFilter.Value!),
                _ => source
            };
        }

        return source;
    }

    /// <summary>
    /// Create an ConstantExpression for that analyses the property type to convert the value.
    /// </summary>
    /// <param name="property"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private static ConstantExpression? CreateConstantExpressionForProperty(PropertyInfo property, object? value)
    {
        var propertyType = property.PropertyType;
        if (propertyType != typeof(Guid))
        {
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                propertyType = Nullable.GetUnderlyingType(propertyType);
            }

            if (propertyType is null)
            {
                return null;
            }

            return Type.GetTypeCode(propertyType) switch
            {
                TypeCode.Int16 => Expression.Constant(Convert.ToInt16(value), property.PropertyType),
                TypeCode.Int32 => Expression.Constant(Convert.ToInt32(value), property.PropertyType),
                TypeCode.Int64 => Expression.Constant(Convert.ToInt64(value), property.PropertyType),
                TypeCode.Decimal => Expression.Constant(Convert.ToDecimal(value), property.PropertyType),
                TypeCode.Double => Expression.Constant(Convert.ToDouble(value), property.PropertyType),
                TypeCode.Single => Expression.Constant(Convert.ToSingle(value), property.PropertyType),
                _ => Expression.Constant(value, property.PropertyType),
            };
        }
        else
        {
            if (Guid.TryParse(value?.ToString(), out var guidValue))
            {
                return Expression.Constant(guidValue, property.PropertyType);
            }
        }

        return null;
    }

    /// <summary>
    /// Search for records based on multiple fields.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="fields"></param>
    /// <param name="value"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static IQueryable<T> WhereLike<T>(this IQueryable<T> source, IEnumerable<string> fields, string value, string? pattern = "*")
    {
        var type = typeof(T);
        var property = type.GetProperty(fields.First());
        if (property is null)
        {
            return source;
        }

        var parameter = Expression.Parameter(type, "x");
        var expressions = new List<Expression>();
        foreach (var field in fields)
        {
            var fieldType = typeof(T);
            var fieldProperty = fieldType.GetProperty(field);
            if (fieldProperty is null)
            {
                continue;
            }

            var startsWith = value!.StartsWith(pattern!);
            var endsWith = value.EndsWith(pattern!);
            var member = Expression.Property(parameter, fieldProperty);
            var constant = Expression.Constant(value.Trim(pattern!.ToCharArray()));
            Expression? fieldExpression;
            if (endsWith && startsWith)
            {
                fieldExpression = Expression.Call(member, typeof(string).GetMethod(nameof(string.Contains))!, constant);
            }
            else if (endsWith) // use the pattern as wildcard
            {
                fieldExpression = Expression.Call(member, typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string) })!, constant);
            }
            else if (startsWith) // use the pattern as wildcard
            {
                fieldExpression = Expression.Call(member, typeof(string).GetMethod(nameof(string.EndsWith), new[] { typeof(string) })!, constant);
            }
            else // equal
            {
                constant = CreateConstantExpressionForProperty(property, value);
                if (constant is null)
                {
                    return source;
                }
                fieldExpression = Expression.Equal(member, constant);
            }

            expressions.Add(fieldExpression);
        }

        var expression = expressions.Aggregate(Expression.Or);

        return source.Where(Expression.Lambda<Func<T, bool>>(expression, parameter));
    }

    /// <summary>
    /// Search for records that has the field's value like the specified value. Use the pattern as wildcard.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="field"></param>
    /// <param name="value"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static IQueryable<T> WhereLike<T>(this IQueryable<T> source, string field, string value, string? pattern = "*")
    {
        var type = typeof(T);
        var property = type.GetProperty(field);
        if (property is null)
        {
            return source;
        }

        var startsWith = value!.StartsWith(pattern!);
        var endsWith = value.EndsWith(pattern!);
        var constant = Expression.Constant(value.Trim(pattern!.ToCharArray()));
        var parameter = Expression.Parameter(type, "x");
        var member = Expression.Property(parameter, property);
        Expression expression;
        if (endsWith && startsWith)
        {
            expression = Expression.Call(member, typeof(string).GetMethod(nameof(string.Contains))!, constant);
        }
        else if (endsWith) // use the pattern as wildcard
        {
            expression = Expression.Call(member, typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string) })!, constant);
        }
        else if (startsWith) // use the pattern as wildcard
        {
            expression = Expression.Call(member, typeof(string).GetMethod(nameof(string.EndsWith), new[] { typeof(string) })!, constant);
        }
        else // equal
        {
            constant = CreateConstantExpressionForProperty(property, value);
            if (constant is null)
            {
                return source;
            }
            expression = Expression.Equal(member, constant);
        }

        return source.Where(Expression.Lambda<Func<T, bool>>(expression, parameter));
    }

    /// <summary>
    /// Search for records based on numbers, booleans and string where equals the specified value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="field"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static IQueryable<T> WhereEquals<T>(this IQueryable<T> source, string field, object value)
    {
        var type = typeof(T);
        var property = type.GetProperty(field);
        if (property is null)
        {
            return source;
        }

        var valueConstantExpression = CreateConstantExpressionForProperty(property, value);
        if (valueConstantExpression is null)
        {
            return source;
        }

        var parameter = Expression.Parameter(type, "x");
        var expression = Expression.Equal(Expression.Property(parameter, property), valueConstantExpression);

        return source.Where(Expression.Lambda<Func<T, bool>>(expression, parameter));
    }

    /// <summary>
    /// Search for records that has the field's value in the searchList.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="field"></param>
    /// <param name="searchList"></param>
    /// <returns></returns>
    public static IQueryable<T> WhereContains<T>(this IQueryable<T> source, string field, IEnumerable<string> searchList)
    {
        var type = typeof(T);
        var property = type.GetProperty(field);
        if (property is null || !searchList.Any())
        {
            return source;
        }

        Type listType = searchList.GetType();
        ConstantExpression? listConstantExpression;
        if (property.PropertyType != typeof(Guid))
        {
            listConstantExpression = Type.GetTypeCode(property.PropertyType) switch
            {
                TypeCode.Int16 => Expression.Constant(searchList.ParseList<short>(out listType), listType),
                TypeCode.UInt16 => Expression.Constant(searchList.ParseList<ushort>(out listType), listType),
                TypeCode.Int32 => Expression.Constant(searchList.ParseList<int>(out listType), listType),
                TypeCode.UInt32 => Expression.Constant(searchList.ParseList<uint>(out listType), listType),
                TypeCode.Int64 => Expression.Constant(searchList.ParseList<long>(out listType), listType),
                TypeCode.UInt64 => Expression.Constant(searchList.ParseList<ulong>(out listType), listType),
                TypeCode.Single => Expression.Constant(searchList.ParseList<float>(out listType), listType),
                TypeCode.Double => Expression.Constant(searchList.ParseList<double>(out listType), listType),
                TypeCode.Decimal => Expression.Constant(searchList.ParseList<decimal>(out listType), listType),
                TypeCode.DateTime => Expression.Constant(searchList.ParseList<DateTime>(out listType), listType),
                TypeCode.String => Expression.Constant(searchList, searchList.GetType()),
                _ => null,
            };
        }
        else
        {
            listConstantExpression = Expression.Constant(searchList.ParseList<Guid>(out listType), listType);
        }

        if (listConstantExpression is null)
        {
            return source;
        }

        var parameter = Expression.Parameter(type, "x");
        var method = listType.GetMethod("Contains");
        if (method is null)
        {
            return source;
        }

        var expression = Expression.Call(listConstantExpression, method, Expression.Property(parameter, property));

        return source.Where(Expression.Lambda<Func<T, bool>>(expression, parameter));
    }

    /// <summary>
    /// Search for records where the field's value is greater then or equal to the value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="field"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static IQueryable<T> WhereGreaterOrEqual<T>(this IQueryable<T> source, string field, object? value)
    {
        var type = typeof(T);
        var property = type.GetProperty(field);
        if (property is null)
        {
            return source;
        }

        var valueConstantExpression = CreateConstantExpressionForProperty(property, value);
        if (valueConstantExpression is null)
        {
            return source;
        }

        var parameter = Expression.Parameter(type, "x");
        var expression = Expression.GreaterThanOrEqual(Expression.Property(parameter, property), valueConstantExpression);

        return source.Where(Expression.Lambda<Func<T, bool>>(expression, parameter));
    }

    /// <summary>
    /// Search for records where the field's value is less or equal to the value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="field"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static IQueryable<T> WhereLessOrEqual<T>(this IQueryable<T> source, string field, object? value)
    {
        var type = typeof(T);
        var property = type.GetProperty(field);
        if (property is null)
        {
            return source;
        }

        var valueConstantExpression = CreateConstantExpressionForProperty(property, value);
        if (valueConstantExpression is null)
        {
            return source;
        }

        var parameter = Expression.Parameter(type, "x");
        var expression = Expression.LessThanOrEqual(Expression.Property(parameter, property), valueConstantExpression);

        return source.Where(Expression.Lambda<Func<T, bool>>(expression, parameter));
    }

    /// <summary>
    /// Search for records where the field's value is between the value and max value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="field"></param>
    /// <param name="value"></param>
    /// <param name="maxValue"></param>
    /// <returns></returns>
    public static IQueryable<T> WhereBetween<T>(this IQueryable<T> source, string field, object? value, object? maxValue)
    {
        if (value is not null)
        {
            source = source.WhereGreaterOrEqual(field, value);
        }

        if (maxValue is not null)
        {
            source = source.WhereLessOrEqual(field, maxValue);
        }

        return source;
    }

    /// <summary>
    /// Search for records where the field's value equals the (DateTime) value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="field"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static IQueryable<T> WhereDateEquals<T>(this IQueryable<T> source, string field, DateTime? value)
    {
        var type = typeof(T);
        var property = type.GetProperty(field);
        if (property is null)
        {
            return source;
        }

        var parameter = Expression.Parameter(type, "x");
        var propertyExpression = Expression.Property(parameter, property);
        var datePropertyExpression = Expression.Property(propertyExpression, nameof(DateTime.Date));
        var expression = Expression.Equal(datePropertyExpression, Expression.Constant(value, property.PropertyType));

        return source.Where(Expression.Lambda<Func<T, bool>>(expression, parameter));
    }

    /// <summary>
    /// Search for records where the field's value is greater then or equal to the (DateTime) value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="field"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static IQueryable<T> WhereDateGreaterOrEqual<T>(this IQueryable<T> source, string field, DateTime? value)
    {
        var type = typeof(T);
        var property = type.GetProperty(field);
        if (property is null)
        {
            return source;
        }

        var parameter = Expression.Parameter(type, "x");
        var propertyExpression = Expression.Property(parameter, property);
        var datePropertyExpression = Expression.Property(propertyExpression, nameof(DateTime.Date));
        var expression = Expression.GreaterThanOrEqual(datePropertyExpression, Expression.Constant(value, property.PropertyType));

        return source.Where(Expression.Lambda<Func<T, bool>>(expression, parameter));
    }

    /// <summary>
    /// Search for records where the field's value is less or equal to the (DateTime) value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="field"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static IQueryable<T> WhereDateLessOrEqual<T>(this IQueryable<T> source, string field, DateTime? value)
    {
        var type = typeof(T);
        var property = type.GetProperty(field);
        if (property is null)
        {
            return source;
        }

        var parameter = Expression.Parameter(type, "x");
        var propertyExpression = Expression.Property(parameter, property);
        var datePropertyExpression = Expression.Property(propertyExpression, nameof(DateTime.Date));
        var expression = Expression.LessThanOrEqual(datePropertyExpression, Expression.Constant(value, property.PropertyType));

        return source.Where(Expression.Lambda<Func<T, bool>>(expression, parameter));
    }

    /// <summary>
    /// Search for records where the field's value is between the (DateTime) value and (DateTime) max value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="field"></param>
    /// <param name="value"></param>
    /// <param name="maxValue"></param>
    /// <returns></returns>
    public static IQueryable<T> WhereDateBetween<T>(this IQueryable<T> source, string field, DateTime? value, DateTime? maxValue)
    {
        if (value is not null)
        {
            source = source.WhereDateGreaterOrEqual(field, value);
        }

        if (maxValue is not null)
        {
            source = source.WhereDateLessOrEqual(field, maxValue);
        }

        return source;
    }

    #endregion [ Where: Filters ]
}
