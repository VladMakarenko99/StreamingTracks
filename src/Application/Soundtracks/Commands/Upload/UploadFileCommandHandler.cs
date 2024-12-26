using Application.Abstractions;
using Application.DTOs.Result;
using Domain.Constants;
using Domain.Entities;
using MediatR;
using static Domain.Constants.DirectoryConstants;

namespace Application.Soundtracks.Commands.Upload;

public class UploadFileCommandHandler(ISoundtrackRepository repository, IUnitOfWork unitOfWork, ICacheService cache)
    : IRequestHandler<UploadFileCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        var musicFileName = request.File.FileName;
        var musicFilePath = Path.Combine(Directory.GetCurrentDirectory(), GeneralMusicDirectory, musicFileName);

        if (File.Exists(musicFilePath))
        {
            return Result<string>.Failure("File already exists.");
        }

        var albumCoverName = string.Empty;

        try
        {
            var extension = Path.GetExtension(musicFileName);
            if (extension != ".mp3" && extension != ".m4a")
            {
                return Result<string>.Failure("Only .mp3 and .m4a files are allowed.");
            }

            var musicFileDirectory = Path.Combine(Directory.GetCurrentDirectory(), GeneralMusicDirectory);

            if (!Directory.Exists(musicFileDirectory))
            {
                Directory.CreateDirectory(musicFileDirectory);
            }

            await using (var stream = new FileStream(musicFilePath, FileMode.Create))
            {
                await request.File.CopyToAsync(stream, cancellationToken);
            }

            double audioLength = 0;

            using (var file = TagLib.File.Create(musicFilePath))
            {
                audioLength = file.Properties.Duration.TotalSeconds;

                if (file.Tag.Pictures.Length > 0)
                {
                    var albumCoverData = file.Tag.Pictures[0].Data.Data;
                    var albumCoverDirectory = Path.Combine(Directory.GetCurrentDirectory(), GeneralMusicDirectory,
                        AlbumCoverDirectory);

                    if (!Directory.Exists(albumCoverDirectory))
                    {
                        Directory.CreateDirectory(albumCoverDirectory);
                    }

                    albumCoverName = (Path.GetFileNameWithoutExtension(musicFileName) + ".jpg").Replace(' ', '-');
                    await File.WriteAllBytesAsync(Path.Combine(albumCoverDirectory, albumCoverName), albumCoverData,
                        cancellationToken);
                }
            }

            var soundtrack = new Soundtrack()
            {
                Title = Path.GetFileNameWithoutExtension(musicFileName),
                Extension = extension,
                LengthInSeconds = audioLength,
                AlbumCoverFileName = albumCoverName,
                CreatedAt = DateTime.Now
            };

            await repository.Add(soundtrack);
            await unitOfWork.CommitAsync(cancellationToken);

            await cache.RefreshSoundtracksAsync(cancellationToken);
            
            return Result<string>.Success(musicFileName);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync(cancellationToken);

            if (File.Exists(musicFilePath))
            {
                File.Delete(musicFilePath);
            }

            if (!string.IsNullOrEmpty(albumCoverName))
            {
                var albumCoverPath = Path.Combine(Directory.GetCurrentDirectory(), GeneralMusicDirectory,
                    AlbumCoverDirectory, albumCoverName);
                if (File.Exists(albumCoverPath))
                {
                    File.Delete(albumCoverPath);
                }
            }

            return Result<string>.Failure(ex.Message);
        }
    }
}