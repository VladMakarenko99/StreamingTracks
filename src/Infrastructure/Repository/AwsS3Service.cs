using Amazon.S3;
using Amazon.S3.Model;
using Application.Abstractions;
using Domain.Constants;
using static Domain.Constants.AwsConstants;

namespace Infrastructure.Repository;

public class AwsS3Service(IAmazonS3 amazonS3) : IAwsS3Service
{
    public async Task<string> UploadFileAsync(Stream fileStream, string key, string contentType, CancellationToken cancellationToken)
    {
        var request = new PutObjectRequest
        {
            BucketName = BucketName,
            Key = key,
            InputStream = fileStream,
            ContentType = contentType,
            CannedACL = S3CannedACL.PublicRead
        };
        
        await amazonS3.PutObjectAsync(request, cancellationToken);
        
        return GetFileUrlAsync(key);
    }

    public async Task<bool> DeleteFileAsync(string key, CancellationToken cancellationToken)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = BucketName,
            Key = key
        };

        await amazonS3.DeleteObjectAsync(request, cancellationToken);
        return true;
    }
    
    public string GetFileUrlAsync(string key)
    {
        return $"https://{BucketName}.s3.{Region}.amazonaws.com/{key}";
    }
}