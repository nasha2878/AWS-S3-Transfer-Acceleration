using System;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace TransferAcceleration
{
   

    namespace Amazon.DocSamples.S3
    {
        class TransferAccelerationTest
        {
            private const string bucketName = "s3transferaccelerationbucket";
            private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;
            private static IAmazonS3 s3Client;
            public static void Main()
            {
        
                Console.ReadLine();

                s3Client = new AmazonS3Client(new AmazonS3Config
                {
                    RegionEndpoint = bucketRegion,
                    UseAccelerateEndpoint = true
                });

                var getRequest = new GetBucketAccelerateConfigurationRequest
                {
                    BucketName = bucketName
                };
                var response = s3Client.GetBucketAccelerateConfigurationAsync(getRequest);

                Console.WriteLine("Acceleration state = '{0}' ", response.Status);

                    UploadtoS3usingTransAccelerate();
                    DownloadfromS3usingTransAccelerate();                
            }

            static void UploadtoS3usingTransAccelerate()
            {
                s3Client.PutObject(new PutObjectRequest() { BucketName = bucketName, FilePath = @"C:\Namrata\AWS\S3\transfer acceleration\sample.jpg", Key = "sample" });
                Console.WriteLine("file uploaded successfully");
            }

            static void DownloadfromS3usingTransAccelerate()
            {
                GetObjectResponse response = s3Client.GetObject ( bucketName, "sample" );
                response.WriteResponseStreamToFile(@"C:\Namrata\AWS\S3\transfer acceleration\download.jpg");
                Console.WriteLine(response.Key);
            }
            static  void EnableAccelerationAsync()
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
}