using Minio;
using Minio.DataModel.Args;


namespace MinioConverter.Domain.Data
{
    internal class MinioConnection:IS3StoreConnection
    {
        static IMinioClient minio;
        public MinioConnection()
        {

        }
        public MinioConnection(string endpoint, string accessKey, string secretKey)
        {
            if (minio is null)
            {
                minio = new MinioClient().WithEndpoint(endpoint)
                                             .WithCredentials(accessKey, secretKey)
                                             .Build();
            }
        }
        public MinioClient Connect(string endpoint, string accessKey, string secretKey)
        {
            if (minio is null)
            {
                minio = new MinioClient().WithEndpoint(endpoint)
                                             .WithCredentials(accessKey, secretKey)
                                             .Build();
            }
            return (MinioClient)minio;
        }
        public async Task CreateFile(string bucketName, string objectName, string filePath, byte[] fileContent)
        {
            var contentType = "application/octet-stream";
            try
            {
                // Make a bucket on the server, if not already present.
                var beArgs = new BucketExistsArgs()
                    .WithBucket(bucketName);
                bool found = await minio.BucketExistsAsync(beArgs).ConfigureAwait(false);
                if (!found)
                {
                    var mbArgs = new MakeBucketArgs()
                        .WithBucket(bucketName);
                    await minio.MakeBucketAsync(mbArgs).ConfigureAwait(false);
                }
                // Upload a file to bucket.
                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithObjectSize(fileContent.Length)
                .WithStreamData(new MemoryStream(fileContent))
                    .WithContentType(contentType);
                await minio.PutObjectAsync(putObjectArgs).ConfigureAwait(false);

            }
            catch(Exception ex)
            {

            }
            }
        public async void DeleteFile(string bucketName,string objectName)
        {
            try
            {
                var beArgs = new BucketExistsArgs()
                    .WithBucket(bucketName);
                bool found = await minio.BucketExistsAsync(beArgs).ConfigureAwait(false);
                if (!found)
                {
                    var mbArgs = new MakeBucketArgs()
                        .WithBucket(bucketName);
                    await minio.MakeBucketAsync(mbArgs).ConfigureAwait(false);
                }

                var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(bucketName)
                    .WithObject(objectName);
                await minio.RemoveObjectAsync(removeObjectArgs).ConfigureAwait(false);
                Console.WriteLine("Successfully removed " + objectName);
            }
            catch (Exception ex) { }
        }
        public async Task<MemoryStream?> LoadFile(string bucketName,string objectName)
        {
            try
            {
                var beArgs = new BucketExistsArgs()
                    .WithBucket(bucketName);
                bool found = await minio.BucketExistsAsync(beArgs).ConfigureAwait(false);
                if (!found)
                {
                    var mbArgs = new MakeBucketArgs()
                        .WithBucket(bucketName);
                    await minio.MakeBucketAsync(mbArgs).ConfigureAwait(false);
                }
                var inputStream = new MemoryStream();
                var args = new GetObjectArgs()
                     .WithBucket(bucketName)
                     .WithObject(objectName)
                     .WithCallbackStream((stream) =>
                     {
                         stream.CopyTo(inputStream);
                     });

                var stat = await minio.GetObjectAsync(args).ConfigureAwait(false);


                return inputStream;
            }
            catch(Exception ex) { return null; }
        }
    }
}
