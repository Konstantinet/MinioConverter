
using MinioConverter.UI.CLI;
using MinioConverter.UI;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;

public class Program
{
    public static void Main(string[] args)
    {

        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("C:\\Users\\uzver\\source\\repos\\MinioConverter\\MinioConverter.UI\\appsettings.json", true, true);

        IConfiguration config = builder.Build();
        UIHandler handler = new CLICommandHandler();
        //UIHandler handler = new ConsoleAppHandler();
        handler.Handle(config,args).Wait();

        //var store = new MinioFileStoreKiuss("localhost:9000", "ROOTUSER", "CHANGEME123");
        //var content = File.ReadAllBytes("F:\\dp_emulation\\4ced9581-a08b-433b-b1a6-c99e1e393684\\2e455c4b-cfc7-4b68-be8e-9a9013e91644.txt");
        //store.CreateFile(new Guid("2e455c4b-cfc7-4b68-be8e-9a9013e91644"), new Guid("4ced9581-a08b-433b-b1a6-c99e1e393684"), content, "2e455c4b-cfc7-4b68-be8e-9a9013e91644.txt");
        //store.DeleteFile(new Guid("2e455c4b-cfc7-4b68-be8e-9a9013e91644"), new Guid("4ced9581-a08b-433b-b1a6-c99e1e393684"),"2e455c4b-cfc7-4b68-be8e-9a9013e91644.txt");
    }
}
