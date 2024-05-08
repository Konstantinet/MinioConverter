using Dt.Kpsirs.Common.File;
using Dt.Kpsirs.Common.File.Dto;
using Dt.Kpsirs.Common.File.Files;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using MinioConverter.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinioConverter.Domain.Domain
{
    public class MinioFileStoreKpsirs : IFileStore
    {
        static IS3StoreConnection connection;
        string bucketName = "kpsirs";
        string initialPath = "F:\\dp_emulation\\kiuss";

        public MinioFileStoreKpsirs(IS3StoreConnection connection)
        {
            MinioFileStoreKpsirs.connection = connection;
        }
        public MinioFileStoreKpsirs(string endpoint, string accessKey, string secretKey)
        {
            connection.Connect(endpoint, accessKey, secretKey);
        }
        public async Task CreateFile(Guid fileId, Guid drillingProgramId, byte[] fileContent, string fileName, FileType fileType)
        {
            
            var objectName = $"{drillingProgramId}\\{fileType}\\{fileId}.{fileName.Split('.').Last()}";
            var filePath = $"{initialPath}\\{drillingProgramId}\\{fileType}\\{fileId}.{fileName.Split('.').Last()}";

            try
            {
                await connection.CreateFile(bucketName, objectName, filePath, fileContent);
                Console.WriteLine("Successfully uploaded " + objectName);
            }
            catch (Exception e)
            {
                Console.WriteLine("File Upload Error: {0}", e.Message);
            }
        }

        public async void DeleteFile(Guid fileId, Guid drillingProgramId, string fileName, FileType fileType)
        {
            var objectName = $"{drillingProgramId}\\{fileType}\\{fileId}.{fileName.Split('.').Last()}";
            try
            {
                connection.DeleteFile(bucketName, objectName);
                Console.WriteLine("Successfully removed " + objectName);
            }
            catch (Exception e)
            {
                Console.WriteLine("File Upload Error: {0}", e.Message);
            }
        }

        public async Task<FileContentDto> LoadFile(Guid fileId, Guid drillingProgramId, string fileName, FileType fileType)
        {
            var objectName = $"{drillingProgramId}\\{fileType}\\{fileId}.{fileName.Split('.').Last()}";
            try
            {
                var stream = await connection.LoadFile(bucketName, objectName);
                if (stream != null)
                {
                    Console.WriteLine("Successfully removed " + objectName);
                    string result = Encoding.Default.GetString(stream.ToArray());
                    Console.WriteLine(result);

                    return new FileContentDto(fileName, stream.ToArray());
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("File Upload Error: {0}", e.Message);
                return null;
            }
        }
    }
}
