using Domain.Entities;

namespace Application.Abstractions;

public interface ISoundtrackRepository
{
    Task Add(Soundtrack soundtrack);
    
    Task Update(Soundtrack soundtrack);
    
    Task<Soundtrack?> GetById(string id);

    Task<List<Soundtrack>> GetAll();
    
    Task Delete(Soundtrack soundtrack);
}
