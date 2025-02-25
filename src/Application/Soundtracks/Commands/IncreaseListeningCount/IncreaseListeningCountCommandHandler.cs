using Application.Abstractions;
using Application.DTOs.Result;
using Domain.Entities;
using MediatR;

namespace Application.Soundtracks.Commands.IncreaseListeningCount;

public class IncreaseListeningCountCommandHandler(ISoundtrackRepository repository, ICacheService cacheService)
    : IRequestHandler<IncreaseListeningCountCommand, Result<string>>
{
    public async Task<Result<string>> Handle(IncreaseListeningCountCommand request, CancellationToken cancellationToken)
    {
        var soundtrack = await repository.GetById(request.Id);
        if (soundtrack == null)
        {
            return Result<string>.Failure("The soundtrack not found.");
        }
        
        soundtrack.Listenings = (soundtrack.Listenings ?? 0) + 1;
        await repository.Update(soundtrack);
        await cacheService.RefreshSoundtracksAsync(cancellationToken);
        
        return Result<string>.Success("Successfully increased litenings count");
    }
}