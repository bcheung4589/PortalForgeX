namespace PortalForgeX.Shared.DTOs;

public static class PaymentMethods
{
    public readonly static string Cash = "cash";
    public readonly static string Ideal = "ideal";
    public readonly static string Banktransfer = "banktransfer";

    public static string[] AsArray() => new string[] { Cash, Ideal, Banktransfer };

    public static Dictionary<string, string> AsDictionary() => new()
    {
        { Ideal, nameof(Ideal)},
        { Banktransfer, nameof(Banktransfer)},
        { Cash, nameof(Cash)}
    };
}
