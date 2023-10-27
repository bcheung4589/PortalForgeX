using Newtonsoft.Json;
using System.Reflection;

namespace PortalForgeX.Persistence.EFCore.Seeders.Internals;

public class AddressList
{
    [JsonProperty("street_suffix")]
    public string[] StreetSuffix { get; set; } = null!;
    public string[] Places { get; set; } = null!;

    private AddressList() { }

    public static async Task<AddressList> LoadAsync()
    {
        var list = new AddressList();

        var filepath = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Seeders/Data/addresses.json";
        if (!File.Exists(filepath))
        {
            return list;
        }

        using var reader = new StreamReader(filepath);
        string json = await reader.ReadToEndAsync();
        list = JsonConvert.DeserializeObject<AddressList>(json);

        return list ?? new AddressList();
    }

    public string GetRandomStreet(Random random, string seedName)
        => StreetSuffix.Length < 1 ? seedName : $"{seedName}{StreetSuffix[random.Next(StreetSuffix.Length)]}";

    public string GetRandomPlace(Random random)
        => Places.Length < 1 ? "" : Places[random.Next(Places.Length)];
}
