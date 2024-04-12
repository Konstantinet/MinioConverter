using Dt.Kiuss.Supervisor.Domain.Utils.File;
using Dt.Kpsirs.Common.File.Dto;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dt.Kiuss.Supervisor.Domain.Utils.File;

internal class MinioFileStoreKiuss : IFileStore
{
    static IMinioClient minio;

    public MinioFileStoreKiuss()
    {

    }
    public MinioFileStoreKiuss(string endpoint,string accessKey,string secretKey)
    {
        ConnectToMinio(endpoint, accessKey, secretKey); 
    }

    public MinioClient ConnectToMinio(string endpoint, string accessKey, string secretKey)
    {
        if (minio is null)
        {
            minio = new MinioClient().WithEndpoint(endpoint)
                                         .WithCredentials(accessKey, secretKey)
                                         .Build();
        }
        return (MinioClient)minio;
    }
    
    public async Task CreateFile(Guid fileId, Guid drillingProjectId, byte[] fileContent, string fileName)
    {
        var bucketName = $"kiuss";
        var objectName = $"{drillingProjectId}\\{fileId}.{fileName.Split('.').Last()}";
        var filePath = $"F:\\dp_emulation\\kiuss\\{drillingProjectId}\\{fileId}.{fileName.Split('.').Last()}";
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
            Console.WriteLine("Successfully uploaded " + objectName);
        }
        catch (MinioException e)
        {
            Console.WriteLine("File Upload Error: {0}", e.Message);
        }

    }

    public async void DeleteFile(Guid fileId, Guid drillingProjectId, string fileName)
    {
        var bucketName = $"kiuss";
        var objectName = $"{drillingProjectId}\\{fileId}.{fileName.Split('.').Last()}";
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
        catch (MinioException e)
        {
            Console.WriteLine("File Upload Error: {0}", e.Message);
        }
    }

    public async Task<FileContentDto> LoadFile(Guid fileId, Guid drillingProjectId, string fileName)
    {
        var bucketName = $"kiuss";
        var objectName = $"{drillingProjectId}\\{fileId}.{fileName.Split('.').Last()}";
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
            Console.WriteLine("Successfully removed " + objectName);
            string result  = System.Text.Encoding.Default.GetString( inputStream.ToArray());
            Console.WriteLine(result);
            
                return new FileContentDto(fileName, inputStream.ToArray());
        }
        catch (MinioException e)
        {
            Console.WriteLine("File Upload Error: {0}", e.Message);
            return null;
        }
        
    }
}

