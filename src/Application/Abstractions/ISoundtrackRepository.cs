using Domain.Entities;

namespace Application.Abstractions;

public interface ISoundtrackRepository
{
    Task Add(Soundtrack soundtrack);
    
    Task Update(Soundtrack soundtrack);
    
    Task<Soundtrack?> GetById(Guid id);

    Task<List<Soundtrack>> GetAll();
    
    Task Delete(Soundtrack soundtrack);

    Task<(Guid? PrevTrackId, Guid? NextTrackId)> GetPreviousAndNextTrackIds(Soundtrack currentTrack);
}
