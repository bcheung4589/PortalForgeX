using Microsoft.EntityFrameworkCore;
using PortalForgeX.Application.Data;
using PortalForgeX.Domain.Entities;
using PortalForgeX.Domain.Services;
using PortalForgeX.Persistence.EFCore;
using PortalForgeX.Persistence.EFCore.Seeders.Internals;

namespace PortalForgeX.Persistence.EFCore.Seeders;

public sealed class ClientSeeder : IClientSeeder
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DbSet<Client> _table;
    private IList<string> generatedClientNames = null!;

    static readonly Random random = new();

    public ClientSeeder(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _table = ((DomainContext)_unitOfWork.Context).Set<Client>();
    }

    public async Task<int> ExecuteAsync(int amount = 100, CancellationToken cancellationToken = default)
    {
        var namesList = await PersonNamesList.LoadAsync();
        if (namesList.Boys.Length == 0 && namesList.Girls.Length == 0)
        {
            return 0;
        }

        namesList.Generate(amount, random);

        var addressList = await AddressList.LoadAsync();
        var generatedSeeds = new List<Client>();
        generatedClientNames = new List<string>();

        List<ClientContact>? contacts = null;
        List<BusinessLocation>? businessLocations = null;
        for (int i = 0; i < amount; i++)
        {
            var clientName = GenerateUniqueName();
            var clientCreationDate = DateTime.UtcNow.AddDays(random.Next(1000) * -1);

            var contactsCount = random.Next(1, 4);
            if (contactsCount > 0)
            {
                contacts = new List<ClientContact>();
                businessLocations = new List<BusinessLocation>();
                for (int j = 0; j < contactsCount; j++)
                {
                    var checkouts = new List<Checkout>();

                    var fullName = namesList.GeneratedNames[j].ToString();
                    contacts.Add(new ClientContact
                    {
                        FullName = fullName,
                        PhoneNr = random.Next(1000000000, 2147483647).ToString(),
                        Email = $"{fullName.ToLower().Replace(" ", "_")}@{clientName.ToLower().Replace(" ", "-")}.com",
                        IsActive = true,
                        CreationTime = clientCreationDate,
                    });

                    var checkoutsCount = random.Next(1, 4);
                    for (int k = 0; k < checkoutsCount; k++)
                    {
                        checkouts.Add(new Checkout
                        {
                            DeviceCode = Guid.NewGuid().ToString("N")[..12],
                            SoftwareVersion = "v0.3.0",
                            IsActive = true
                        });
                    }

                    businessLocations.Add(new BusinessLocation
                    {
                        ApiKey = Guid.NewGuid().ToString("N"),
                        Street = addressList.GetRandomStreet(random, clientName),
                        HouseNr = random.Next(1, 1000).ToString(),
                        Zipcode = $"{random.Next(1000, 10000)} {NextStrings(AlphaChars, 2, 2)}",
                        Place = addressList.GetRandomPlace(random),
                        Country = "Nederland",
                        StartDate = clientCreationDate.AddDays(random.Next(28)),
                        HasSubscription = random.Next(6) != 0,
                        IsActive = true,
                        Checkouts = checkouts
                    });
                }
            }

            generatedSeeds.Add(new Client
            {
                Name = clientName,
                IsActive = random.Next(4) != 0,
                HasCustomerCarePlus = random.Next(2) != 0,
                CreationTime = clientCreationDate,
                Contacts = contacts,
                Locations = businessLocations,
            });
        }

        using var transaction = _unitOfWork.BeginTransaction();
        await _table.AddRangeAsync(generatedSeeds, cancellationToken);
        int changes = await _unitOfWork.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return changes;
    }

    private string GenerateUniqueName()
    {
        var clientName = FictionalNameGenerator.GenerateRandomName(random);
        if (generatedClientNames.Contains(clientName))
        {
            return GenerateUniqueName();
        }

        generatedClientNames.Add(clientName);
        return clientName;
    }

    private readonly string AlphaChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static string NextStrings(string allowedChars, int minimalValue, int maximalValue)
    {
        var stringLength = random.Next(minimalValue, maximalValue + 1);
        var chars = new char[stringLength];
        for (int i = 0; i < stringLength; ++i)
        {
            chars[i] = allowedChars[random.Next(allowedChars.Length)];
        }

        return new string(chars);
    }
}
