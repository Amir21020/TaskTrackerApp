using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using TaskTrackerApp.Application.Interfaces;
using TaskTrackerApp.Infrastructure.Configuration;

namespace TaskTrackerApp.Infrastructure.Services;

public sealed class MinioBlobStorage(
    IMinioClient minioClient,
    IOptions<MinioOptions> minioOptions,
    ILogger<MinioBlobStorage> logger) : IBlobStorage
{
    private readonly MinioOptions _options = minioOptions.Value;

    public async Task<bool> ExistsFileAsync(string fileName, CancellationToken ct = default)
    {
        logger.LogDebug("Checking if file {FileName} exists in bucket {BucketName}", fileName, _options.BucketName);

        var found = await ExistsBucketAsync(_options.BucketName);

        if (!found)
        {
            logger.LogInformation("Bucket {BucketName} not found. Creating bucket and setting public policy...", _options.BucketName);
            await MakeBucketAsync(_options.BucketName);
            await SetPolicyAsync(_options.BucketName);
        }

        try
        {
            var stateArgs = new StatObjectArgs()
                .WithBucket(_options.BucketName)
                .WithObject(fileName);

            await minioClient.StatObjectAsync(stateArgs);
            return true;
        }
        catch (Exception)
        {
            logger.LogDebug("File {FileName} not found in bucket {BucketName}", fileName, _options.BucketName);
            return false;
        }
    }

    public string GetPublicUrl(string fileName)
    {
        var protocol = _options.UseSSL ? "https" : "http";
        var host = _options.Endpoint.TrimEnd('/');
        var url = $"{protocol}://{host}/{_options.BucketName}/{fileName}";

        logger.LogDebug("Generated public URL for {FileName}: {Url}", fileName, url);
        return url;
    }

    public async Task UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken ct = default)
    {
        logger.LogInformation("Uploading file {FileName} to bucket {BucketName} (Content-Type: {ContentType})",
            fileName, _options.BucketName, contentType);
        var putObjectArgs = new PutObjectArgs()
            .WithObject(fileName)
            .WithBucket(_options.BucketName)
            .WithStreamData(fileStream)
            .WithObjectSize(fileStream.Length)
            .WithContentType(contentType);

        var found = await ExistsBucketAsync(_options.BucketName);

        if (!found)
        {
            logger.LogInformation("Bucket {BucketName} not found. Creating bucket and setting public policy...", _options.BucketName);
            await MakeBucketAsync(_options.BucketName);
            await SetPolicyAsync(_options.BucketName);
        }

        await minioClient.PutObjectAsync(putObjectArgs);

        logger.LogInformation("Successfully uploaded file {FileName} to Minio", fileName);
    }

    private async Task<bool> ExistsBucketAsync(string bucketName)
    {
        var args = new BucketExistsArgs().WithBucket(bucketName);
        return await minioClient.BucketExistsAsync(args);
    }

    private async Task MakeBucketAsync(string bucketName)
    {
        logger.LogInformation("Creating new bucket: {BucketName}", bucketName);
        var makeBucketArgs = new MakeBucketArgs().WithBucket(bucketName);
        await minioClient.MakeBucketAsync(makeBucketArgs);
    }

    private async Task SetPolicyAsync(string bucketName)
    {
        logger.LogInformation("Setting public read policy for bucket: {BucketName}", bucketName);

        var policyJson = $$"""
        {
            "Version": "2012-10-17",
            "Statement": [
                {
                    "Effect" : "Allow",
                    "Principal": { "AWS": ["*"] },
                    "Action" : [ "s3:GetObject" ],
                    "Resource" : "arn:aws:s3:::{{bucketName}}/*"
                }
            ]
        }
        """;

        var args = new SetPolicyArgs().WithPolicy(policyJson).WithBucket(bucketName);
        await minioClient.SetPolicyAsync(args).ConfigureAwait(false);
    }
}