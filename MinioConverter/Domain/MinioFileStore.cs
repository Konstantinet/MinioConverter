

namespace MinioConverter.Domain.Domain
{
    public class MinioFileStore 
    {
        public Task CreateFile(Guid fileId, Guid drillingProjectId, byte[] fileContent, string fileName)
        {
            throw new NotImplementedException();
        }

        public Task CreateFile(Guid fileId, Guid drillingProgramId, byte[] fileContent, string fileName, FileType fileType)
        {
            throw new NotImplementedException();
        }

        public void DeleteFile(Guid fileId, Guid drillingProjectId, string fileName)
        {
            throw new NotImplementedException();
        }

        public void DeleteFile(Guid fileId, Guid drillingProgramId, string fileName, FileType fileType)
        {
            throw new NotImplementedException();
        }

        public Task<FileContentDto> LoadFile(Guid fileId, Guid drillingProjectId, string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<FileContentDto> LoadFile(Guid fileId, Guid drillingProgramId, string fileName, FileType fileType)
        {
            throw new NotImplementedException();
        }
    }
}
