using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BLL.Utilities.AWSHelper
{
    public class AWSHelper : IAWSHelper
    {
        private readonly IAmazonS3 _s3client;
        private readonly string _bucketName;

        public AWSHelper(IAmazonS3 s3client, IConfiguration configuration)
        {
            _s3client = s3client;
            _bucketName = configuration["AWS:BucketName"]!;
        }

        public (bool, string) UploadImage(IFormFile imageFile)
        {
            try
            {
                var uploadImageRequest = new PutObjectRequest()
                {
                    BucketName = _bucketName,
                    Key = imageFile.FileName,
                    InputStream = imageFile.OpenReadStream(),
                };

                uploadImageRequest.Metadata.Add("Content-type", imageFile.ContentType);
                _s3client.PutObjectAsync(uploadImageRequest).Wait();
                return (true, $"https://{_bucketName}.s3.amazonaws.com/{imageFile.FileName}");
            }
            catch (Exception ex)
            {
                return (false, ex.ToString());
            }
        }
        public (bool, string) DeleteImage(string imageUrl)
        {
            try
            {
                // Extract the key (file name) from the URL
                Uri uri = new Uri(imageUrl);
                string key = uri.AbsolutePath.TrimStart('/');

                // Create the delete object request
                DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = key
                };

                // Delete the object
                _s3client.DeleteObjectAsync(deleteObjectRequest).Wait();

                return (true, "");
            }
            catch (Exception ex)
            {
                // Handle exception
                return (false, ex.ToString());
            }
        }
    }
}
