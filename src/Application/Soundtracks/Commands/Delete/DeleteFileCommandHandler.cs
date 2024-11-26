using System.Runtime.CompilerServices;
using Application.Abstractions;
using Application.DTOs.Result;
using MediatR;
using static Domain.Constants.DirectoryConstants;

namespace Application.Soundtracks.Commands.Delete;

public class DeleteFileCommandHandler(ISoundtrackRepository repository, IUnitOfWork unitOfWork, ICacheService cache) : IRequestHandler<DeleteFileCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        
        List<(string FilePath, bool IsDeleted)> deletedFiles = new();
        try
        {
            var soundtrack = await repository.GetById(request.Id);
            if (soundtrack == null)
            {
                return Result<string>.Failure("The soundtrack not found.");
            }

            var musicFilepath = Path.Combine(Directory.GetCurrentDirectory(), GeneralMusicDirectory, soundtrack.Title) + soundtrack.Extension;
        
            if (File.Exists(musicFilepath))
            {
                File.Copy(musicFilepath, musicFilepath + ".bak", overwrite: true);
                File.Delete(musicFilepath);
                deletedFiles.Add((musicFilepath, true));
            }
            
            if (!string.IsNullOrEmpty(soundtrack.AlbumCoverFileName))
            {
                var albumCoverFilePath = Path.Combine(Directory.GetCurrentDirectory(), GeneralMusicDirectory, AlbumCoverDirectory, soundtrack.AlbumCoverFileName);
                if (File.Exists(albumCoverFilePath))
                {
                    File.Copy(albumCoverFilePath, albumCoverFilePath + ".bak", overwrite: true);
                    File.Delete(albumCoverFilePath);
                    deletedFiles.Add((albumCoverFilePath, true));
                }
            }
            
            await repository.Delete(soundtrack);
            await unitOfWork.CommitAsync(cancellationToken);
            
            await cache.RefreshSoundtracksAsync(cancellationToken);
            
            return Result<string>.Success($"Deleted: {soundtrack.Title}");
        }
        catch (Exception e)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            
            foreach (var (filePath, isDeleted) in deletedFiles)
            {
                if (isDeleted && !File.Exists(filePath))
                {
                    try
                    {
                        File.Copy(filePath + ".bak", filePath);
                        File.Delete(filePath + ".bak");
                    }
                    catch (Exception restoreException)
                    {
                        Console.WriteLine($"Failed to restore file: {filePath}. Error: {restoreException.Message}");
                    }
                }
            }
            
            return Result<string>.Failure($"Fail during deletion in file system: {e.Message}");
        }
    }
}