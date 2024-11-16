using Application.Abstractions;
using Application.DTOs.Result;
using Domain.Entities;
using MediatR;
using static Domain.Constants.DirectoryConstants;

namespace Application.Soundtracks.Commands.Upload;

public class UploadFileCommandHandler(ISoundtrackRepository repository)
    : IRequestHandler<UploadFileCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var filename = request.File.FileName;

            var extension = Path.GetExtension(filename);
            if (extension != ".mp3" && extension != ".m4a")
            {
                return Result<string>.Failure("Only .mp3 and .m4a files are allowed.");
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), GeneralMusicDirectory);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            filePath = Path.Combine(Directory.GetCurrentDirectory(), GeneralMusicDirectory, filename);
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(stream, cancellationToken);
            }

            double audioLength = 0; 
            var albumCoverName = string.Empty;
            using (var file = TagLib.File.Create(filePath))
            {
                audioLength = file.Properties.Duration.TotalSeconds;
                if (file.Tag.Pictures.Length > 0)
                {
                    var albumCoverData = file.Tag.Pictures[0].Data.Data;
                    var albumCoverDirectory = Path.Combine(Directory.GetCurrentDirectory(), GeneralMusicDirectory, AlbumCoverDirectory);
                  
                    if (!Directory.Exists(albumCoverDirectory))
                    {
                        Directory.CreateDirectory(albumCoverDirectory);
                    }

                    albumCoverName = (Path.GetFileNameWithoutExtension(filename) + ".jpg").Replace(' ', '-');
                    
                    await File.WriteAllBytesAsync(Path.Combine(albumCoverDirectory, albumCoverName), albumCoverData, cancellationToken);
                }
            }
            var soundtrack = new Soundtrack()
            {
                Title = Path.GetFileNameWithoutExtension(filename),
                Extension = extension,
                LengthInSeconds = audioLength,
                AlbumCoverFileName = albumCoverName
            };

            await repository.Add(soundtrack);


            return Result<string>.Success(filename);
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(ex.Message);
        }
    }
}