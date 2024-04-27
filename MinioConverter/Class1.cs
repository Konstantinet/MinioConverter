using Dt.Kiuss.Supervisor.Domain.Utils.File;
using Microsoft.Extensions.Configuration;
using MinioConverter.Domain.Data;
using System.IO;
using System.Net;

namespace MinioConverter
{
    public class Class1
    {
        public static void Main(string[] args)
        {
            /*var build = new ConfigurationBuilder();
            build.SetBasePath(Directory.GetCurrentDirectory());  
            build.AddJsonFile(CONFIG_FILE, true, true);
            Configs = build.Build();

            IConfiguration config = builder.Build();*/
            var endPoint = "";
            var accessKey = "";
            var secretKey = "";
            var connection = new MinioConnection(endPoint,accessKey,secretKey);


        }
    }
}