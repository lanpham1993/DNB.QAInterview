using DNB.QAInterview.Entities;

namespace DNB.QAInterview.PersonService;

// Hello world
// Pretend this repository is on an Azure SQL Database
public class DummyPersonRepository : IPersonRepository
{
    private readonly List<Person> _people = createDummyPeople();
    
    public Task AddPerson(Person person, CancellationToken cancellationToken = default)
    {
        if (person is null)
        {
            throw new ArgumentNullException(nameof(person));
        }

        _people.Add(person);
        
        return Task.CompletedTask;
    }

    public Task DeletePerson(int id, CancellationToken cancellationToken = default)
    {
        var person = _people.FirstOrDefault(p => p.Id == id);

        if (person is not null)
            _people.Remove(person);

        return Task.CompletedTask;
    }

    public Task<IEnumerable<Person>> GetPeople(int? startIndex = 0, int? pageSize = 50, CancellationToken cancellationToken = default)
    {
        var peopleQuery = _people.AsQueryable<Person>();

        if (startIndex is not null)
            peopleQuery = peopleQuery.Skip(startIndex.Value);

        if (pageSize is not null)
            peopleQuery = peopleQuery.Take(pageSize.Value);

        return Task.FromResult(peopleQuery.AsEnumerable());
    }

    public Task<Person?> GetPerson(int id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_people.FirstOrDefault(p => p.Id == id));
    }

    public Task UpdatePerson(Person person, CancellationToken cancellationToken = default)
    {
        var oldPerson = _people.FirstOrDefault(p => p.Id == person.Id);

        if (oldPerson is not null)
            oldPerson = person;

        return Task.CompletedTask;
    }

    
    private static List<Person> createDummyPeople()
    {
        var people = new List<Person>();
        for (int i = 0; i < 10000; i++)
        {
            var person = new Person
            {
                Id = i,
                Name = $"Person {i}",
                Address = $"Address {i}",
                PhoneNumber = $"Phone Number {i}",
                Email = $"Email {i}"
            };
            people.Add(person);
        }
        return people;
    }
}
