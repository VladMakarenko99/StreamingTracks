using Application.Abstractions;
using Application.DTOs.Result;
using Application.DTOs.Soundtrack;
using Domain.Entities;
using MediatR;
using Exception = System.Exception;

namespace Application.Soundtracks.Queries.GetById;

public class GetByIdQueryHandler(ISoundtrackRepository repository)
    : IRequestHandler<GetByIdQuery, Result<SoundtrackDto>>
{
    public async Task<Result<SoundtrackDto>> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var soundtrack = await repository.GetById(request.id);
            if (soundtrack is null)
            {
                return Result<SoundtrackDto>.Failure("Soundtrack not found.");
            }

            var ids = await repository.GetPreviousAndNextTrackIds(soundtrack);
            
            var dto = new SoundtrackDto(
                soundtrack.Id,
                soundtrack.Title,
                soundtrack.LengthInSeconds,
                soundtrack.AlbumCoverFileName,
                ids.NextTrackId ?? Guid.Empty,
                ids.PrevTrackId ?? Guid.Empty
            );
            return Result<SoundtrackDto>.Success(dto);
        }
        catch (Exception e)
        {
            return Result<SoundtrackDto>.Failure($"{e.Message}");
        }
    }
}