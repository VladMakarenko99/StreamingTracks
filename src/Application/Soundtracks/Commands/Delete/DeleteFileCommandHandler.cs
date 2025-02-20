using Application.Abstractions;
using Application.DTOs.Result;
using MediatR;
using static Domain.Constants.AwsConstants;

namespace Application.Soundtracks.Commands.Delete;

public class DeleteFileCommandHandler(
    ISoundtrackRepository repository,
    IUnitOfWork unitOfWork,
    ICacheService cache,
    IAwsS3Service awsS3Service) : IRequestHandler<DeleteFileCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var soundtrack = await repository.GetById(request.Id);
            if (soundtrack == null)
            {
                return Result<string>.Failure("The soundtrack not found.");
            }

            var musicFileKey = $"{TracksFolder}/{soundtrack.Slug}{soundtrack.Extension}";
            await awsS3Service.DeleteFileAsync(musicFileKey, cancellationToken);
            
            if (!string.IsNullOrEmpty(soundtrack.AlbumCoverUrl))
            {
                var albumCoverKey = $"{AlbumCoversFolder}/{soundtrack.Slug}.jpg";
                await awsS3Service.DeleteFileAsync(albumCoverKey, cancellationToken);
            }

            await repository.Delete(soundtrack);
            await unitOfWork.CommitAsync(cancellationToken);

            await cache.RefreshSoundtracksAsync(cancellationToken);

            return Result<string>.Success($"Deleted: {soundtrack.Title}");
        }
        catch (Exception e)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            
            // TODO backup managing system
            
            return Result<string>.Failure($"Fail during deletion in file system: {e.Message}");
        }
    }
}