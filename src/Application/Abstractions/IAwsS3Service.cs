using Amazon.S3.Model;

namespace Application.Abstractions;

public interface IAwsS3Service
{
    string GetFileUrlAsync(string key);
    Task<string> UploadFileAsync(Stream fileStream, string key, string contentType, CancellationToken cancellationToken);

    Task<bool> DeleteFileAsync(string key, CancellationToken cancellationToken);
}