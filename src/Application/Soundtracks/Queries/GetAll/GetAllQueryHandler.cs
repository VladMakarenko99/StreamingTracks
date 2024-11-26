using Application.Abstractions;
using Application.DTOs.Result;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using static Domain.Constants.CacheConstants;

namespace Application.Soundtracks.Queries.GetAll;

public class GetAllQueryHandler(ISoundtrackRepository repository, ICacheService cacheService, IMemoryCache cache)
    : IRequestHandler<GetAllQuery, Result<List<Soundtrack>>>
{
    public async Task<Result<List<Soundtrack>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        if (!cache.TryGetValue(SoundtracksKey, out List<Soundtrack> soundtracks))
        {
            await cacheService.RefreshSoundtracksAsync(cancellationToken);
            
            soundtracks = cache.Get<List<Soundtrack>>(SoundtracksKey) ?? [];
        }

        return Result<List<Soundtrack>>.Success(soundtracks);
    }
}