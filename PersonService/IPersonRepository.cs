using DNB.QAInterview.Entities;

namespace DNB.QAInterview.PersonService;

public interface IPersonRepository
{
    public Task<Person?> GetPerson(int id, CancellationToken cancellationToken = default);
    public Task<IEnumerable<Person>> GetPeople(int? startIndex = 0, int? pageSize = 0, CancellationToken cancellationToken = default);
    public Task AddPerson(Person person, CancellationToken cancellationToken = default);
    public Task UpdatePerson(Person person, CancellationToken cancellationToken = default);
    public Task DeletePerson(int id, CancellationToken cancellationToken = default);
   
}
