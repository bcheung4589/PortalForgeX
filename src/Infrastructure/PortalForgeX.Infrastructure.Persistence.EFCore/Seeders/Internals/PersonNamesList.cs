using Newtonsoft.Json;
using System.Reflection;

namespace PortalForgeX.Persistence.EFCore.Seeders.Internals;

public record PersonName(string FirstName, string LastName, bool IsMale)
{
    public override string ToString()
        => $"{FirstName} {LastName}";
}

public class PersonNamesList
{
    public string[] Boys { get; set; } = null!;
    public string[] Girls { get; set; } = null!;
    public string[] Last { get; set; } = null!;

    public Dictionary<int, PersonName> GeneratedNames { get; private set; } = null!;

    private PersonNamesList() { }

    public static async Task<PersonNamesList> LoadAsync()
    {
        var list = new PersonNamesList();

        var filepath = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Seeders/Data/personnames.json";
        if (!File.Exists(filepath))
        {
            return list;
        }

        using var reader = new StreamReader(filepath);
        string json = await reader.ReadToEndAsync();
        list = JsonConvert.DeserializeObject<PersonNamesList>(json);

        return list ?? new PersonNamesList();
    }

    public void Generate(int amount, Random random)
    {
        var boysCount = Boys.Length;
        var girlsCount = Girls.Length;
        var lastCount = Last.Length;
        GeneratedNames = new Dictionary<int, PersonName>();
        for (int i = 0; i < amount; i++)
        {
            bool isMale = random.Next(0, 1) == 1;

            string? firstName;
            string? lastName;
            if (isMale)
            {
                firstName = Girls[random.Next(0, boysCount - 1)];
                lastName = Last[random.Next(0, lastCount - 1)];
            }
            else
            {
                firstName = Girls[random.Next(0, girlsCount - 1)];
                lastName = Last[random.Next(0, lastCount - 1)];
            }

            GeneratedNames[i] = new PersonName(firstName, lastName, isMale);
        }
    }
}
