using System.Diagnostics;
using Application.Abstractions;
using Application.DTOs.Result;
using Domain.Constants;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using static Domain.Constants.CacheConstants;

namespace Application.Soundtracks.Queries.GetAll;

public class GetAllQueryHandler(ISoundtrackRepository repository, IMemoryCache cache)
    : IRequestHandler<GetAllQuery, Result<List<Soundtrack>>>
{
    public async Task<Result<List<Soundtrack>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        if (!cache.TryGetValue(SoundtracksKey, out List<Soundtrack> soundtracks))
        {
            soundtracks = await repository.GetAll();

            var cacheExpiryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                .SetPriority(CacheItemPriority.Normal);

            cache.Set(SoundtracksKey, soundtracks, cacheExpiryOptions);
        }

        stopwatch.Stop();
        return Result<List<Soundtrack>>.Success(soundtracks);
    }
}