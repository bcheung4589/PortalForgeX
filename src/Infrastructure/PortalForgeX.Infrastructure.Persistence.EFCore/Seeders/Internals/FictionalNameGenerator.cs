namespace PortalForgeX.Persistence.EFCore.Seeders.Internals;

internal class FictionalNameGenerator
{
    static readonly string[] prefixes = { "Za", "Li", "Mo", "Ga", "Ri", "Th", "Br", "Kr", "Qu", "Ha", "Fe", "Dr", "Ty", "Le", "Va", "Ne", "Ho", "Je", "Pl", "Xi" };
    static readonly string[] vowels = { "a", "e", "i", "o", "u", "y" };
    static readonly string[] suffixes = { "th", "ra", "lo", "na", "do", "mi", "zo", "ve", "ph", "el", "on", "ka", "si", "lu", "pa", "cy", "mo", "ko", "ru", "ta" };

    internal static string GenerateRandomName(Random random)
        => $"{prefixes[random.Next(prefixes.Length)]}{vowels[random.Next(vowels.Length)]}{suffixes[random.Next(suffixes.Length)]}";
}
