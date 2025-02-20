using Application.Abstractions;
using Application.DTOs.Result;
using Domain.Entities;
using MediatR;
using Slugify;
using Unidecode.NET;
using static Domain.Constants.AwsConstants;

namespace Application.Soundtracks.Commands.Upload;

public class UploadFileCommandHandler(
    ISoundtrackRepository repository,
    IUnitOfWork unitOfWork,
    ICacheService cache,
    IAwsS3Service awsS3Service)
    : IRequestHandler<UploadFileCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        var musicFileName = request.File.FileName;
        var extension = Path.GetExtension(musicFileName);
        var nameWithoutExtension = Path.GetFileNameWithoutExtension(musicFileName);
        var slug = new SlugHelper().GenerateSlug(nameWithoutExtension.Unidecode());

        if (extension != ".mp3" && extension != ".m4a")
        {
            return Result<string>.Failure("Only .mp3 and .m4a files are allowed.");
        }

        if (await repository.ExistsAsync(nameWithoutExtension))
        {
            return Result<string>.Failure("File already exists.");
        }

        var albumCoverName = string.Empty;
        byte[]? albumCoverData = null;
        string musicFileKey = string.Empty;
        string albumCoverKey = string.Empty;

        try
        {
            double audioLength = 0;
            using var file = TagLib.File.Create(new FormFileAbstraction(musicFileName, request.File.OpenReadStream()));
            
            audioLength = file.Properties.Duration.TotalSeconds;

            if (file.Tag.Pictures.Length > 0)
            {
                albumCoverData = file.Tag.Pictures[0].Data.Data;
                albumCoverName = $"{slug}.jpg";
            }


            musicFileKey = $"{TracksFolder}/{slug}{extension}";
            var musicFileUrl = await awsS3Service.UploadFileAsync(request.File.OpenReadStream(), musicFileKey,
                request.File.ContentType, cancellationToken);

            string? albumCoverUrl = null;
            if (albumCoverData != null)
            {
                albumCoverKey = $"{AlbumCoversFolder}/{albumCoverName}";
                albumCoverUrl = await awsS3Service.UploadFileAsync(new MemoryStream(albumCoverData), albumCoverKey,
                    "image/jpeg", cancellationToken);
            }

            var soundtrack = new Soundtrack()
            {
                Title = nameWithoutExtension,
                Extension = extension,
                LengthInSeconds = audioLength,
                CreatedAt = DateTime.UtcNow,
                Slug = slug,
                MusicFileUrl = musicFileUrl,
                AlbumCoverUrl = albumCoverUrl
            };

            await repository.Add(soundtrack);
            await unitOfWork.CommitAsync(cancellationToken);
            await cache.RefreshSoundtracksAsync(cancellationToken);

            return Result<string>.Success(musicFileUrl);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            if (!string.IsNullOrEmpty(musicFileKey))
            {
                await awsS3Service.DeleteFileAsync(musicFileKey, cancellationToken);
            }

            if (!string.IsNullOrEmpty(albumCoverKey))
            {
                await awsS3Service.DeleteFileAsync(albumCoverKey, cancellationToken);
            }

            return Result<string>.Failure(ex.Message);
        }
    }
}