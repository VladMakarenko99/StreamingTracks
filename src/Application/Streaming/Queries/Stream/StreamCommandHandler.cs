using Application.Abstractions;
using Application.DTOs.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Application.Streaming.Queries.Stream;

public class StreamCommandHandler(ISoundtrackRepository repository) : IRequestHandler<StreamCommand, Result<FileStreamResult>>
{
    public async Task<Result<FileStreamResult>> Handle(StreamCommand request, CancellationToken cancellationToken)
    {
        var soundtrack = await repository.GetById(request.Id);
        if (soundtrack is null)
        {
            return Result<FileStreamResult>.Failure("Soundtrack not found.");
        }

        var fileName = soundtrack.Title + soundtrack.Extension;
        
        var path = Path.Combine(Directory.GetCurrentDirectory(), "FileTest", fileName);
        var fs = new FileStream(path, FileMode.Open);

        var fileStreamResult = new FileStreamResult(fs, "audio/mp3")
        {
            EnableRangeProcessing = true,
        };
        
        return Result<FileStreamResult>.Success(fileStreamResult);
    }
}