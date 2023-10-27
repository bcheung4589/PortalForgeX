namespace PortalForgeX.Domain.Entities.Internal;

public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
{
    /// <inheritdoc/>
    public TPrimaryKey Id { get; set; } = default!;

    /// <inheritdoc/>
    public bool IsTransient()
    {
        if (EqualityComparer<TPrimaryKey>.Default.Equals(Id, default))
        {
            return true;
        }

        if (typeof(TPrimaryKey) == typeof(int))
        {
            return Convert.ToInt32(Id) <= 0;
        }

        if (typeof(TPrimaryKey) == typeof(long))
        {
            return Convert.ToInt64(Id) <= 0;
        }

        return false;
    }

    /// <summary>
    /// Check if the object is equal based on its Id
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not Entity<TPrimaryKey>)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        var other = (Entity<TPrimaryKey>)obj;
        if (IsTransient() && other.IsTransient())
        {
            return false;
        }

        var typeOfThis = GetType();
        var typeOfOther = other.GetType();
        if (!typeOfThis.IsAssignableFrom(typeOfOther) && !typeOfOther.IsAssignableFrom(typeOfThis))
        {
            return false;
        }

        return Id!.Equals(other.Id);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Id!.GetHashCode();
    }

    /// <inheritdoc/>
    public static bool operator ==(Entity<TPrimaryKey>? left, Entity<TPrimaryKey>? right)
    {
        if (Equals(left, null))
        {
            return Equals(right, null);
        }

        return left.Equals(right);
    }

    /// <inheritdoc/>
    public static bool operator !=(Entity<TPrimaryKey>? left, Entity<TPrimaryKey>? right)
    {
        return !(left == right);
    }
}
