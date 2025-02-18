using Amazon.S3.Model;

namespace Application.Abstractions;

public interface IAwsS3Service
{
    Task<string> GetPreSignedUrlAsync(string key, int expirationInMinutes = 60);
}