using Application.Abstractions;
using Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using static Domain.Constants.CacheConstants;

namespace Infrastructure.Repository;

public class CacheService(IMemoryCache cache, ISoundtrackRepository repository) : ICacheService
{
    public async Task RefreshSoundtracksAsync(CancellationToken cancellationToken)
    {
        var updatedSoundtracks = await repository.GetAll();
        cache.Set(SoundtracksKey, updatedSoundtracks, new MemoryCacheEntryOptions
        {
            Priority = CacheItemPriority.NeverRemove
        });
    }
}