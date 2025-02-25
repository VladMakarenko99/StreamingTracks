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

    public async Task<Soundtrack?> GetBySlug(string slug)
        => await context.Soundtracks.SingleOrDefaultAsync(x => x.Slug == slug);


    public async Task<List<Soundtrack>> GetAll() => await context.Soundtracks.OrderBy(x => x.CreatedAt).ToListAsync();

    public async Task Delete(Soundtrack soundtrack)
    {
        context.Remove(soundtrack);
        await context.SaveChangesAsync();
    }

    public async Task<(string PrevTrackSlug, string NextTrackSlug)> GetPreviousAndNextTrackSlugs(Soundtrack currentTrack)
    {
        var prevTrackSlug
            = await context.Soundtracks
                .Where(t => t.CreatedAt < currentTrack.CreatedAt)
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => t.Slug)
                .FirstOrDefaultAsync();

        if (string.IsNullOrEmpty(prevTrackSlug))
        {
            prevTrackSlug = await context.Soundtracks
                .OrderBy(t => t.CreatedAt)
                .Select(t => t.Slug)
                .LastOrDefaultAsync();
        }

        var nextTrackSlug = await context.Soundtracks
            .Where(t => t.CreatedAt > currentTrack.CreatedAt)
            .OrderBy(t => t.CreatedAt)
            .Select(t => t.Slug)
            .FirstOrDefaultAsync();

        if (string.IsNullOrEmpty(nextTrackSlug))
        {
            nextTrackSlug = await context.Soundtracks
                .OrderBy(t => t.CreatedAt)
                .Select(t => t.Slug)
                .FirstOrDefaultAsync();
        }

        return (prevTrackSlug, nextTrackSlug)!;
    }

    public Task<bool> ExistsAsync(string title)
    {
        return context.Soundtracks.AnyAsync(soundtrack => soundtrack.Title == title);
    }
}