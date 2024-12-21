using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Exception = System.Exception;
using static Domain.Constants.DirectoryConstants;

namespace Infrastructure;

public class BackupDeletionService(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var generalMusicDirectory = Path.Combine(Directory.GetCurrentDirectory(), GeneralMusicDirectory);
                var albumCoverDirectory = Path.Combine(Directory.GetCurrentDirectory(), GeneralMusicDirectory,
                    AlbumCoverDirectory);

                DeleteBackupFiles(generalMusicDirectory);

                if (Directory.Exists(albumCoverDirectory))
                {
                    DeleteBackupFiles(albumCoverDirectory);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in BackupDeletionService: {e.Message}");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }

    private void DeleteBackupFiles(string directoryPath)
    {
        if (!Directory.Exists(directoryPath)) return;
        var backupFiles = Directory.GetFiles(directoryPath, "*.bak");

        foreach (var backupFile in backupFiles)
        {
            File.Delete(backupFile);
            Console.WriteLine($"Deleted backup file: {backupFile}");
        }
    }
}