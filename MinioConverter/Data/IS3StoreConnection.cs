using Minio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinioConverter.Domain.Data
{
    public interface IS3StoreConnection
    {
        public MinioClient Connect(string endpoint, string accessKey, string secretKey);

        public Task CreateFile(string bucketName, string objectName, string filePath, byte[] fileContent);

        public void DeleteFile(string bucketName, string objectName);

        public Task<MemoryStream?> LoadFile(string bucketName, string objectName);
    }
}
