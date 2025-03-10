using Domain.Entities;

namespace Application.Abstractions;

public interface ISoundtrackRepository
{
    Task Add(Soundtrack soundtrack);
    
    Task Update(Soundtrack soundtrack);
    
    Task<Soundtrack?> GetById(Guid id);
    
    Task<Soundtrack?> GetBySlug(string slug);
    
    Task<List<Soundtrack>> GetAll();
    
    Task Delete(Soundtrack soundtrack);

    Task<(string PrevTrackSlug, string NextTrackSlug)> GetPreviousAndNextTrackSlugs(Soundtrack currentTrack);

    Task<bool> ExistsAsync(string title);
}
