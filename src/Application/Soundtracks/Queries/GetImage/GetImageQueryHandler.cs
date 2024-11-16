using Application.DTOs.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Domain.Constants.DirectoryConstants;

namespace Application.Soundtracks.Queries.GetImage;

public class GetImageQueryHandler : IRequestHandler<GetImageQuery, Result<byte[]>>
{
    public async Task<Result<byte[]>> Handle(GetImageQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), GeneralMusicDirectory, AlbumCoverDirectory, request.FileName);

            if (!File.Exists(filePath))
            {
                return Result<byte[]>.Failure("Image not found.");
            }

            return Result<byte[]>.Success(await File.ReadAllBytesAsync(filePath, cancellationToken));
        }
        catch (Exception e)
        {
            return Result<byte[]>.Failure("Failure: " + e.Message);
        }
    }
}