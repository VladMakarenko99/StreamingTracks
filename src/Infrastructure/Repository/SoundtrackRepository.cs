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
        await context.SaveChangesAsync();
    }

    public async Task Update(Soundtrack soundtrack)
    {
        context.Soundtracks.Update(soundtrack);
        await context.SaveChangesAsync();
    }

    public async Task<Soundtrack?> GetById(string id) => await context.Soundtracks.FindAsync(id);

    public async Task<List<Soundtrack>> GetAll() => await context.Soundtracks.ToListAsync();

    public async Task Delete(Soundtrack soundtrack)
    {
        context.Remove(soundtrack);
        await context.SaveChangesAsync();
    }
}