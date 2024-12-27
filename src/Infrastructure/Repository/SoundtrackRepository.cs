using Application.Abstractions;
using Domain.Entities;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class SoundtrackRepository(AppDbContext context) : ISoundtrackRepository
{
    public async Task Add(Soundtrack soundtrack)
    {
        await context.Soundtracks.AddAsync(soundtrack);
    }

    public async Task Update(Soundtrack soundtrack)
    {
        context.Soundtracks.Update(soundtrack);
        await context.SaveChangesAsync();
    }

    public async Task<Soundtrack?> GetById(Guid id) => await context.Soundtracks.FindAsync(id);

    public async Task<List<Soundtrack>> GetAll() => await context.Soundtracks.ToListAsync();

    public async Task Delete(Soundtrack soundtrack)
    {
        context.Remove(soundtrack);
        await context.SaveChangesAsync();
    }

    public async Task<(Guid? PrevTrackId, Guid? NextTrackId)> GetPreviousAndNextTrackIds(Soundtrack currentTrack)
    {
        var prevTrackId = await context.Soundtracks
            .Where(t => t.CreatedAt < currentTrack.CreatedAt)
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => t.Id)
            .FirstOrDefaultAsync();
        
        if (prevTrackId == Guid.Empty)
        {
            prevTrackId = await context.Soundtracks
                .OrderBy(t => t.CreatedAt)
                .Select(t => t.Id)
                .LastOrDefaultAsync();
        }
        
        var nextTrackId = await context.Soundtracks
            .Where(t => t.CreatedAt > currentTrack.CreatedAt)
            .OrderBy(t => t.CreatedAt)
            .Select(t => t.Id)
            .FirstOrDefaultAsync();
        
        if (nextTrackId == Guid.Empty)
        {
            nextTrackId = await context.Soundtracks
                .OrderBy(t => t.CreatedAt)
                .Select(t => t.Id)
                .FirstOrDefaultAsync();
        }

        return (prevTrackId, nextTrackId);
    }
}