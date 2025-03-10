using Application.Abstractions;
using Application.DTOs.Result;
using Application.DTOs.Soundtrack;
using Domain.Entities;
using MediatR;
using Exception = System.Exception;

namespace Application.Soundtracks.Queries.GetById;

public class GetBySlugQueryHandler(ISoundtrackRepository repository, IAwsS3Service awsS3Service)
    : IRequestHandler<GetBySlugQuery, Result<SoundtrackDto>>
{
    public async Task<Result<SoundtrackDto>> Handle(GetBySlugQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var soundtrack = await repository.GetBySlug(request.Slug);
            if (soundtrack is null)
            {
                return Result<SoundtrackDto>.Failure("Soundtrack not found.");
            }

            var slugs = await repository.GetPreviousAndNextTrackSlugs(soundtrack);

            var dto = new SoundtrackDto(
                soundtrack.Id,
                soundtrack.Title,
                soundtrack.LengthInSeconds,
                soundtrack.Slug,
                slugs.NextTrackSlug ?? string.Empty,
                slugs.PrevTrackSlug ?? string.Empty,
                soundtrack.MusicFileUrl,
                soundtrack.AlbumCoverUrl ?? string.Empty,
                soundtrack.Listenings);

            return Result<SoundtrackDto>.Success(dto);
        }
        catch (Exception e)
        {
            return Result<SoundtrackDto>.Failure($"{e.Message}");
        }
    }
}