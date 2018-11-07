using System;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace TransferAcceleration
{
    class TransferAccelerationTest
    {
        private const string bucketName = "s3transferaccelerationbucket";
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;
        private static IAmazonS3 s3Client;
        public static void Main()
        {

           // EnableAcceleration();    
            Console.ReadLine();

            s3Client = new AmazonS3Client(new AmazonS3Config
            {
                RegionEndpoint = bucketRegion,
                UseAccelerateEndpoint = true
            });

            UploadtoS3usingTransAccelerate();

            Console.ReadLine();

            DownloadfromS3usingTransAccelerate();

            Console.ReadLine();

            SuspendAcceleration();

            Console.ReadLine();
        }

        static void UploadtoS3usingTransAccelerate()
        {
            s3Client.PutObject(new PutObjectRequest() { BucketName = bucketName, FilePath = @"C:\Namrata\AWS\S3\transfer acceleration\martha vineyard.jpg", Key = "martha" });
            Console.WriteLine("file uploaded successfully");
        }

        static void DownloadfromS3usingTransAccelerate()
        {
            GetObjectResponse response = s3Client.GetObject ( bucketName, "Toronto.jpg" );
            response.WriteResponseStreamToFile(@"C:\Namrata\AWS\S3\transfer acceleration\download.jpg");
            Console.WriteLine(response.Key);
        }

        static void SuspendAcceleration()
        {
            try
            {
                s3Client = new AmazonS3Client(bucketRegion);
                var putRequest = new PutBucketAccelerateConfigurationRequest
                {
                    BucketName = bucketName,
                    AccelerateConfiguration = new AccelerateConfiguration
                    {
                        Status = BucketAccelerateStatus.Suspended
                    }
                };
                s3Client.PutBucketAccelerateConfigurationAsync(putRequest);
            }
                
            catch (AmazonS3Exception amazonS3Exception)
            {
                Console.WriteLine(
                    "Error occurred. Message:'{0}' when setting transfer acceleration",
                    amazonS3Exception.Message);
            }
        }

        static void EnableAcceleration()
        {
            try
            {
                s3Client = new AmazonS3Client(bucketRegion);
                var putRequest = new PutBucketAccelerateConfigurationRequest
                {
                    BucketName = bucketName,
                    AccelerateConfiguration = new AccelerateConfiguration
                    {
                        Status = BucketAccelerateStatus.Enabled
                    }
                };
                s3Client.PutBucketAccelerateConfigurationAsync(putRequest);

                // To enable transfer acceleration thru code can take sometime.
                   
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                Console.WriteLine(
                    "Error occurred. Message:'{0}' when setting transfer acceleration",
                    amazonS3Exception.Message);
            }
        }
    }
}