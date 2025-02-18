using Amazon;
using Amazon.Internal;
using Amazon.S3;
using Amazon.S3.Model;
using Application.Abstractions;

namespace Infrastructure.Repository;

public class AwsS3Service(IAmazonS3 amazonS3) : IAwsS3Service
{
    private const string BucketName = "streaming-audios";

    public async Task<string> GetPreSignedUrlAsync(string key, int expirationInMinutes)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = BucketName,
            Key = key,
            Expires = DateTime.UtcNow.AddMinutes(expirationInMinutes),
            Verb = HttpVerb.GET,
            ResponseHeaderOverrides = new ResponseHeaderOverrides
            {
                ContentDisposition = "inline"
            }
        };

        return await amazonS3.GetPreSignedURLAsync(request);
    }
}