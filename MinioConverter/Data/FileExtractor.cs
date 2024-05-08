using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinioConverter.Domain.Data
{
    public class FileExtractor
    {
        string path;
        public FileExtractor(string path) {
        this.path = path;
        }  
        public byte[] GetFile()
        {
            return File.ReadAllBytes(path);
        }
        
    }
}
