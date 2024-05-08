using Dt.Kiuss.Supervisor.Domain.Utils.File;
using Dt.Kpsirs.Common.File.Dto;
using Minio.Exceptions;
using MinioConverter.Domain.Data;
using System.ComponentModel.Design;

namespace MinioConverter.Domain.Domain;

public class MinioFileStoreKiuss : MinioFileStore,IFileStore
{
    static IS3StoreConnection connection;
    string bucketName = "kiuss";
    string initialPath = "F:\\dp_emulation\\kiuss";

    public MinioFileStoreKiuss(IS3StoreConnection connection)
    {
        MinioFileStoreKiuss.connection = connection;
    }
    public MinioFileStoreKiuss(string endpoint, string accessKey, string secretKey)
    {
        if (connection == null) {
            connection = new MinioConnection(endpoint, accessKey, secretKey);
        }
        else{
            connection.Connect(endpoint, accessKey, secretKey);
        }
    }

    public async Task CreateFile(Guid fileId, Guid drillingProjectId, byte[] fileContent, string fileName)
    {
        try
        {
            var objectName = $"{drillingProjectId}\\{fileId}.{fileName.Split('.').Last()}";
            var filePath = $"{initialPath}\\{drillingProjectId}\\{fileId}.{fileName.Split('.').Last()}";
            await connection.CreateFile(bucketName, objectName, filePath, fileContent);
            Console.WriteLine("Successfully uploaded " + objectName);
        }
        catch (Exception e)
        {
            Console.WriteLine("File Upload Error: {0}", e.Message);
        }

    }

    public async void DeleteFile(Guid fileId, Guid drillingProjectId, string fileName)
    {
        var objectName = $"{drillingProjectId}\\{fileId}.{fileName.Split('.').Last()}";
        try
        {
            connection.DeleteFile(bucketName, objectName);
            Console.WriteLine("File deleted: ");
        }
        catch (MinioException e)
        {
            Console.WriteLine("File Upload Error: {0}", e.Message);
        }
    }

    public async Task<FileContentDto> LoadFile(Guid fileId, Guid drillingProjectId, string fileName)
    {
        var objectName = $"{drillingProjectId}\\{fileId}.{fileName.Split('.').Last()}";
        try
        {
            var stream = await connection.LoadFile(bucketName, objectName);
            if (stream != null)
            {
                Console.WriteLine("Successfully removed " + objectName);
                string result = System.Text.Encoding.Default.GetString(stream.ToArray());
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

