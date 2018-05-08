using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AwsS3WithNetCore
{
    class Program
    {
        private static  string bucketName = Constants.bucketName;
        private static string keyName1 = "EnesTEST1";
        private static string keyName2 = "EnesTEST2";
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1; //Your bucket region
      public static AWSCredentials awsCredentials = new BasicAWSCredentials(Constants.accessKey,Constants.secretKey);
 

        private static IAmazonS3 client;
        static void Main(string[] args)
        {
            Console.WriteLine("Aws S3 servisine dosya atan .Net Core Console Uygulaması Başladı");

            client = new AmazonS3Client(awsCredentials, bucketRegion);

            // Choose Whatever you want. I suppose to you should try each method
            WritingAnObjectAsync().Wait();
            DeleteAnObjectAsync("EnesTEST1").Wait();
            ListObjectsAsync().Wait();
            Console.ReadKey();
        }

        static async Task WritingAnObjectAsync(string bodyMessage = "", string ResimPath = "")
        {
            //Write your İmage Path
            string Path = String.IsNullOrEmpty(ResimPath) ? @"C:\Users\enes.aysan\Desktop\yildizLogo.png" : ResimPath;
            try
            {
                // 1. Put object-specify only key name for the new object.
                var putRequest1 = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName1,
                    ContentBody = String.IsNullOrEmpty(bodyMessage) ? " ENES AWS S3" : bodyMessage,

                };

                PutObjectResponse response1 = await client.PutObjectAsync(putRequest1);

                // 2. Put the object-set ContentType and add metadata.
                var putRequest2 = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName2,
                    CannedACL = S3CannedACL.PublicRead,
                    FilePath = Path,
                    TagSet = new List<Tag>{
                                    new Tag { Key = "Test", Value = "S3Test"} }
                    //ContentType = "image/png"
                };
                putRequest2.Metadata.Add("x-amz-meta-title", "AwsS3NetConsole");
                PutObjectResponse response2 = await client.PutObjectAsync(putRequest2);

                Console.WriteLine("İşlem Başarı ile gerçekleştirildi");

            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine(
                        "Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "Unknown encountered on server. Message:'{0}' when writing an object"
                    , e.Message);
            }
        }
        static async Task DeleteAnObjectAsync(string key)
        {
            try
            {
                // 1. Delete object-specify only key name for the object.
                var deleteRequest1 = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = key
                };

                DeleteObjectResponse response1 = await client.DeleteObjectAsync(deleteRequest1);

                Console.WriteLine("Silme İşlem Başarı ile gerçekleştirildi");

            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine(
                        "Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "Unknown encountered on server. Message:'{0}' when writing an object"
                    , e.Message);
            }
        }

        static async Task ListObjectsAsync(string prefix = "")
        {
            try
            {

                var list = new ListObjectsRequest
                {
                    BucketName = bucketName,
                    Prefix = prefix
                };

                ListObjectsResponse response1 = await client.ListObjectsAsync(list);
                foreach (var item in response1.S3Objects)
                {
                    Console.WriteLine(item.BucketName + " " + item.Key);
                }

                Console.WriteLine("Listeleme İşlemi Başarı ile gerçekleştirildi");

            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine(
                        "Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "Unknown encountered on server. Message:'{0}' when writing an object"
                    , e.Message);
            }
        }

    }
}