using Application.Abstractions;
using Application.DTOs.Result;
using CSCore;
using Domain.Entities;
using MediatR;
using CSCore.Codecs;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace Application.Soundtracks.Commands.Upload;

public class UploadFileCommandHandler
    : IRequestHandler<UploadFileCommand, Result<string>>
{
    private readonly ISoundtrackRepository _repository;

    public UploadFileCommandHandler(ISoundtrackRepository repository)
    {
        this._repository = repository;

        FFmpeg.SetExecutablesPath(Path.Combine(Directory.GetCurrentDirectory(), "FFmpeg"));
    }


    public async Task<Result<string>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            string filename = request.File.FileName;

            var extension = "." + filename.Split('.')[filename.Split('.').Length - 1];
            if (extension != ".mp3" && extension != ".m4a")
            {
                return Result<string>.Failure("Only .mp3 and .m4a files are allowed.");
            }

            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "FileTest");

            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }

            var exactPath = Path.Combine(Directory.GetCurrentDirectory(), "FileTest", filename);
            await using (var stream = new FileStream(exactPath, FileMode.Create))
            {
                await request.File.CopyToAsync(stream, cancellationToken);
            }

            var mediaInfo = await FFmpeg.GetMediaInfo(exactPath);
            var audioLengthInSeconds = mediaInfo.Duration.TotalSeconds;


            var soundtrack = new Soundtrack()
            {
                Title = Path.GetFileNameWithoutExtension(filename),
                Extension = extension,
                LengthInSeconds = audioLengthInSeconds
            };

            await _repository.Add(soundtrack);


            return Result<string>.Success(filename);
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(ex.Message);
        }
    }
}