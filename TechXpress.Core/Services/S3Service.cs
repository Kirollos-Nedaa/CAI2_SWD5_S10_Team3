using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using System.Net;

namespace TechXpress.Core.Services
{
    public class S3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly ILogger<S3Service> _logger;

        public S3Service(ILogger<S3Service> logger)
        {
            _bucketName = Environment.GetEnvironmentVariable("AWS_BUCKET_NAME");

            _s3Client = new AmazonS3Client(
                Environment.GetEnvironmentVariable("AWS_ACCESS_KEY"),
                Environment.GetEnvironmentVariable("AWS_SECRET_KEY"),
                Amazon.RegionEndpoint.GetBySystemName(Environment.GetEnvironmentVariable("AWS_REGION"))
            );

            _logger = logger;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string subfolder = "")
        {
            var key = $"{subfolder}/{Guid.NewGuid()}_{file.FileName}";

            using (var newMemoryStream = new MemoryStream())
            {
                await file.CopyToAsync(newMemoryStream);
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = newMemoryStream,
                    Key = key,
                    BucketName = _bucketName
                };

                var fileTransferUtility = new TransferUtility(_s3Client);
                await fileTransferUtility.UploadAsync(uploadRequest);
            }

            return $"https://{_bucketName}.s3.amazonaws.com/{key}";
        }

        public async Task DeleteProductAsync(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return;

            var uri = new Uri(imageUrl);
            var key = WebUtility.UrlDecode(uri.AbsolutePath.TrimStart('/'));

            // Debuging
            //_logger.LogInformation($"[S3 Delete] Extracted key: {key}");
            //_logger.LogInformation($"[S3 Delete] From URL: {imageUrl}");
            //_logger.LogInformation($"[S3 Delete] Using bucket: {_bucketName} in region: {_s3Client.Config.RegionEndpoint.SystemName}");

            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };

            await _s3Client.DeleteObjectAsync(request);
        }
    }
}
