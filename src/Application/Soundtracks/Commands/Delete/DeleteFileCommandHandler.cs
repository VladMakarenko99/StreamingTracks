using Application.Abstractions;
using Application.DTOs.Result;
using MediatR;
using static Domain.Constants.DirectoryConstants;

namespace Application.Soundtracks.Commands.Delete;

public class DeleteFileCommandHandler(ISoundtrackRepository repository) : IRequestHandler<DeleteFileCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var soundtrack = await repository.GetById(request.Id);
            if (soundtrack == null)
            {
                return Result<string>.Failure("The soundtrack not found.");
            }

            var filepath = Path.Combine(Directory.GetCurrentDirectory(), GeneralMusicDirectory, soundtrack.Title);
        
            File.Delete(filepath + soundtrack.Extension);
            if (!string.IsNullOrEmpty(soundtrack.AlbumCoverFileName))
            {
                var albumCoverFilepath = Path.Combine(Directory.GetCurrentDirectory(), GeneralMusicDirectory, AlbumCoverDirectory, soundtrack.AlbumCoverFileName);
                File.Delete(albumCoverFilepath);
            }
            
            await repository.Delete(soundtrack);
        
            return Result<string>.Success($"Deleted: {soundtrack.Title}");
        }
        catch (Exception e)
        {
            return Result<string>.Failure($"Fail during deletion in file system: {e.Message}");
        }
    }
}