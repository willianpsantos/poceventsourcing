using POCEventSourcing.Entities;
using POCEventSourcing.Interfaces.Services;
using POCEventSourcing.IoC;

var dibuilder = new ManualDependencyInjectionBuilder();

var di =
    dibuilder
        .AppSettingsPath("appsettings.json")
        .BuildConfiguration(true)
        .Build();

var personService = di.GetService<IPersonService>();

var person = new Person
{
    Name = "TESTE TABLE STORAGE",
    Document = "0200456859",
    Addresses = new List<PersonAddress>
    {
        new PersonAddress
        {
            Address = "RUA DAS FLORES, 123",
            City = "CUIABÁ",
            Region = "MT",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = 1
        }
    }
};

try
{
    var id = await personService.InsertAsync(person);

    person.Name = "TESTE EDIÇÃO STORAGE";
    person.Addresses.First().Address = "RUA DAS ANTIGAS FLORES, 4321";

    await personService.UpdateAsync(person);

    await personService.SendTrackedChangesAsync();

}
catch (Exception ex)
{

}

Console.ReadKey();
