using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace TechXpress.Core.Services
{
    public class S3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public S3Service()
        {
            _bucketName = Environment.GetEnvironmentVariable("AWS_BUCKET_NAME");

            _s3Client = new AmazonS3Client(
                Environment.GetEnvironmentVariable("AWS_ACCESS_KEY"),
                Environment.GetEnvironmentVariable("AWS_SECRET_KEY"),
                Amazon.RegionEndpoint.GetBySystemName(Environment.GetEnvironmentVariable("AWS_REGION"))
            );
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
    }
}
